using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
 public Rigidbody2D rb; //get this obj's rigidbody;
public float SpeedX = 2000f;
public float SpeedY =3005f;
    // Start is called before the first frame update
    void Start()
    {
         rb = gameObject.GetComponent<Rigidbody2D>();
rb.AddForce(new Vector2(SpeedX, SpeedY)); 
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(new Vector2(SpeedX, SpeedY));
    }
}
