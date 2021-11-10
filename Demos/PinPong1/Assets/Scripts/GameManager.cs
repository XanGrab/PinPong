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
    public PlayerInput leftInput;
    public GameObject leftGoal;


    [Header("Right Player")]
    public GameObject righty;
    public PlayerInput rightInput;
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

    void Start(){
        resetMenu.SetActive(false);

        lefty.GetComponent<PlayerInput>().SwitchCurrentControlScheme(controlScheme: "Left Keyboard", Keyboard.current);
        righty.GetComponent<PlayerInput>().SwitchCurrentControlScheme(controlScheme: "Right Keyboard", Keyboard.current);
    }

    void Update(){
        timeValue -= Time.deltaTime;
        DisplayTime(timeValue);

        if(timeValue < 1){
            resetMenu.SetActive(true);
            ball.SetActive(false);
            lefty.SetActive(false);
            righty.SetActive(false);
        }
    }

    public void leftScored(){
        lScore++;
        lScoreTxt.GetComponent<TextMeshProUGUI>().text = lScore.ToString();
        ball.GetComponent<Ball>().Reset();
    }

    public void rightScored(){
        rScore++;
        rScoreTxt.GetComponent<TextMeshProUGUI>().text = rScore.ToString();
        ball.GetComponent<Ball>().Reset();
    }

    void DisplayTime(float timeToDisplay){
        if(timeToDisplay < 0){
            timeToDisplay = 0;
        }

        int min = Mathf.FloorToInt(timeToDisplay / 60);
        int sec = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", min, sec);
    }

    public void Rematch(){
        //Debug.Log("Rematch!");
        SceneManager.LoadScene("Arena");
        /*timeValue = 90;
        resetMenu.SetActive(false);
        ball.SetActive(true);
        ball.GetComponent<Ball>().Reset();
        lefty.transform.position = new Vector3(lefty.transform.position.x, 0, 0);
        lefty.SetActive(true);
        righty.transform.position = new Vector3(righty.transform.position.x, 0, 0);
        righty.SetActive(true);
        lScore = 0;
        rScore = 0;*/
    }

    public void ToMainMenu(){
        SceneManager.LoadScene("Main Menu");
    }
}
