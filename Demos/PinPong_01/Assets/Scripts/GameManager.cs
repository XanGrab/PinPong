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
    public GameObject ball;

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
    private int lScore;
    private int rScore;

    /*[Header("Timer")]
    public Text timerText;
    public float timeValue = 90;*/

    [Header("Targets")]
    public GameObject targetManager;
    public GameObject pointsTargetManager;
    public GameObject currentLayout;
    public GameObject pointTargetsCurrentLayout;
    public int targetHealthPickUp;

    private static bool playing;
    public static bool gamePaused;
    AudioManager am;

    void Awake(){
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
        currentLayout = targetManager.transform.GetChild(Random.Range(0, targetManager.transform.childCount - 1)).gameObject;
        pointsTargetManager = GameObject.Find("Points Target Manager");
        pointTargetsCurrentLayout = pointsTargetManager.transform.GetChild(Random.Range(0, pointsTargetManager.transform.childCount - 1)).gameObject;
    }

    void Update(){
        if(playing && (!gamePaused)){

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
        //Debug.Log("Right HP" + righty.GetComponent<HP>().hp);
        //lScore += ball.GetComponent<Ball>().score;
        righty.GetComponent<HP>().hp -= ball.GetComponent<Ball>().score;
        righty.GetComponent<HP>().UpdateHealth();
        lScoreTxt.GetComponent<TextMeshProUGUI>().text = lScore.ToString();
        ball.GetComponent<Ball>().Reset();
        resetTargets(currentLayout);
        resetPointTargets(pointTargetsCurrentLayout);
        if(righty.GetComponent<HP>().hp <= 0){
            //Debug.Log("Health: " + righty.GetComponent<Health>().health);
            EndMatch();
        }
    }

    public void rightScored(){
        //Debug.Log("Left HP: " + lefty.GetComponent<HP>().hp);
        rScore += ball.GetComponent<Ball>().score;
        //lefty.GetComponent<HP>().hp--;
        lefty.GetComponent<HP>().hp -= ball.GetComponent<Ball>().score;
        lefty.GetComponent<HP>().UpdateHealth();
        rScoreTxt.GetComponent<TextMeshProUGUI>().text = rScore.ToString();
        ball.GetComponent<Ball>().Reset();
        resetTargets(currentLayout);
        resetPointTargets(pointTargetsCurrentLayout);
        if(lefty.GetComponent<HP>().hp <= 0){
            //Debug.Log("Health: " + righty.GetComponent<Health>().health);
            EndMatch();
        }
    }

     public void TargetLeftScored(){
        lefty.GetComponent<HP>().hp += targetHealthPickUp;
        lefty.GetComponent<HP>().UpdateHealth();
        //lScoreTxt.GetComponent<TextMeshProUGUI>().text = lScore.ToString();
    }

    public void TargetRightScored(){
        righty.GetComponent<HP>().hp += targetHealthPickUp;
        lefty.GetComponent<HP>().UpdateHealth();
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
        if(gamePaused){
            Debug.Log("Resume!");
            gamePaused = false;
            Time.timeScale = 1f;
            gameMenu.SetActive(true);
            pauseMenu.SetActive(false);
        }else{
            Debug.Log("Pause.");
            gamePaused = true;
            Time.timeScale = 0f;
            gameMenu.SetActive(false);
            pauseMenu.SetActive(true);
        }
    }

    public void resetTargets(GameObject curr){
        curr.SetActive(false);
        int rand = Random.Range(0, targetManager.transform.childCount);
        Debug.Log(rand);
        curr = targetManager.transform.GetChild(rand).gameObject;
        curr.SetActive(true);
        for(int i = 0; i < curr.transform.childCount; i++){
            curr.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

public void resetPointTargets(GameObject curr){
        curr.SetActive(false);
        int rand = Random.Range(0, pointsTargetManager.transform.childCount);
        Debug.Log(rand);
        curr = pointsTargetManager.transform.GetChild(rand).gameObject;
        curr.SetActive(true);
        for(int i = 0; i < curr.transform.childCount; i++){
            curr.transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private IEnumerator Animate(GameObject winner, GameObject loser){
        //Debug.Log("I happened.");
        ParticleSystem death;
        winner.GetComponent<Player>().enabled = false;
        loser.GetComponent<Player>().enabled = false;
        
        winner.transform.GetChild(0).gameObject.SetActive(true);
        death = loser.transform.GetChild(1).GetComponent<ParticleSystem>();
        loser.GetComponent<SpriteRenderer>().enabled = false;
        death.Play();
        yield return new WaitForSeconds(death.main.startLifetime.constantMax);
        death.Stop();
        yield return new WaitForSeconds(3f);
        winner.transform.GetChild(0).gameObject.SetActive(false);
        winner.SetActive(false);
        loser.SetActive(false);
        loser.GetComponent<SpriteRenderer>().enabled = true;
    }

    public void EndMatch(){
        ball.SetActive(false);
        targetManager.SetActive(false);
        pointsTargetManager.SetActive(false);

        if(lefty.GetComponent<HP>().hp > righty.GetComponent<HP>().hp){
            StartCoroutine(Animate(lefty.gameObject, righty.gameObject));
        }else if(lefty.GetComponent<HP>().hp < righty.GetComponent<HP>().hp){
            StartCoroutine(Animate(righty, lefty));
        }

        lefty.SetActive(false);
        righty.SetActive(false);
        resetMenu.SetActive(true);
    }

    public void Rematch(){
        SceneManager.LoadScene("Arena");
    }

    public void ToMainMenu(){
        if(gamePaused){
            am.Play("ButtonPress");
            OnPause();
        }
        
        int currTime = am.GetSoundTime("ArenaTheme");
        am.Stop("ArenaTheme");
        am.Play("MenuTheme", currTime);
        SceneManager.LoadScene("Main Menu");
    }
}
