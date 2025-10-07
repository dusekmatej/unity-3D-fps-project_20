using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float walkSpeed = 6f;
    public float sprintSpeed = 13f;
    public float crouchSpeed = 3f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    
    private float _defaultWalkSpeed;
    private float _defaultSprintSpeed;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public float slideSpeed = 20f;
    public float slideDuration = 1f;
    public float crouchHeight = 1f;
    private float originalHeight;
    
    private Coroutine _speedBoostCoroutine;
    
    private Vector3 velocity;
    private bool isGrounded;
    private bool isSliding;
    private bool isCrouching;
    private float slideTimer;

    private CharacterController charController;

    void Start()
    {
        charController = GetComponent<CharacterController>();
        originalHeight = charController.height;
        
        _defaultWalkSpeed = walkSpeed;
        _defaultSprintSpeed = sprintSpeed;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Jump
        if (isGrounded && Input.GetKeyDown(KeyCode.Space) && !isSliding)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        // Toggle crouch
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isSliding)
        {
            if (isCrouching)
                StopCrouch();
            else
                StartCrouch();
        }

        // Slide (only from sprint + crouch while moving)
        if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftShift) && move.magnitude > 0.1f && isGrounded)
        {
            StartSlide();
        }

        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            controller.Move(move.normalized * slideSpeed * Time.deltaTime);

            if (slideTimer <= 0)
                StopSlide();
        }
        else
        {
            float currentSpeed = isCrouching ? crouchSpeed :
                                 Input.GetKey(KeyCode.LeftShift) ? sprintSpeed :
                                 walkSpeed;

            controller.Move(move.normalized * currentSpeed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Cancel slide manually (optional)
        if (Input.GetKeyUp(KeyCode.LeftControl) && isSliding)
        {
            StopSlide();
        }
    }

    void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;
        charController.height = crouchHeight;
    }

    void StopSlide()
    {
        isSliding = false;
        StopCrouch(); // Go back to crouch or stand
    }

    void StartCrouch()
    {
        isCrouching = true;
        charController.height = crouchHeight;
    }

    void StopCrouch()
    {
        isCrouching = false;
        charController.height = originalHeight;
    }
    
    public void BoostSpeed(float boostAmount, float boostDuration)
    {
        if (_speedBoostCoroutine != null)
            StopCoroutine(_speedBoostCoroutine);
        
        _speedBoostCoroutine = StartCoroutine(SpeedBoostRoutine(boostAmount, boostDuration));
    }
    
    private IEnumerator SpeedBoostRoutine(float boostAmount, float boostDuration)
    {
        walkSpeed += boostAmount;
        sprintSpeed += boostAmount;
        
        yield return new WaitForSeconds(boostDuration);
        
        walkSpeed = _defaultWalkSpeed;
        sprintSpeed = _defaultSprintSpeed;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
