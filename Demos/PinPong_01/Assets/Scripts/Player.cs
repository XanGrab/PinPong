using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerControls controls;
    public enum state{
        Move,
        FlipUp,
        FlipDown,
        ResetUp,
        ResetDown,
        Frozen
    }

    public bool hasSpeedPower;
    private state playerState;
    public float speed;
    public float flipTorque;

    private Vector2 movVector;
    private Rigidbody2D rb;
    private HingeJoint2D hj;
    GameObject[] walls;
    GameObject[] playerWalls;
    AudioManager am;

    void OnEnable(){ controls.Player.Enable(); }
    void OnDisable(){ controls.Player.Disable(); }

    void Awake(){
        PlayerInput input = GetComponent<PlayerInput>();
        string d = input.defaultControlScheme;
        input.SwitchCurrentControlScheme(d, Keyboard.current);
        controls = new PlayerControls();
        
        playerWalls = GameObject.FindGameObjectsWithTag("PlayerWall");
        foreach(GameObject wall in playerWalls){
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), wall.gameObject.GetComponent<Collider2D>());
        }
    }


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
        hj = gameObject.GetComponent<HingeJoint2D>();
        am = FindObjectOfType<AudioManager>();
        walls = GameObject.FindGameObjectsWithTag("Wall");

        SetMoveComponents();
        hasSpeedPower = false;
        playerState = state.Move;
    }

    public void OnMove( InputAction.CallbackContext context ){
        movVector.y = context.ReadValue<float>() * speed;
    }

    public void OnFlipUp( InputAction.CallbackContext ctx ){
        if(ctx.performed){
            if((playerState == state.Move) || (playerState == state.ResetDown)){
                if(hj.anchor.y != 0.5){
                    JointAngleLimits2D limits = hj.limits;
                    hj.anchor = new Vector2(0, 0.5f);
                    limits.max *= -1;
                    hj.limits = limits;
                }
                SetFlipComponents();
                playerState = state.FlipUp;
                FindObjectOfType<AudioManager>().Play("Flip");
            }
        }
    }

    public void OnFlipDown( InputAction.CallbackContext ctx ){
        if(ctx.performed){
            if((playerState == state.Move) || (playerState == state.ResetUp)){
                if(hj.anchor.y != -0.5){
                    JointAngleLimits2D limits = hj.limits;
                    limits.max *= -1;
                    hj.anchor = new Vector2(0, -0.5f);
                    hj.limits = limits;
                }
                SetFlipComponents();
                playerState = state.FlipDown;
                FindObjectOfType<AudioManager>().Play("Flip");
            }
        }
    }

    void SetMoveComponents(){
        hj.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
        
        foreach(GameObject wall in walls){
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), wall.gameObject.GetComponent<Collider2D>(), false);
        }
    }

    void SetFlipComponents(){
        rb.constraints = RigidbodyConstraints2D.None;
        hj.enabled = true;
        
        // Ignore wall collisions when flipping
        foreach(GameObject wall in walls){
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), wall.gameObject.GetComponent<Collider2D>());
        }
    }

    public void Freeze() {
        StartCoroutine(Frozen());
    }
    
    /**
    * Called by ball on power-up collect
    */
    private IEnumerator Frozen() {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        playerState = state.Frozen;
        GetComponent<SpriteRenderer>().color = Color.cyan;
        am.Play("Freeze");
        yield return new WaitForSeconds(1f);
        //Debug.Log("~UnFreeze~");
        GetComponent<SpriteRenderer>().color = Color.white;
        SetMoveComponents();
        playerState = state.Move;
    }


    void FixedUpdate()
    {        
        // Debug.Log(gameObject.name + " state: " + playerState);
        switch(playerState){
            case state.Move:
                rb.velocity = new Vector2(0, movVector.y * speed);
                break;
            case state.FlipUp:
                rb.AddTorque(flipTorque * 10f);

                playerState = state.ResetDown;
                break;
            case state.ResetDown:
                rb.AddTorque((-1) * flipTorque);
                if(Mathf.Round(gameObject.transform.rotation.eulerAngles.z)%360 == Mathf.Abs(hj.limits.min)){
                    //re-enable wall collisions
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    SetMoveComponents();
                    playerState = state.Move;
                }
                break;
            case state.FlipDown:
                rb.AddTorque(flipTorque * -10f);

                playerState = state.ResetUp;
                break;
            case state.ResetUp:
                rb.AddTorque(flipTorque);
                if(Mathf.Round(gameObject.transform.rotation.eulerAngles.z)%360 == Mathf.Abs(hj.limits.min)){
                    //re-enable wall collisions
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                    SetMoveComponents();
                    playerState = state.Move;
                }
                break;
            case state.Frozen:
                break;
        }
    }
}
