using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody2D p1Rb; //get this obj's rigidbody
    public float speed;
    private Vector2 p1MovementVector;

    void Start()
    {
        p1Rb = gameObject.GetComponent<Rigidbody2D>(); 
    }

    //check for regognized input
    public void OnMove( InputAction.CallbackContext context ){
        p1MovementVector = context.ReadValue<Vector2>() * speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        p1Rb.velocity = new Vector2(0, p1MovementVector.y) * speed;
        //rb.AddForce( new Vector2(0, movementInput.y) * speed );
    }
}
