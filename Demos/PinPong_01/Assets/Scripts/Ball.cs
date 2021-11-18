using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool leftTouchedLast;
    public float speed;
    public float maxSqrVelocity;
    public int score = 100;
    public Rigidbody2D rb;
    public Vector2 startPos;
    private TrailRenderer trail;
    private Renderer _render;
    Color initColor;
    private float lerpValue;

    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();   
        trail = GetComponent<TrailRenderer>();
        //startGradient = trail.colorGradient;
        _render = GetComponent<Renderer>();
        initColor = Color.white;
        lerpValue = 0f;
        Launch();
    }

    void Update(){
        if(rb.velocity.sqrMagnitude > maxSqrVelocity){
            Debug.Log("slowling velocity: " + rb.velocity.sqrMagnitude);
            rb.velocity *= 0.991f;
        }
    }

    private void Launch(){
        score = 100;
        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;
        rb.velocity = new Vector2(x, y) * speed;
    }

    public void Reset(){
        transform.position = startPos;
        Launch();
        //trail.Clear();
        lerpValue = 0f;
        trail.material.color = initColor;
        _render.material.color = trail.material.color;
    }

    private void OnTriggerEnter2D(Collider2D obj){
        if(obj.gameObject.CompareTag("Target")){
            score += 100;
            lerpValue += 0.15f;  

            Color objColor = obj.GetComponent<Renderer>().material.color;

            _render.material.color = Color.Lerp(initColor, objColor,  lerpValue);
            trail.material.color = _render.material.color;
        }
    }

    /*void OnCollisionEnter2D(Collision2D other) {

            if (other.gameObject.name.Equals("Paddle Left")) {
                Debug.Log("Left");
                leftTouchedLast = true;
            } else if (other.gameObject.name.Equals("Paddle Right")) {
                Debug.Log("Right");
                leftTouchedLast = false;
            }
                     
    }*/
}
