using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTarget : MonoBehaviour
{
    private ParticleSystem particles;
    private MeshRenderer mr;
    private BoxCollider2D box2D;

    private void Awake(){
        particles = GetComponentInChildren<ParticleSystem>();
        box2D = GetComponent<BoxCollider2D>();
        mr = GetComponent<MeshRenderer>();
    }
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);  
    }

    private void OnTriggerEnter2D(Collider2D obj){
        if(obj.gameObject.CompareTag("Ball")){
            StartCoroutine(Break());
            if (obj.gameObject.GetComponent<Ball>().touchedLast == -1) {
                //GameManager.Instance.TargetLeftScored();
                FindObjectOfType<GameManager>().LeftHPPickUp();
            } else if (obj.gameObject.GetComponent<Ball>().touchedLast == 1){
                //GameManager.Instance.TargetRightScored();
                FindObjectOfType<GameManager>().RightHPPickUp();
            }

        }
    }

    private IEnumerator Break(){
        particles.Play();
        mr.enabled = false;
        box2D.enabled = false;
        yield return new WaitForSeconds(particles.main.startLifetime.constantMax);
        gameObject.SetActive(false);
        box2D.enabled = true;
        mr.enabled = true;
    }
}
