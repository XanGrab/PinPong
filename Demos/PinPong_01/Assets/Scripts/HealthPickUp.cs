using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    private ParticleSystem particles;
    private MeshRenderer mr;
    private CircleCollider2D circle2D;

    private void Awake(){
        particles = GetComponentInChildren<ParticleSystem>();
        circle2D = GetComponent<CircleCollider2D>();
        mr = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        transform.Rotate(new Vector3(0, 30, 0) * Time.deltaTime);  
    }

    private void OnTriggerEnter2D(Collider2D obj){
        if(obj.gameObject.CompareTag("Ball")){
            StartCoroutine(Break());
            if (obj.gameObject.GetComponent<Ball>().touchedLast == -1) {
                FindObjectOfType<GameManager>().HPPickUp( -1 );
            } else if (obj.gameObject.GetComponent<Ball>().touchedLast == 1){
                FindObjectOfType<GameManager>().HPPickUp( 1 );
            }

        }
    }

    private IEnumerator Break(){
        particles.Play();
        mr.enabled = false;
        circle2D.enabled = false;
        yield return new WaitForSeconds(particles.main.startLifetime.constantMax);
        gameObject.SetActive(false);
        circle2D.enabled = true;
        mr.enabled = true;
        FindObjectOfType<GameManager>().pointTargetsBroken++;
    }
}
