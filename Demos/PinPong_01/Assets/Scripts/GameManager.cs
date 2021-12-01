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

    private static bool playing;
    private static bool gamePaused;
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
        Debug.Log(righty.GetComponent<Health>().health);
        if(righty.GetComponent<Health>().health <= 0){
            //Debug.Log("Health: " + righty.GetComponent<Health>().health);
            EndMatch();
        }
        //lScore += ball.GetComponent<Ball>().score;
        righty.GetComponent<Health>().health--;
        righty.GetComponent<Health>().UpdateHealth();
        lScoreTxt.GetComponent<TextMeshProUGUI>().text = lScore.ToString();
        ball.GetComponent<Ball>().Reset();
        resetTargets(currentLayout);
        resetPointTargets(pointTargetsCurrentLayout);
    }

    public void rightScored(){
        Debug.Log(lefty.GetComponent<Health>().health);
        if(lefty.GetComponent<Health>().health <= 0){
            //Debug.Log("Health: " + righty.GetComponent<Health>().health);
            EndMatch();
        }
        //rScore += ball.GetComponent<Ball>().score;
        lefty.GetComponent<Health>().health--;
        lefty.GetComponent<Health>().UpdateHealth();
        rScoreTxt.GetComponent<TextMeshProUGUI>().text = rScore.ToString();
        ball.GetComponent<Ball>().Reset();
        resetTargets(currentLayout);
        resetPointTargets(pointTargetsCurrentLayout);
    }

     public void TargetLeftScored(){
        lScore += 50;
        lScoreTxt.GetComponent<TextMeshProUGUI>().text = lScore.ToString();
    }

    public void TargetRightScored(){
        rScore += 50;
        rScoreTxt.GetComponent<TextMeshProUGUI>().text = rScore.ToString();
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

    public IEnumerator EndMatch(){
        ball.SetActive(false);
        targetManager.SetActive(false);
        pointsTargetManager.SetActive(false);

        ParticleSystem death;
        if(lefty.GetComponent<Health>().health > righty.GetComponent<Health>().health){
            lefty.GetComponent<Player>().enabled = false;
            righty.GetComponent<Player>().enabled = false;
            
            lefty.transform.GetChild(0).gameObject.SetActive(true);
            death = righty.transform.GetChild(1).GetComponent<ParticleSystem>();
            righty.GetComponent<SpriteRenderer>().enabled = false;
            death.Play();
            yield return new WaitForSeconds(death.main.startLifetime.constantMax);
            death.Stop();
            yield return new WaitForSeconds(3f);
            lefty.transform.GetChild(0).gameObject.SetActive(false);
            lefty.SetActive(false);
            righty.SetActive(false);
            righty.GetComponent<SpriteRenderer>().enabled = true;
        }else if(lefty.GetComponent<Health>().health < righty.GetComponent<Health>().health){
            lefty.GetComponent<Player>().enabled = false;
            righty.GetComponent<Player>().enabled = false;

            righty.transform.GetChild(0).gameObject.SetActive(true);
            death = lefty.transform.GetChild(1).GetComponent<ParticleSystem>();
            lefty.GetComponent<SpriteRenderer>().enabled = false;
            death.Play();
            yield return new WaitForSeconds(death.main.startLifetime.constantMax);
            death.Stop();
            yield return new WaitForSeconds(3f);
            righty.transform.GetChild(0).gameObject.SetActive(false);
            lefty.SetActive(false);
            righty.SetActive(false);
            righty.GetComponent<SpriteRenderer>().enabled = true;
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
