using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    public bool isLeft;

    [SerializeField]
    public GameObject fieldManager;

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Ball"))
        {
            if(!isLeft)
            {
                Debug.Log("Lefty Scores!");
                fieldManager.GetComponent<GameManager>().leftScored();                  
            }else{
                Debug.Log("Righty Scores!");
                fieldManager.GetComponent<GameManager>().rightScored();                  
            }

        }
    }

}
