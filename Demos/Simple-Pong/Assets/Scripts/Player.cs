using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb; //get this obj's rigidbody
    public float speed;
    private Vector2 movementInput;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
    }

    //check for regognized input
    public void OnMove( InputAction.CallbackContext context){
        movementInput = context.ReadValue<Vector2>() * Time.deltaTime * speed;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(0, movementInput.y * speed);
    }
}
