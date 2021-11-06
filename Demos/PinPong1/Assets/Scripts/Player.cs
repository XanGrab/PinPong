using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;
    public float flipTorque;
    private Vector2 movVector;
    private bool flipped;
    private bool flipping;
    private Rigidbody2D rb;
    private HingeJoint2D hj;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
        hj = gameObject.GetComponent<HingeJoint2D>();
        hj.enabled = false;

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    //check for regognized input
    public void OnMove( InputAction.CallbackContext context ){
        movVector = context.ReadValue<Vector2>() * speed;
        Debug.Log(gameObject.name + ".OnMove() Action requested");
    }

    public void OnFlip( InputAction.CallbackContext context ){
        flipping = context.action.triggered;
        hj.enabled = true;

        rb.AddTorque(flipTorque * 10f);
        
        flipped = true;
        rb.constraints = RigidbodyConstraints2D.None;
        Debug.Log(gameObject.name + ".OnFlip() Action requested");
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector2(0, movVector.y) * speed;
        //Debug.Log(flipped);
        if(flipped == true){
            if(!flipping){
                rb.AddTorque(-flipTorque);
            }
            
            /*if(gameObject.transform.rotation.y == 0){
                flipped = false;
                hj.enabled = false;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            }*/
        }
        /*if(flipped == true){
            rb.AddTorque(-(flipTorque / 1.25f));

            if(gameObject.transform.rotation.z == 0){
                flipped = false;
            }
        }*/
        //rb.AddForce( new Vector2(0, movementInput.y) * speed );
    }
}
