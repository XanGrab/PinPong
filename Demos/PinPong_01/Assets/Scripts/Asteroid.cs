using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public int health;
    private ParticleSystem particles;
    private MeshRenderer mr;
    private CircleCollider2D circle2D;

    private void Awake(){
        particles = GetComponentInChildren<ParticleSystem>();
        circle2D = GetComponent<CircleCollider2D>();
        mr = GetComponent<MeshRenderer>();
    }
    private void OnCollisionEnter2D(Collision2D other) {
        health--;
        if(other.gameObject.CompareTag("Ball") || other.gameObject.CompareTag("PowerUp")){
            if(health <= 0){
                StartCoroutine(Break());
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
    }
}
