using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  void OnCollisionEnter2D(Collision2D other) {
             if (other.gameObject.tag == "target1" || other.gameObject.tag == "target2" || other.gameObject.tag == "target3" || other.gameObject.tag == "target4" || other.gameObject.tag == "target5") {
            Debug.Log ("pushRight");
            Destroy(other.gameObject);
            }
            
    }
}
