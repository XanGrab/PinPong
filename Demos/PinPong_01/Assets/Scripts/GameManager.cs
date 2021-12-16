using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
    
    public static GameManager Instance {
    get {
        if (_instance == null) {
        Debug.LogError( "Game Manager is NULL!");
        }
        return _instance;
        }
    }

    [Header("Ball")]
    public GameObject ballPrefab;
    public GameObject ball;
    public GameObject displayPrefab;
    public GameObject launchTxt;

    [Header("Player Input")]
    private PlayerInput playerInput;
    private PlayerControls controls;

    [Header("Left Player")]
    public GameObject lefty;
    public GameObject leftGoal;
    private PlayerInput leftInput;


    [Header("Right Player")]
    public GameObject righty;
    public GameObject rightGoal;
    private PlayerInput rightInput;

    [Header("UI")]
    public GameObject gameMenu;
    public GameObject lScoreTxt;
    public GameObject rScoreTxt;
    public GameObject resetMenu;
    public GameObject pauseMenu;
    public Settings settings;
    private int lScore;
    private int rScore;

    [Header("Pause")]
    [SerializeField]
    private InputAction pauseButton;
    private static bool playing;
    public static bool gamePaused;
    private float timeValue;

    [Header("Targets")]
    public GameObject targetManager;
    public GameObject pointsTargetManager;
    public GameObject powerupManager;
    public GameObject currentLayout;
    public GameObject pointTargetsCurrentLayout;
    public GameObject powerupCurrentLayout;
    public int targetsBroken = 0;
    public int pointTargetsBroken = 0;
    public int numTargets;
    public int targetHealthPickUp;
    public float powerupTimerValue = 20;

    AudioManager am;

    private void OnEnable() {
        pauseButton.Enable();    
    }
    private void OnDisable() {
        pauseButton.Disable();
    }

    private void UpdateControllers(){
        // always assign the keyboard assuming not null
        if(Keyboard.current != null){
            leftInput = lefty.GetComponent<PlayerInput>();
            string leftDevice = leftInput.defaultControlScheme;
            leftInput.SwitchCurrentControlScheme(leftDevice, Keyboard.current);
            
            rightInput = righty.GetComponent<PlayerInput>();
            string rightDevice = rightInput.defaultControlScheme;
            rightInput.SwitchCurrentControlScheme(rightDevice, Keyboard.current);
        }
        
        // assign gamepad if connected
        if(Gamepad.all.Count == 1){
            leftInput.SwitchCurrentControlScheme(Gamepad.current);
        }else if(Gamepad.all.Count > 1){
            Gamepad[] gamepads = Gamepad.all.ToArray();
            leftInput.SwitchCurrentControlScheme(gamepads[0]);
            Debug.Log("left paired w: " + gamepads[0].name);
            rightInput.SwitchCurrentControlScheme(gamepads[1]);
            Debug.Log("right paired w: " + gamepads[1].name);
        }
        Debug.Log("Gamepads: " + Gamepad.all.Count);
    }

    void Awake(){
        settings = FindObjectOfType<Settings>();
        if(settings == null){
            Debug.LogError("null settings");
            return;
        }
        settings.GetComponent<Canvas>().enabled = false;

        UpdateControllers();
        InputSystem.onDeviceChange += (device, change) => {
            switch(change){
                default:
                    UpdateControllers();
                    break;
            }
        };
        
        _instance = this;
        pauseButton.performed += ctx => OnPause();
    }
    void Start(){
        playing = true;
        gamePaused = false;
        gameMenu.SetActive(true);
        resetMenu.SetActive(false);
        launchTxt.SetActive(true);
        am = FindObjectOfType<AudioManager>();
        timeValue = 4f;

        // Set current target layout to first layout
        currentLayout = targetManager.transform.GetChild(0).gameObject;
        pointsTargetManager = GameObject.Find("Points Target Manager");
        pointTargetsCurrentLayout = pointsTargetManager.transform.GetChild(0).gameObject;
        powerupManager = GameObject.Find("Powerup Manager");
        powerupCurrentLayout = powerupManager.transform.GetChild(Random.Range(0, pointsTargetManager.transform.childCount - 1)).gameObject;
        numTargets = 4;
    }

    void Update(){
        if(playing && (!gamePaused)){
            powerupTimerValue -= Time.deltaTime;            
            /**
            * Launches the Countdown sequence if timeValue > 0, else tiemValue is set to -1 when disabled
            */
            if(Mathf.FloorToInt(timeValue) > 0){
                launchTxt.GetComponent<TextMeshProUGUI>().text = Mathf.FloorToInt(timeValue).ToString();
                timeValue -= Time.deltaTime;
            }if(Mathf.FloorToInt(timeValue) == 0){
                timeValue = -1;
                
                launchTxt.SetActive(false);
                ball = Instantiate(ballPrefab, new Vector3(0.0f, 0.0f, 1f), Quaternion.identity);
            }

            /**
            * Resets target layouts if all targets destroyed
            */
            if (targetsBroken == numTargets) {
                targetsBroken = 0;
                resetTargets(currentLayout);
            }

            if (pointTargetsBroken == numTargets) {
                pointTargetsBroken = 0;
                resetTargets(pointTargetsCurrentLayout);
            }

            if (powerupTimerValue < 1) {
                resetPowerUp();
                powerupTimerValue = Random.Range(15, 25);
            }
        }
    }

    /**
    * Handles the Event when a player scores
    * 
    * int param: player - determins the scoring player
    */
    public void Score( int player ){
        // Play Score Sound and Update Health        
        am.Play("Break");
        if(player == -1){ // Right Player Scores
            rScore += ball.GetComponent<Ball>().score;
            lefty.GetComponent<HP>().hp -= ball.GetComponent<Ball>().score;
            lefty.GetComponent<HP>().UpdateHealth();
            if(lefty.GetComponent<HP>().hp <= 0){
                EndMatch();
                return;
            }
            rScoreTxt.GetComponent<TextMeshProUGUI>().text = rScore.ToString();
        }else if(player == 1){ // Left Player Scores
            lScore += ball.GetComponent<Ball>().score;
            righty.GetComponent<HP>().hp -= ball.GetComponent<Ball>().score;
            righty.GetComponent<HP>().UpdateHealth();
            if(righty.GetComponent<HP>().hp <= 0){
                EndMatch();
                return;
            }
            lScoreTxt.GetComponent<TextMeshProUGUI>().text = lScore.ToString();
        }

        // Ready Launch Sequence
        Destroy(ball);
        launchTxt.SetActive(true);
        timeValue = 4;
        updateNumTargets();
        resetTargets(currentLayout);
        resetTargets(pointTargetsCurrentLayout);

    }

    /**
    * This method Displays the ball's point value when a ball breaks a target
    */
    public void DisplayPoints(Vector3 position){
        GameObject display;
        display = Instantiate(displayPrefab, position, Quaternion.identity);
        display.GetComponentInChildren<TextMeshPro>().text = ball.GetComponent<Ball>().score.ToString();
        display.GetComponentInChildren<Animator>().Play("Ball Points", 0, 0f);
    }

    /**
    * This method Displays the points lost when a ball enters a goal
    */
    public void DisplayDamage(Vector3 position){
        GameObject display;

        if(position.x < 0){
            position.x += 1.5f;  
        }else{
            position.x -= 1.5f;  
        }
        display = Instantiate(displayPrefab, position, Quaternion.identity);
        display.GetComponentInChildren<TextMeshPro>().text = "-" + ball.GetComponent<Ball>().score.ToString();
        display.GetComponentInChildren<Animator>().Play("Damaged", 0, 0f);
    }

    /** 
    * int param: player - determins the scoring player
    */
    public void HPPickUp(int player){
        if(player == -1){
            lefty.GetComponent<HP>().hp += targetHealthPickUp;
            lefty.GetComponent<HP>().UpdateHealth();
        }else{
            righty.GetComponent<HP>().hp += targetHealthPickUp;
            righty.GetComponent<HP>().UpdateHealth();
        }
    }
    
    /**
    * Handles the pause and unpause event
    */
    public void OnPause(){
        //Debug.Log("OnPause Called.");
        FindObjectOfType<AudioManager>().Play("ButtonPress");
        if(gamePaused){
            //Debug.Log("Resume!");
            gamePaused = false;
            Time.timeScale = 1f;
            gameMenu.SetActive(true);
            pauseMenu.SetActive(false);
            settings.GetComponent<Canvas>().enabled = false;
            if(timeValue > 0){
                launchTxt.SetActive(true);
            }
        }else{
            //Debug.Log("Pause.");
            gamePaused = true;
            Time.timeScale = 0f;
            gameMenu.SetActive(false);
            pauseMenu.SetActive(true);
            settings.GetComponent<Canvas>().enabled = true;
            launchTxt.SetActive(false);
        }
    }

    /**
    * Reset targets given for some given layout
    * param curr - Layout object
    */
    public void resetTargets(GameObject curr){
        curr.SetActive(true);
        for(int i = 0; i < numTargets; i++){
        float randX = Random.Range(-9.5f, 9.5f);
        float randY = Random.Range(-5f, 5F);
            curr.transform.GetChild(i).position= new Vector3(randX, randY, 0);
            curr.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
    
    /**
    * Resets power-up items for a given layout
    */
    public void resetPowerUp() {
        powerupCurrentLayout.SetActive(false);
        int rand = Random.Range(0, (powerupManager.transform.childCount));
        powerupCurrentLayout = powerupManager.transform.GetChild(rand).gameObject;
        float randX = Random.Range(-9.5f, 9.5f);
        float randY = Random.Range(-5f, 5F);
        powerupCurrentLayout.transform.position = new Vector3(randX, randY, 0);
        powerupCurrentLayout.gameObject.SetActive(true);
    }

    /**
    *
    */
    public void updateNumTargets() {
        int leftHp = lefty.GetComponent<HP>().hp;
        int rightHp = righty.GetComponent<HP>().hp;
        if (((leftHp + rightHp) <= 1500) && ((leftHp + rightHp) > 1000)) {
            numTargets = 5;
        }
        if ((leftHp + rightHp) <= 1000) {
            numTargets = 6;
        }
    }

    /**
    * Enumerator handling the Win animation
    */
    private IEnumerator EndAnimation(GameObject winner, GameObject loser){
        am.Quiet("ArenaTheme", true);
        ParticleSystem death;
        winner.GetComponent<Player>().enabled = false;
        loser.GetComponent<Player>().enabled = false;
        winner.transform.position = new Vector3(winner.transform.position.x, 0f, winner.transform.position.z);
        
        winner.transform.GetChild(0).gameObject.SetActive(true);
        am.Play("Winner");
        death = loser.transform.GetChild(1).GetComponent<ParticleSystem>();
        loser.GetComponent<SpriteRenderer>().enabled = false;
        death.Play();
        yield return new WaitForSeconds(death.main.startLifetime.constantMax);
        death.Stop();
        yield return new WaitForSeconds(3f);
        winner.transform.GetChild(0).gameObject.SetActive(false);
        am.Quiet("ArenaTheme", false);
        winner.SetActive(false);
        loser.SetActive(false);
        loser.GetComponent<SpriteRenderer>().enabled = true;
        resetMenu.SetActive(true);
    }


    public void EndMatch(){
        timeValue = -1;
        ball.SetActive(false);
        targetManager.SetActive(false);
        pointsTargetManager.SetActive(false);
        powerupManager.SetActive(false);
        if(lefty.GetComponent<HP>().hp > righty.GetComponent<HP>().hp){
            //lefty.transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(EndAnimation(lefty, righty));
        }else if(lefty.GetComponent<HP>().hp < righty.GetComponent<HP>().hp){
            //righty.transform.GetChild(0).gameObject.SetActive(true);
            StartCoroutine(EndAnimation(righty, lefty));
        }
    }

    public void Rematch(){
        am.Play("ButtonPress");
        settings.GetComponent<Canvas>().enabled = true;
        SceneManager.LoadScene("Arena");
    }

    public void ToMainMenu(){
        am.Play("ButtonPress");
        if(gamePaused){
            OnPause();
        }
        
        am.Stop("ArenaTheme");
        am.Play("MenuTheme");
        SceneManager.LoadScene("Main Menu");
    }
}
