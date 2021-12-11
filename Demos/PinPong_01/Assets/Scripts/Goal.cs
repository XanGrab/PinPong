using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    public bool isLeft;

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("Ball"))
        {
            FindObjectOfType<GameManager>().DisplayDamage(collision.gameObject.transform.position);
            if(!isLeft)
            {
                FindObjectOfType<GameManager>().Score(1);                  
            }else{
                FindObjectOfType<GameManager>().Score(-1);                  
            }

        }
    }

}
