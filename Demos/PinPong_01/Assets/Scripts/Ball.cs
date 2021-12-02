using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int touchedLast = 0;
    public int superSpeed = 0;

    public float speed;
    public float maxSqrVelocity;
    public int score = 100;
    public Rigidbody2D rb;
    public Vector2 startPos;
    private TrailRenderer trail;
    private Renderer _render;
    Color initColor;
    private float lerpValue;
    GameObject[] playerWalls;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();   
        trail = GetComponent<TrailRenderer>();
        //startGradient = trail.colorGradient;
        _render = GetComponent<Renderer>();
        playerWalls = GameObject.FindGameObjectsWithTag("PlayerWall");
        foreach(GameObject wall in playerWalls){
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), wall.gameObject.GetComponent<Collider2D>());
        }
        initColor = Color.white;
        lerpValue = 0f;
        Launch();
    }

    void Update(){
        if(rb.velocity.sqrMagnitude > maxSqrVelocity){
            Debug.Log("slowling velocity: " + rb.velocity.sqrMagnitude);
            rb.velocity *= 0.9f;
        }
    }

    private void Launch(){
        score = 10;
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;
        rb.velocity = new Vector2(x, y) * speed;
    }

    public void Reset(){
        transform.position = startPos;
        FindObjectOfType<AudioManager>().Play("Break");
        Launch();
        //trail.Clear();
        lerpValue = 0f;
        trail.material.color = initColor;
        _render.material.color = trail.material.color;
        touchedLast = 0;
    }

    private void OnTriggerEnter2D(Collider2D obj){
        if(obj.gameObject.CompareTag("Target")){
            if(obj.gameObject.name.Equals("Target")){
                score += 10;
                lerpValue += 0.22f;  

                Color objColor = obj.GetComponent<Renderer>().material.color;

                _render.material.color = Color.Lerp(initColor, objColor,  lerpValue);
                trail.material.color = _render.material.color;
            }
            FindObjectOfType<AudioManager>().Play("TargetBreak");
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
            if (other.gameObject.name.Equals("Paddle Left")) {
                //Debug.Log("Left");
                touchedLast = -1;
                if (superSpeed == -1) {
                    rb.velocity = rb.velocity * 3;
                    superSpeed = 0;
                    GameObject paddleLeft = GameObject.Find("Paddle Left");
                    //Kind of janky to change color
                    paddleLeft.GetComponent<SpriteRenderer>().color = Color.white;
                }
            } else if (other.gameObject.name.Equals("Paddle Right")) {
                //Debug.Log("Right");
                touchedLast = 1;
                if (superSpeed == 1) {
                    rb.velocity = rb.velocity * 3;
                    superSpeed = 0;
                    GameObject paddleRight = GameObject.Find("Paddle Right");
                    //Kind of janky to change color
                    paddleRight.GetComponent<SpriteRenderer>().color = Color.white;
                }
            }
                     
    }
}
