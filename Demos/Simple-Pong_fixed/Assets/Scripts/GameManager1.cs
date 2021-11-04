using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager1 : MonoBehaviour
{
    [Header("Ball")]
    public GameObject ball;

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
        var p1 = PlayerInput.all[0];
        var p2 = PlayerInput.all[1];

        p1.user.UnpairDevices();
        p2.user.UnpairDevices();
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
