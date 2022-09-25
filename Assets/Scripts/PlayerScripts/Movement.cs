using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Movement : MonoBehaviour
{
    // Class handling the player's movement
    public bool canMove;

    public CharacterController characterBody;
    public Transform characterPos;

    public float playerSpeed = 12f;
    private float originalPlayerSpeed;

    bool startsRunning;
    bool running;
    public int maxStamina;
    private int currentStamina;
    public float staminaLossRate;
    public float staminaGainRate;

    bool isMoving;

    bool playingWalkingSound;

    [SerializeField]
    private AudioSource movementSounds;
    
    [SerializeField]
    private Volume staminaExhaustionFilter;

    System.Random rnd;
    [SerializeField]
    private RND random;

    [HideInInspector]
    public float gravity = -9.81f;

    Vector3 velocity;

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundDistance = 0.4f;
    [SerializeField]
    private LayerMask groundMask;
    bool isGrounded;

    [SerializeField]
    private Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        originalPlayerSpeed = playerSpeed;
        canMove = true;
        currentStamina = maxStamina;
        startsRunning = false;
        running = false;
        isMoving = false;
        playingWalkingSound = false;

        rnd = random.rnd;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is on the ground, set vertical velocity accordingly
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Handles movement input from arrow keys or WASD
        if(canMove)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * horizontal + transform.forward * z;
            if(move != Vector3.zero) isMoving = true;
            else isMoving = false; 

            characterBody.Move(move * playerSpeed * Time.deltaTime);
        }
        
        if(!isGrounded) velocity.y += gravity * Time.deltaTime;
        characterBody.Move(velocity * Time.deltaTime);

    }

    // Handles running
    void FixedUpdate()
    {
        if(startsRunning && !running)
        {
            playerSpeed = originalPlayerSpeed * 2.5f;
            running = true;
            startsRunning = false;
            StartCoroutine(CharacterRun());
        }
    }

    // Handles visual elements of movement
    void OnGUI()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !running && currentStamina > maxStamina/4)
        { 
            startsRunning = true;
        }

        if(Input.GetKeyUp(KeyCode.Space) && running)
        {
            running = false;
        }

        if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && !playingWalkingSound && isMoving) 
        {
            StartCoroutine(PlayMovementSounds());
        }

        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) playerAnimator.SetBool("WalkingBack", true);
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) playerAnimator.SetBool("WalkingLeft", true);
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) playerAnimator.SetBool("WalkingForward", true);
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) playerAnimator.SetBool("WalkingRight", true);
        if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) playerAnimator.SetBool("WalkingBack", false);
        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) playerAnimator.SetBool("WalkingLeft", false);
        if(Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) playerAnimator.SetBool("WalkingForward", false);
        if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) playerAnimator.SetBool("WalkingRight", false);
    }

    // Handles "teleporting" the player
    public void MoveCharacter(Vector3 position)
    {
        characterBody.enabled = false;
        characterPos.position = position;
        characterBody.enabled = true;
    }

    public IEnumerator CharacterRun()
    {
        playerSpeed = originalPlayerSpeed * 1.5f;
        StartCoroutine(ReduceStamina());
        while(Input.GetKey(KeyCode.Space) && currentStamina > 0)
        {
            yield return null;
        }

        running = false;

        StartCoroutine(ReplenishStamina());
        StopCoroutine(CharacterRun());
    }

    public IEnumerator ReduceStamina()
    {
        while(Input.GetKey(KeyCode.Space) && currentStamina > 0)
        {
            currentStamina--;
            yield return new WaitForSeconds(staminaLossRate);
        }
        StopCoroutine(ReduceStamina());
    }

    public IEnumerator ReplenishStamina()
    {
        playerSpeed = originalPlayerSpeed;
        while(Input.GetKey(KeyCode.Space)) yield return new WaitForSeconds(staminaGainRate);
        while(!startsRunning && !running && currentStamina < maxStamina)
        {
            currentStamina++;
            yield return new WaitForSeconds(1f);
        }
        StopCoroutine(ReplenishStamina());
    }

    public IEnumerator PlayMovementSounds()
    {
        playingWalkingSound = true;
        movementSounds.Play();
        if(!running) yield return new WaitForSeconds(.7f);
        else
        { 
            yield return new WaitForSeconds(.3f);
        }
        playingWalkingSound = false;
        StopCoroutine(PlayMovementSounds());
    }
}
