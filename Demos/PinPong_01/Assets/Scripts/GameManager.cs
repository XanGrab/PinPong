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
private void Awake() {
_instance = this;
}

    [Header("Ball")]
    public GameObject ball;

    [Header("Player Input")]
    private PlayerInput playerInput;

    [Header("Left Player")]
    public GameObject lefty;
    public GameObject leftGoal;


    [Header("Right Player")]
    public GameObject righty;
    public GameObject rightGoal;

    [Header("Score UI")]
    public GameObject lScoreTxt;
    public GameObject rScoreTxt;
    public GameObject resetMenu;
    private int lScore;
    private int rScore;

    [Header("Timer")]
    public Text timerText;
    public float timeValue = 90;

    [Header("Targets")]
    public GameObject targetManager;
    public GameObject currentLayout;

    private bool playing;

    void Start(){
        playing = true;
        resetMenu.SetActive(false);

        lefty.GetComponent<PlayerInput>().SwitchCurrentControlScheme(controlScheme: "Left Keyboard", Keyboard.current);
        righty.GetComponent<PlayerInput>().SwitchCurrentControlScheme(controlScheme: "Right Keyboard", Keyboard.current);
        currentLayout = targetManager.transform.GetChild(Random.Range(0, targetManager.transform.childCount - 1)).gameObject;
    }

    void Update(){
        if(playing){
            timeValue -= Time.deltaTime;
            DisplayTime(timeValue);

            if(timeValue < 1){
                if(lScore != rScore){
                    playing = false;
                    StartCoroutine(EndMatch());
                }else{
                    timeValue += 30;
                }
            }
        }
        // Debug.Log(ball.GetComponent<Ball>().velo);
        //  Debug.Log(ball.GetComponent<Ball>().speed);
    }

    public void leftScored(){
        lScore += ball.GetComponent<Ball>().score;
        lScoreTxt.GetComponent<TextMeshProUGUI>().text = lScore.ToString();
        ball.GetComponent<Ball>().Reset();
        resetTargets(currentLayout);
    }

    public void rightScored(){
        rScore += ball.GetComponent<Ball>().score;
        rScoreTxt.GetComponent<TextMeshProUGUI>().text = rScore.ToString();
        ball.GetComponent<Ball>().Reset();
        resetTargets(currentLayout);
    }

     public void TargetLeftScored(){
        lScore += 27;
        lScoreTxt.GetComponent<TextMeshProUGUI>().text = lScore.ToString();
    }

    public void TargetRightScored(){
        rScore += 27;
        rScoreTxt.GetComponent<TextMeshProUGUI>().text = rScore.ToString();
    }

    void DisplayTime(float timeToDisplay){
        if(timeToDisplay < 0){
            timeToDisplay = 0;
        }

        int min = Mathf.FloorToInt(timeToDisplay / 60);
        int sec = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", min, sec);
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

    public IEnumerator EndMatch(){
        ball.SetActive(false);
        targetManager.SetActive(false);

        ParticleSystem death;
        if(lScore > rScore){
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
        }else if(lScore < rScore){
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
        SceneManager.LoadScene("Main Menu");
    }
}
