using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerControl : MonoBehaviour
{
    enum MoveSpeed{
        idle,
        walk,
        run,
        sneak
    }
    [Header("Movement")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float runSneakMultiplier = 1.5f;

    [Header("Look")]
    [SerializeField] private float mouseSense = 2f;
    [SerializeField] private float upDownRange = 80f;

    [Header("Stamina")]
    [SerializeField] private float stam = 1000;
    [SerializeField] private float maxStam = 1000;
    [SerializeField] private float runStam = 1;
    [SerializeField] private float rechargeRate = 1;
    private Coroutine stamRecharge;
    private bool isRunning;



    [Header("Inputs Custom")]
    private string hMoveInput = "Horizontal";
    private string vMoveInput = "Vertical";
    private string mouseXInput = "Mouse X";
    private string mouseYInput = "Mouse Y";
    [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode sneakKey = KeyCode.LeftControl;

    [Header("View Pane")]
    [SerializeField] private ViewPane playerPane;



    private float vRotate;
    private Camera mainCam;
    private CharacterController controller;
    [SerializeField] private float sneakCrouchHeight = 2.75f;
    [SerializeField] private float standHeight = 4f;

    private Animator anim;
    private MoveSpeed currentSpeedType;
    void Awake(){
        GameManager.SetPlayer(transform);
        GameManager.SetPlayerPane(playerPane);
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        mainCam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller.height = standHeight;
        
    }

    // Update is called once per frame
    void Update()
    {
        rotateHandler();
        if (!GameManager.PauseInput){
            moveHandler();
            
            if (isRunning)
            {
                stamHandler();
                if (stamRecharge != null) { StopCoroutine(stamRecharge); }
                stamRecharge = StartCoroutine(RechargeStam());
            }
        }
        else if (anim.GetInteger("Speed") > 0) anim.SetInteger("Speed", 0);
        #if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.K)) GameManager.PlayerWon();//EditorApplication.isPlaying = false;
        #endif

    }
    private void SetAnimSpeed(Vector3 movement){
        MoveSpeed value = MoveSpeed.idle;
        if (movement.magnitude > 0){
            if (Input.GetKey(runKey) && stam > 0)
                value = MoveSpeed.run;
            else if (Input.GetKey(sneakKey)){
                value = MoveSpeed.sneak;
            }
            else
                value = MoveSpeed.walk;
        }
        if (currentSpeedType != value) SetAnimationFromSpeedType(value);
    }
    private void SetAnimationFromSpeedType(MoveSpeed speedType){
        currentSpeedType = speedType;
        switch(speedType){
            case MoveSpeed.idle:
                anim.Play("Idle");
                break;
            case MoveSpeed.walk:
                anim.Play("Player Walk");
                break;
            case MoveSpeed.sneak:
                anim.Play("Player Sneak");
                break;
            case MoveSpeed.run:
                anim.Play("Player Run");
                break;
        }
    }
    private void moveHandler()
    {
        float vSpeed;
        float hSpeed;
        float speedMultiplier = (Input.GetKey(runKey) && stam !=0) || Input.GetKey(sneakKey) ? runSneakMultiplier : 1f;
        if (Input.GetKey(sneakKey))
        {
            vSpeed = Input.GetAxisRaw(vMoveInput) * speed / speedMultiplier;
            hSpeed = Input.GetAxisRaw(hMoveInput) * speed / speedMultiplier;
            controller.height = sneakCrouchHeight;
        }
        else
        {
            vSpeed = Input.GetAxisRaw(vMoveInput) * speed * speedMultiplier;
            hSpeed = Input.GetAxisRaw(hMoveInput) * speed * speedMultiplier;
            controller.height = standHeight;
        }
        Vector3 move = new Vector3(hSpeed, 0, vSpeed);
        move = transform.rotation * move;

        controller.SimpleMove(move);
        isRunning = Input.GetKey(runKey) && (move.magnitude !=0) ? true : false;
        SetAnimSpeed(move);
        //Debug.Log(move.magnitude);
    }
    private void rotateHandler()
    {
        float xRotate = Input.GetAxis(mouseXInput) * mouseSense;
        transform.Rotate(0, xRotate, 0);
        vRotate -= Input.GetAxis(mouseYInput) * mouseSense;
        vRotate = Mathf.Clamp(vRotate, -upDownRange, upDownRange);
        mainCam.transform.localRotation = Quaternion.Euler(vRotate, 0, 0);
    }

    private void stamHandler()
    {
        stam = Input.GetKey(runKey) ? stam-(runStam / 10f) : stam;
        if (stam < 0) { stam = 0; }
    }
    private IEnumerator RechargeStam()
    {
        yield return new WaitForSeconds(1f);
        while (stam < maxStam)
        {
            stam += rechargeRate / 10f;
            if (stam > maxStam) { stam = maxStam; }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
