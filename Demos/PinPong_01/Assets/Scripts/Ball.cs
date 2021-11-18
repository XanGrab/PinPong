using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed;
    public float maxSqrVelocity;
    public int score = 100;
    public Rigidbody2D rb;
    public Vector2 startPos;
    private TrailRenderer trail;
    private Renderer _render;
    Color initColor;
    Color finColor;
    Color lerpColor;
    private float t = 0;
    //private Gradient startGradient;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();   
        trail = GetComponent<TrailRenderer>();
        //startGradient = trail.colorGradient;
        _render = GetComponent<Renderer>();
        initColor = Color.white;
        finColor = Color.red;
        Launch();
    }

    void Update(){
        //Debug.Log("Ball velocity: " + rb.velocity.sqrMagnitude);
        //Debug.Log(trail.colorGradient.colorKeys[0].color);

 
        if(t < 1f){
            Debug.Log("Changing Color");
            lerpColor = Color.Lerp(initColor, finColor,  t);
            _render.material.color = lerpColor;
            t += Time.deltaTime/6f;
        }
        //trail.colorGradient.colorKeys[0].color;
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
        //trail.colorGradient = startGradient; 
        trail.enabled = false;
        transform.position = startPos;
        Launch();
        trail.enabled = true;
        _render.material.color = initColor;
        t = 0;
    }

    private void OnTriggerEnter2D(Collider2D obj){
        if(obj.gameObject.CompareTag("Target")){
            score += 100;

            //trail.colorGradient.colorKeys[0].color = newColor;
        }
    }
}
