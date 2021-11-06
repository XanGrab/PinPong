using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    private int lScore;
    private int rScore;

    void Start(){
        //var lefty = PlayerInput.all[0];
        //var righty = PlayerInput.all[1];
        
        //InputUser.PerformPairingWithDevice(Keyboard.current, lefty);

        //lefty.user.ActivateControlScheme("Left Keyboard");
        //righty.user.ActivateControlScheme("Right Keyboard");
        var left = PlayerInput.Instantiate(lefty, controlScheme: "Left Keyboard", pairWithDevice: Keyboard.current);
        //var right = PlayerInput.Instantiate(righty, controlScheme: "Right Keyboard", pairWithDevice: Keyboard.current);
        
        lefty.SetActive(false);
        righty.SetActive(false);
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
}
