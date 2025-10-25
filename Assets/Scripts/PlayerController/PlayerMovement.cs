using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [Header("States")]
    public bool CanMove = true;
    public bool canJump = false;
    
    public bool isWalking;
    public bool isCrouching;

    [Space(10)]
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float crouchSpeed = 2f;
    [SerializeField] private float acceleration = 12f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -20f;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchHeight = 1.0f;
    private float originalHeight;
    [SerializeField] private float crouchSmooth = 6f;

    [Header("Camera Settings")]
    public Transform cameraTransform;
    [SerializeField] private float lookSensitivity = 2f;
    [SerializeField] private float lookXLimit = 80f;
    [SerializeField] private float crouchCamOffset = 0.5f;

    private CharacterController characterController;
    private Vector3 moveDirection;
    private Vector3 currentVelocity;
    private float rotationX = 0;
    private float targetSpeed;
    public float currentSpeed;
    private Vector3 camDefaultPos;

    private void Awake()
    {
        Instance = this;
        characterController = GetComponent<CharacterController>();
        originalHeight = characterController.height;
        camDefaultPos = cameraTransform.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!CanMove) return;
        HandleAllMovement();
    }

    void HandleAllMovement()
    {
        HandleCrouch();
        HandleJump();
        HandleCameraRotation();
        HandleMovement();
    }

    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
            ToggleCrouch();

        float targetHeight = isCrouching ? crouchHeight : originalHeight;
        characterController.height = Mathf.Lerp(characterController.height, targetHeight, Time.deltaTime * crouchSmooth);

        Vector3 targetCamPos = isCrouching
            ? camDefaultPos - new Vector3(0, crouchCamOffset, 0)
            : camDefaultPos;

        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, targetCamPos, Time.deltaTime * crouchSmooth);
    }

    private void ToggleCrouch()
    {
        if (isCrouching) if (Physics.Raycast(transform.position, Vector3.up, originalHeight - characterController.height + 0.1f)) return;
        isCrouching = !isCrouching;
    }

    public void HandleJump()
    {
        if (isCrouching) return; 
        if (characterController.isGrounded && canJump && Input.GetKeyDown(KeyCode.Space)) currentVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

    }

    private void HandleMovement()
    {
        bool isGrounded = characterController.isGrounded;
        if (isGrounded && currentVelocity.y < 0) currentVelocity.y = -2f;

        targetSpeed = isCrouching ? crouchSpeed : walkSpeed;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        currentSpeed = (inputDirection.magnitude <= 0)
            ? Mathf.Lerp(currentSpeed, 0, acceleration * Time.deltaTime)
            : Mathf.Lerp(currentSpeed, targetSpeed, acceleration * Time.deltaTime);

        if (inputDirection.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            isWalking = true;
        }
        else { moveDirection = Vector3.zero; isWalking = false; }

        Vector3 move = moveDirection.normalized * currentSpeed * Time.deltaTime;
        characterController.Move(move);

        currentVelocity.y += gravity * Time.deltaTime;
        characterController.Move(currentVelocity * Time.deltaTime);
    }

    private void HandleCameraRotation()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        float rotationY = Input.GetAxis("Mouse X") * lookSensitivity;
        transform.rotation *= Quaternion.Euler(0, rotationY, 0);
    }
}
