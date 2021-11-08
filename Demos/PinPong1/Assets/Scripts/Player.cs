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

    private state playerState;
    public float speed;
    public float flipTorque;
    private Vector2 movVector;
    private bool flipping;
    private Rigidbody2D rb;
    private HingeJoint2D hj;


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
        hj = gameObject.GetComponent<HingeJoint2D>();
        hj.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerState = state.Move;
    }

    //check for regognized input
    public void OnMove( InputAction.CallbackContext context ){
        movVector = context.ReadValue<Vector2>() * speed;
        //Debug.Log(gameObject.name + ".OnMove() Action requested");
    }

    public void OnFlip( InputAction.CallbackContext context ){
        hj.enabled = true;
        rb.constraints = RigidbodyConstraints2D.None;
        flipping = context.action.triggered;
        playerState = state.FlipUp;
        //Debug.Log(gameObject.name + ".OnFlip() Action requested");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(playerState){
            case state.Move:
            rb.bodyType = RigidbodyType2D.Dynamic;
                gameObject.GetComponent<Collider2D>().isTrigger = false;
                //Debug.Log(gameObject.name + "state = Move");
                rb.velocity = new Vector2(0, movVector.y) * speed;
                break;
            case state.FlipUp:
                //Debug.Log(gameObject.name + "state = FlipUp");
                Debug.Log("Z: " + gameObject.transform.rotation.z);
                //Debug.Log("Min: " + hj.limits.min);
                rb.AddTorque(flipTorque * 10f);
                if(!flipping){
                    playerState = state.ResetDown;
                }
                break;
            case state.ResetDown:
                //Debug.Log(gameObject.name + "state = ResetDown");
                rb.AddTorque((-1) * flipTorque);
                //gameObject.GetComponent<Collider2D>().isTrigger = true;
                if(Mathf.Round(gameObject.transform.rotation.z) != Mathf.Abs(hj.limits.min)){
                    //gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    hj.enabled = false;
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    playerState = state.Move;
                }
                break;
        }
    }
}
