using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
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

    void Start(){
        resetMenu.SetActive(false);

        lefty.GetComponent<PlayerInput>().SwitchCurrentControlScheme(controlScheme: "Left Keyboard", Keyboard.current);
        righty.GetComponent<PlayerInput>().SwitchCurrentControlScheme(controlScheme: "Right Keyboard", Keyboard.current);
        currentLayout = targetManager.transform.GetChild(Random.Range(0, targetManager.transform.childCount - 1)).gameObject;
    }

    void Update(){
        timeValue -= Time.deltaTime;
        DisplayTime(timeValue);

        if(timeValue < 1){
            resetMenu.SetActive(true);
            targetManager.SetActive(false);
            ball.SetActive(false);
            lefty.SetActive(false);
            righty.SetActive(false);
        }
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
    }

    public void Rematch(){
        SceneManager.LoadScene("Arena");
    }

    public void ToMainMenu(){
        SceneManager.LoadScene("Main Menu");
    }
}
