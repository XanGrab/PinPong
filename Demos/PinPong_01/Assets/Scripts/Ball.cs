using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int touchedLast = 0;
    public bool speedHit = false;
    public float speed;
    private float maxSqrVelocity = 600;
    private float minSqrVelocity = 10;

    public int score = 100;
    public Rigidbody2D rb;
    public Vector2 startPos;
    private TrailRenderer trail;
    private float launchTime;
    private Renderer _render;
    Color initColor;
    private float lerpValue;
    GameObject[] playerWalls;
    
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();   
        trail = GetComponent<TrailRenderer>();
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
        // add velocity clamping to ball
        if(rb.velocity.sqrMagnitude > maxSqrVelocity && speedHit == false){
            rb.velocity *= 0.9f;
        }
        if(rb.velocity.sqrMagnitude < minSqrVelocity){
            rb.velocity *= 1.1f;
        }
    }

    public void Launch(){
        score = 100;
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;
        rb.velocity = new Vector2(x, y) * speed;
    }

    public void Reset(){
        transform.position = startPos;
        FindObjectOfType<AudioManager>().Play("Break");
        Launch();
        
        lerpValue = 0f;
        trail.material.color = initColor;
        _render.material.color = trail.material.color;
        touchedLast = 0;
    }

    private void OnTriggerEnter2D(Collider2D obj){
        if(obj.gameObject.CompareTag("Target")){
            if(obj.gameObject.name.Equals("Target")){
                score += 100;
                FindObjectOfType<GameManager>().DisplayPoints(transform.position);

                // Change Color
                lerpValue += 0.22f;  
                Color objColor = obj.GetComponent<Renderer>().material.color;
                _render.material.color = Color.Lerp(initColor, objColor,  lerpValue);
                trail.material.color = _render.material.color;
            }
            FindObjectOfType<AudioManager>().Play("TargetBreak");
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            GameObject paddle = other.gameObject;
            if(paddle.name.Equals("Paddle Left")){
                touchedLast = -1;
            }else{
                touchedLast = 1;
            }    

            if (paddle.GetComponent<Player>().hasSpeedPower) {
                FindObjectOfType<AudioManager>().Play("Speedy");
                rb.velocity = rb.velocity * 3;
                paddle.GetComponent<Player>().hasSpeedPower = false;
                paddle.transform.GetChild(2).gameObject.SetActive(false);
                paddle.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }
}
