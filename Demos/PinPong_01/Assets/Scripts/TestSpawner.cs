using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager1 : MonoBehaviour
{

    [Header("Left Player")]
    public GameObject p1;


    [Header("Right Player")]
    public GameObject p2;


    void Start(){
        
        var left = PlayerInput.Instantiate(p1, controlScheme: "Left Keyboard", pairWithDevice: Keyboard.current);
        var right = PlayerInput.Instantiate(p2, controlScheme: "Right Keyboard", pairWithDevice: Keyboard.current);
        
        p1.SetActive(false);
        p2.SetActive(false);
    }
}
