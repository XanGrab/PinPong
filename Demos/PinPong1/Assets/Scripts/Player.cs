using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb; //get this obj's rigidbody
    public float speed;
    private Vector2 movVector;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
    }

    //check for regognized input
    public void OnMove( InputAction.CallbackContext context ){
        movVector = context.ReadValue<Vector2>() * speed;
        Debug.Log(gameObject.name + ".OnMove() Action requested");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(0, movVector.y) * speed;
        //rb.AddForce( new Vector2(0, movementInput.y) * speed );
    }
}
