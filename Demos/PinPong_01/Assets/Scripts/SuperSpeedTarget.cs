using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSpeedTarget : MonoBehaviour
{
    private Transform parent;
    private ParticleSystem particles;
    private MeshRenderer mr;
    private BoxCollider2D box2D;
 
    private void Awake(){
        parent = gameObject.transform.parent;
        particles = parent.GetComponentInChildren<ParticleSystem>();
        box2D = GetComponent<BoxCollider2D>();
        mr = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
        //StartCoroutine( ShowAndHide(gameObject, 5.0f) ); // 5 second
    }

    private void OnTriggerEnter2D(Collider2D obj){
        StartCoroutine(Break());
        if(obj.gameObject.CompareTag("Ball")){
            //touchedLast = -1 means that paddle left touched the ball last, and therefore hit the freeze target
            if (obj.gameObject.GetComponent<Ball>().touchedLast == -1) {
                    obj.gameObject.GetComponent<Ball>().superSpeed = -1;
                    GameObject paddleLeft = GameObject.Find("Paddle Left");
                    //Kind of janky to change color
                    paddleLeft.GetComponent<SpriteRenderer>().color = Color.red;
               // GameManager.Instance.TargetLeftScored();
            } else if (obj.gameObject.GetComponent<Ball>().touchedLast == 1){
                    obj.gameObject.GetComponent<Ball>().superSpeed = 1;
                    GameObject paddleRight = GameObject.Find("Paddle Right");
                    paddleRight.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    private IEnumerator Break(){
        particles.Play();
        mr.enabled = false;
        parent.GetComponent<SpriteRenderer>().enabled = false;
        box2D.enabled = false;
        yield return new WaitForSeconds(particles.main.startLifetime.constantMax);
        parent.gameObject.SetActive(false);
        parent.GetComponent<SpriteRenderer>().enabled = true;
        box2D.enabled = true;
        mr.enabled = true;
    }

    private IEnumerator ShowAndHide(GameObject go, float delay){
        go.SetActive(true);
        Debug.Log("set to false");
        yield return new WaitForSeconds(5);
        Debug.Log("set to true");
        go.SetActive(false);
    }
}
