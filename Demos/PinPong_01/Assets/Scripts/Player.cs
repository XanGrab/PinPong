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
    GameObject[] walls;

    void Start()
    {
        setX = transform.position.x;
        rb = gameObject.GetComponent<Rigidbody2D>(); 
        hj = gameObject.GetComponent<HingeJoint2D>();
        hj.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        playerState = state.Move;
        walls = GameObject.FindGameObjectsWithTag("Wall");
    }

    //check for regognized input
    public void OnMove( InputAction.CallbackContext context ){
        movVector = context.ReadValue<Vector2>() * speed;
    }

    public void OnFlipUp( InputAction.CallbackContext context ){
        if((playerState == state.Move) || (playerState == state.ResetDown)){
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
        if((playerState == state.Move) || (playerState == state.ResetUp)){
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

    /*void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Wall"){
            if(playerState != state.Move){
                
            }
        }
    }*/

    void FixedUpdate()
    {
        
        switch(playerState){
            case state.Move:
                rb.velocity = new Vector2(0, movVector.y) * speed;
                rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
                break;
            case state.FlipUp:
                //disable wall collisions
                foreach(GameObject wall in walls){
                    Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), wall.gameObject.GetComponent<Collider2D>());
                }

                rb.AddTorque(flipTorque * 10f);
                if(!flippingUp){
                    playerState = state.ResetDown;
                }
                break;
            case state.ResetDown:
                rb.AddTorque((-1) * flipTorque);
                if(Mathf.Round(gameObject.transform.rotation.eulerAngles.z)%360 == Mathf.Abs(hj.limits.min)){
                    //re-enable wall collisions
                    foreach(GameObject wall in walls){
                        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), wall.gameObject.GetComponent<Collider2D>(), false);
                    }

                    rb.transform.position = new Vector3(setX, rb.transform.position.y, rb.transform.position.z);
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    hj.enabled = false;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
                    flippingUp = false;
                    playerState = state.Move;
                }
                break;
            case state.FlipDown:
                //disable wall collisions
                foreach(GameObject wall in walls){
                    Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), wall.gameObject.GetComponent<Collider2D>());
                }

                rb.AddTorque(flipTorque * -10f);
                if(!flippingDown){
                    playerState = state.ResetUp;
                }
                break;
            case state.ResetUp:
                rb.AddTorque(flipTorque);
                if(Mathf.Round(gameObject.transform.rotation.eulerAngles.z)%360 == Mathf.Abs(hj.limits.min)){
                    //re-enable wall collisions
                    foreach(GameObject wall in walls){
                        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), wall.gameObject.GetComponent<Collider2D>(), false);
                    }

                    rb.transform.position = new Vector3(setX, rb.transform.position.y, rb.transform.position.z);
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    hj.enabled = false;
                    flippingDown = false;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
                    playerState = state.Move;
                }
                break;
        }
    }
}
