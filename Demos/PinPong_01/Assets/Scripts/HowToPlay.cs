using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlay : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void OnSceneLoad(){
        gameObject.SetActive(false);
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.Play("How To Play", 0, 0f); 
    }
}
