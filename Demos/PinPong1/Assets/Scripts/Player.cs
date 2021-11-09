using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public enum state{
        Move,
        FlipUp,
        FlipDown,
        ResetUp,
        ResetDown
    }

    private float setX;
    private state playerState;
    public float speed;
    public float flipTorque;
    private Vector2 movVector;
    private bool flippingUp;
    private bool flippingDown;
    private Rigidbody2D rb;
    private HingeJoint2D hj;

    void Start()
    {
        setX = transform.position.x;
        rb = gameObject.GetComponent<Rigidbody2D>(); 
        hj = gameObject.GetComponent<HingeJoint2D>();
        hj.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation & RigidbodyConstraints2D.FreezePositionX;
        playerState = state.Move;
    }

    //check for regognized input
    public void OnMove( InputAction.CallbackContext context ){
        movVector = context.ReadValue<Vector2>() * speed;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation & RigidbodyConstraints2D.FreezePositionX;
    }

    public void OnFlipUp( InputAction.CallbackContext context ){
        if(!flippingDown){
            if(hj.anchor.y != 0.5){
                JointAngleLimits2D limits = hj.limits;
                hj.anchor = new Vector2(0, 0.5f);
                limits.max *= -1;
                hj.limits = limits;
            }
            hj.enabled = true;
            rb.constraints = RigidbodyConstraints2D.None;
            flippingUp = context.action.triggered;
            playerState = state.FlipUp;
        }
    }

    public void OnFlipDown( InputAction.CallbackContext context ){
        if(!flippingUp){
            if(hj.anchor.y != -0.5){
                JointAngleLimits2D limits = hj.limits;
                limits.max *= -1;
                hj.anchor = new Vector2(0, -0.5f);
                hj.limits = limits;
            }
            hj.enabled = true;
            rb.constraints = RigidbodyConstraints2D.None;
            flippingDown = context.action.triggered;
            playerState = state.FlipDown;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        switch(playerState){
            case state.Move:
                rb.velocity = new Vector2(0, movVector.y) * speed;
                break;
            case state.FlipUp:
                rb.AddTorque(flipTorque * 10f);
                if(!flippingUp){
                    playerState = state.ResetDown;
                    flippingUp = true;
                }
                break;
            case state.ResetDown:
                rb.AddTorque((-1) * flipTorque);
                if(Mathf.Round(gameObject.transform.rotation.eulerAngles.z)%360 == Mathf.Abs(hj.limits.min)){
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    hj.enabled = false;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation & RigidbodyConstraints2D.FreezePositionX;
                    playerState = state.Move;
                    flippingUp = false;
                }
                break;
            case state.FlipDown:
                rb.AddTorque(flipTorque * -10f);
                if(!flippingDown){
                    playerState = state.ResetUp;
                    flippingDown = true;
                }
                break;
            case state.ResetUp:
                rb.AddTorque(flipTorque);
                if(Mathf.Round(gameObject.transform.rotation.eulerAngles.z)%360 == Mathf.Abs(hj.limits.min)){
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    rb.transform.position = new Vector3(setX, rb.transform.position.y, rb.transform.position.z);
                    hj.enabled = false;
                    flippingDown = false;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation & RigidbodyConstraints2D.FreezePositionX;
                    playerState = state.Move;
                }
                break;
        }
        Debug.Log(gameObject.name + Mathf.Round(gameObject.transform.rotation.eulerAngles.z)%360);
    }
}
