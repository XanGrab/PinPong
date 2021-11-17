using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool leftTouchedLast;
    public float speed;
    public int score = 100;
    public Vector2 velo;
    public Rigidbody2D rb;
    public Vector2 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();   

        Launch();
    }

    private void Launch(){
        score = 100;
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;
        rb.velocity = new Vector2(x, y) * speed;
        velo = rb.velocity;	
    }

    public void Reset(){
        //Debug.Log("Reset Ball");
        transform.position = startPos;
        Launch();
    }

    private void OnTriggerEnter2D(Collider2D obj){
        if(obj.gameObject.CompareTag("Target")){
            score += 100;
        }
    }

       void OnCollisionEnter2D(Collision2D other) {

            if (other.gameObject.name.Equals("Paddle Left")) {
                Debug.Log("Left");
                leftTouchedLast = true;
            } else if (other.gameObject.name.Equals("Paddle Right")) {
                Debug.Log("Right");
                leftTouchedLast = false;
            }
                     
    }
}
