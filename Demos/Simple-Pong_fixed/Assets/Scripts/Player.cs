<<<<<<< HEAD
=======
using System.Collections;
using System.Collections.Generic;
>>>>>>> refs/remotes/origin/main
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb; //get this obj's rigidbody
    public float speed;
<<<<<<< HEAD
    private Vector2 movVector;
=======
    private Vector2 movementInput;
>>>>>>> refs/remotes/origin/main

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
    }

    //check for regognized input
<<<<<<< HEAD
    public void OnMove( InputAction.CallbackContext context ){
        movVector = context.ReadValue<Vector2>() * speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(0, movVector.y) * speed;
        //rb.AddForce( new Vector2(0, movementInput.y) * speed );
=======
    public void OnMove( InputAction.CallbackContext context){
        movementInput = context.ReadValue<Vector2>() * Time.deltaTime * speed;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(0, movementInput.y * speed);
>>>>>>> refs/remotes/origin/main
    }
}
