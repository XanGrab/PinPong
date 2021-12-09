using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    
    public static GameManager Instance {
    get {
        if (_instance == null) {
        Debug.LogError( "Game Manager is NULL!");
        }
        return _instance;
        }
    }

    [Header("Ball")]
    public GameObject ballPrefab;
    public GameObject ball;
    public GameObject displayPrefab;
    public GameObject launchTxt;

    [Header("Player Input")]
    private PlayerInput playerInput;
    private PlayerControls controls;

    [Header("Left Player")]
    public GameObject lefty;
    public GameObject leftGoal;


    [Header("Right Player")]
    public GameObject righty;
    public GameObject rightGoal;

    [Header("UI")]
    public GameObject gameMenu;
    public GameObject lScoreTxt;
    public GameObject rScoreTxt;
    public GameObject resetMenu;
    public GameObject pauseMenu;
    public Settings settings;
    private int lScore;
    private int rScore;

    [Header("Timer")]
    //public Text timerText;
    public float timeValue;

    [Header("Targets")]
    public GameObject targetManager;
    public GameObject pointsTargetManager;
    public GameObject powerupManager;
    public GameObject currentLayout;
    public GameObject pointTargetsCurrentLayout;
    public GameObject powerupCurrentLayout;
    public int targetHealthPickUp;

    private static bool playing;
    public static bool gamePaused;
    AudioManager am;

    void Awake(){
        settings = FindObjectOfType<Settings>();
        if(settings == null){
            Debug.LogError("null settings");
            return;
        }
        settings.gameObject.SetActive(false);
        
        controls = new PlayerControls();
        _instance = this;
        //controls.Player.Pause.canceled += ctx => OnPause();
    }
    void Start(){
        playing = true;
        gamePaused = false;
        gameMenu.SetActive(true);
        resetMenu.SetActive(false);

        //lefty.GetComponent<PlayerInput>().SwitchCurrentControlScheme(controlScheme: "Left Keyboard", Keyboard.current);
        //righty.GetComponent<PlayerInput>().SwitchCurrentControlScheme(controlScheme: "Right Keyboard", Keyboard.current);
        am = FindObjectOfType<AudioManager>();
        timeValue = 4f;
        currentLayout = targetManager.transform.GetChild(0).gameObject;
        pointsTargetManager = GameObject.Find("Points Target Manager");
        pointTargetsCurrentLayout = pointsTargetManager.transform.GetChild(0).gameObject;
        powerupManager = GameObject.Find("Powerup Manager");
        powerupCurrentLayout = powerupManager.transform.GetChild(Random.Range(0, pointsTargetManager.transform.childCount - 1)).gameObject;
    }

    void Update(){
        if(playing && (!gamePaused)){

            if(Mathf.FloorToInt(timeValue) > 0){
                launchTxt.GetComponent<TextMeshProUGUI>().text = Mathf.FloorToInt(timeValue).ToString();
                timeValue -= Time.deltaTime;
            }if(Mathf.FloorToInt(timeValue) == 0){
                timeValue = -1;
                
                launchTxt.SetActive(false);
                Debug.Log("Seting a paddle velocity to zero!");
                Debug.Log("Calling Ball Reset");
                ball = Instantiate(ballPrefab, new Vector3(0.0f, 0.0f, 1f), Quaternion.identity);
                //ball.GetComponent<Ball>().Reset();
            }
            /*
            timeValue -= Time.deltaTime;
            DisplayTime(timeValue);

            if(timeValue < 1){
                if(lScore != rScore){
                    playing = false;
                    StartCoroutine(EndMatch());
                }else{
                    timeValue += 30;
                }
            }*/
        }
        // Debug.Log(ball.GetComponent<Ball>().velo);
        //  Debug.Log(ball.GetComponent<Ball>().speed);
    }

    public void leftScored(){
        if(righty.GetComponent<HP>().hp <= 0){
            //Debug.Log("Health: " + righty.GetComponent<Health>().health);
            EndMatch();
        }
        am.Play("Break");
        //Debug.Log("Right HP" + righty.GetComponent<HP>().hp);
        //lScore += ball.GetComponent<Ball>().score;
        righty.GetComponent<HP>().hp -= ball.GetComponent<Ball>().score;
        righty.GetComponent<HP>().UpdateHealth();
        lScoreTxt.GetComponent<TextMeshProUGUI>().text = lScore.ToString();
        //ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Destroy(ball);
        launchTxt.SetActive(true);
        timeValue = 4;
        //launchTxt.SetActive(true);
        //ball.GetComponent<Ball>().Reset();
        resetTargets(currentLayout);
        resetPointTargets(pointTargetsCurrentLayout);
        resetPowerUp(powerupCurrentLayout);
    }

    public void rightScored(){
        if(lefty.GetComponent<HP>().hp <= 0){
            //Debug.Log("Health: " + righty.GetComponent<Health>().health);
            EndMatch();
        }
        am.Play("Break");
        //Debug.Log("Left HP: " + lefty.GetComponent<HP>().hp);
        rScore += ball.GetComponent<Ball>().score;
        //lefty.GetComponent<HP>().hp--;
        lefty.GetComponent<HP>().hp -= ball.GetComponent<Ball>().score;
        lefty.GetComponent<HP>().UpdateHealth();
        rScoreTxt.GetComponent<TextMeshProUGUI>().text = rScore.ToString();
        //ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Destroy(ball);
        launchTxt.SetActive(true);
        timeValue = 4;
        //ball.GetComponent<Ball>().Reset();
        resetTargets(currentLayout);
        resetPointTargets(pointTargetsCurrentLayout);
        resetPowerUp(powerupCurrentLayout);
    }

    public void DisplayPoints(Vector3 position){
        GameObject display;
        display = Instantiate(displayPrefab, position, Quaternion.identity);
        display.GetComponentInChildren<TextMeshPro>().text = ball.GetComponent<Ball>().score.ToString();
        display.GetComponentInChildren<Animator>().Play("Ball Points", 0, 0f);
    }
    public void DisplayDamage(Vector3 position){
        GameObject display;

        if(position.x < 0){
            position.x += 1.5f;  
        }else{
            position.x -= 1.5f;  
        }
        display = Instantiate(displayPrefab, position, Quaternion.identity);
        display.GetComponentInChildren<TextMeshPro>().text = "-" + ball.GetComponent<Ball>().score.ToString();
        display.GetComponentInChildren<Animator>().Play("Damaged", 0, 0f);
    }
     public void LeftHPPickUp(){
        lefty.GetComponent<HP>().hp += targetHealthPickUp;
        lefty.GetComponent<HP>().UpdateHealth();
        //lScoreTxt.GetComponent<TextMeshProUGUI>().text = lScore.ToString();
    }

    public void RightHPPickUp(){
        righty.GetComponent<HP>().hp += targetHealthPickUp;
        righty.GetComponent<HP>().UpdateHealth();
        //rScoreTxt.GetComponent<TextMeshProUGUI>().text = rScore.ToString();
    }

    /**
     * Timer Functionality
     */
    /*void DisplayTime(float timeToDisplay){
        if(timeToDisplay < 0){
            timeToDisplay = 0;
        }

        int min = Mathf.FloorToInt(timeToDisplay / 60);
        int sec = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", min, sec);
    }*/

    public void OnPause(){
        Debug.Log("OnPause Called.");
        FindObjectOfType<AudioManager>().Play("ButtonPress");
        if(gamePaused){
            Debug.Log("Resume!");
            gamePaused = false;
            Time.timeScale = 1f;
            gameMenu.SetActive(true);
            pauseMenu.SetActive(false);
            settings.gameObject.SetActive(false);
            if(timeValue > 0){
                launchTxt.SetActive(true);
            }
        }else{
            Debug.Log("Pause.");
            gamePaused = true;
            Time.timeScale = 0f;
            gameMenu.SetActive(false);
            pauseMenu.SetActive(true);
            settings.gameObject.SetActive(true);
            launchTxt.SetActive(false);
        }
    }

    public void resetTargets(GameObject curr){
        curr.SetActive(true);
        for(int i = 0; i < curr.transform.childCount; i++){
        float randX = Random.Range(-10.0f, 10.0f);
        float randY = Random.Range(-5.30f, 6.50F);
        //Debug.Log("x: " + randX + " | y: " + randY);
            curr.transform.GetChild(i).position= new Vector3(randX, randY, 0);
            curr.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

public void resetPointTargets(GameObject curr){
        curr.SetActive(true);
        for(int i = 0; i < curr.transform.childCount; i++){
        float randX = Random.Range(-10.0f, 10.0f);
        float randY = Random.Range(-5.30f, 6.50F);
        //Debug.Log("x: " + randX + " | y: " + randY);
            curr.transform.GetChild(i).position= new Vector3(randX, randY, 0);
            curr.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void resetPowerUp(GameObject curr) {
        curr.SetActive(false);
         int rand = Random.Range(0, (powerupManager.transform.childCount));
        curr = powerupManager.transform.GetChild(rand).gameObject;
        float randX = Random.Range(-10.0f, 10.0f);
        float randY = Random.Range(-5.30f, 6.50F);
        curr.transform.position= new Vector3(randX, randY, 0);
        curr.gameObject.SetActive(true);
    }

    private IEnumerator Animate(GameObject winner, GameObject loser){
        //Debug.Log("I happened.");
        am.Quiet("ArenaTheme", true);
        ParticleSystem death;
        winner.GetComponent<Player>().enabled = false;
        loser.GetComponent<Player>().enabled = false;
        winner.transform.position = new Vector3(winner.transform.position.x, 0f, winner.transform.position.z);
        
        winner.transform.GetChild(0).gameObject.SetActive(true);
        am.Play("Winner");
        death = loser.transform.GetChild(1).GetComponent<ParticleSystem>();
        loser.GetComponent<SpriteRenderer>().enabled = false;
        death.Play();
        yield return new WaitForSeconds(death.main.startLifetime.constantMax);
        death.Stop();
        yield return new WaitForSeconds(3f);
        winner.transform.GetChild(0).gameObject.SetActive(false);
        am.Quiet("ArenaTheme", false);
        winner.SetActive(false);
        loser.SetActive(false);
        loser.GetComponent<SpriteRenderer>().enabled = true;
        resetMenu.SetActive(true);
    }

    public void EndMatch(){
        timeValue = -1;
        ball.SetActive(false);
        targetManager.SetActive(false);
        pointsTargetManager.SetActive(false);
        powerupManager.SetActive(false);
        if(lefty.GetComponent<HP>().hp > righty.GetComponent<HP>().hp){
            //lefty.transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(Animate(lefty, righty));
        }else if(lefty.GetComponent<HP>().hp < righty.GetComponent<HP>().hp){
            //righty.transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(Animate(righty, lefty));
        }
    }

    public void Rematch(){
        SceneManager.LoadScene("Arena");
    }

    public void ToMainMenu(){
        if(gamePaused){
            am.Play("ButtonPress");
            OnPause();
        }
        
        //int currTime = am.GetSoundTime("ArenaTheme");
        am.Stop("ArenaTheme");
        am.Play("MenuTheme");
        settings.gameObject.SetActive(true);
        SceneManager.LoadScene("Main Menu");
    }
}
