using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    public bool isLeft;

/*    [SerializeField]
    public GameObject fieldManager;*/

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Ball"))
        {
            FindObjectOfType<GameManager>().DisplayDamage(collision.gameObject.transform.position);
            if(!isLeft)
            {
                Debug.Log("Lefty Scores!");
                FindObjectOfType<GameManager>().leftScored();                  
            }else{
                Debug.Log("Righty Scores!");
                FindObjectOfType<GameManager>().rightScored();                  
            }

        }
    }

}
