using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform playerCam;
    public float speed = 7f;
    public float sprintSpeed = 12f;
    public float crouchSpeed = 4f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.2f;
    public float airControl = 0.3f;
    public float acceleration = 10f;
    public float friction = 5f;
    public float mouseSensitivity = 100f;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching;
    private bool isSprinting;
    private Vector3 moveDirection;
    private float currentSpeed;
    private float xRotation = 0f;
    private Animator Animator;
    private int bulletIndex = 0;
    private Vector3[] bulletPattern = new Vector3[] {
        new Vector3(0f, 0f, 1f),
        new Vector3(0.02f, 0.01f, 1f),
        new Vector3(-0.02f, -0.01f, 1f),
        new Vector3(0.03f, -0.02f, 1f),
        new Vector3(-0.03f, 0.02f, 1f)
    };

    void Start()
    {
        currentSpeed = speed;
        Cursor.lockState = CursorLockMode.Locked;
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        HandleMovement();
        HandleMouseLook();
        ApplyGravity();
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        bool jump = Input.GetKey(KeyCode.Space);
        bool crouch = Input.GetKey(KeyCode.LeftControl);
        bool sprint = Input.GetKey(KeyCode.LeftShift);

        if (isGrounded)
        {
            isCrouching = crouch;
            isSprinting = sprint && !crouch;
            currentSpeed = isSprinting ? sprintSpeed : isCrouching ? crouchSpeed : speed;
            moveDirection = transform.right * moveX + transform.forward * moveZ;
            moveDirection.Normalize();

            // Friction
            if (moveDirection.magnitude < 0.1f)
            {
                velocity.x *= Mathf.Clamp01(1 - friction * Time.deltaTime);
                velocity.z *= Mathf.Clamp01(1 - friction * Time.deltaTime);
            }
            else
            {
                velocity.x = Mathf.Lerp(velocity.x, moveDirection.x * currentSpeed, acceleration * Time.deltaTime);
                velocity.z = Mathf.Lerp(velocity.z, moveDirection.z * currentSpeed, acceleration * Time.deltaTime);
            }

            if (jump)
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        else
        {
            // Air Control (Strafing)
            Vector3 airMove = transform.right * moveX + transform.forward * moveZ;
            airMove.Normalize();
            velocity.x += airMove.x * airControl * Time.deltaTime;
            velocity.z += airMove.z * airControl * Time.deltaTime;
        }
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void ApplyGravity()
    {
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    void Shoot()
    {
        Animator.SetBool("IsShooting", true);
        Vector3 bulletDirection = transform.TransformDirection(bulletPattern[bulletIndex]);
        bulletIndex = (bulletIndex + 1) % bulletPattern.Length;
        Debug.Log("Bullet Direction: " + bulletDirection);
    }
}
