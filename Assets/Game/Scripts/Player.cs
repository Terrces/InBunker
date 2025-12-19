using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
public class Player : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpHeight = 25.0f;
    // [SerializeField] private float maxFallSpeed = 100.0f;

    [Header("Camera Settings")]
    [SerializeField] private bool gamepadMode = false;
    [SerializeField] private float mouseSensitivity = 25.0f;
    [SerializeField] private float gamepadSensitivity = 25.0f;
    [SerializeField] float smoothTime = 0.05f;
    [SerializeField] float gamepadSmoothTime = 0f;
    [SerializeField] private Transform cameraTransform;



    private CharacterController characterController; // Ссылка на компонент CharacterController
    private Vector3 velocity;
    private float xRotation = 0f;
    private float xRotationVelocity;
    private float yRotationVelocity;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (tag == "Untagged") tag = "Player";

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        lookAction = InputSystem.actions.FindAction("Look");

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    { 
        HandleCameraRotation();
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 move = transform.right * moveAction.ReadValue<Vector2>().x + transform.forward * moveAction.ReadValue<Vector2>().y;

        if(characterController.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (jumpAction.IsPressed() && characterController.isGrounded && jumpHeight != 0)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }
        else if(!characterController.isGrounded)
        {
            velocity.y += Physics.gravity.y * Time.deltaTime;
        }

        characterController.Move((Vector3.Normalize(move) * moveSpeed * Time.deltaTime) + new Vector3(0,velocity.y,0) * Time.deltaTime);
    }

    private void HandleCameraRotation()
    {
        Vector2 look = lookAction.ReadValue<Vector2>();

        if (gamepadMode) HandleGamepadCamera(look);
        else HandleMouseCamera(look);
    }

    private void HandleMouseCamera(Vector2 look)
    {
        float mouseX = look.x * mouseSensitivity * Time.deltaTime;
        float mouseY = look.y * mouseSensitivity * Time.deltaTime;

        float targetXRotation = xRotation - mouseY;
        targetXRotation = Mathf.Clamp(targetXRotation, -90f, 90f);

        float smoothYDelta = Mathf.SmoothDamp(
            0f,
            mouseX,
            ref yRotationVelocity,
            smoothTime
        );

        xRotation = Mathf.SmoothDamp(
            xRotation,
            targetXRotation,
            ref xRotationVelocity,
            smoothTime
        );

        transform.Rotate(Vector3.up * smoothYDelta);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void HandleGamepadCamera(Vector2 look)
    {
        if (look.sqrMagnitude < 0.001f)
            return;

        float stickX = look.x * gamepadSensitivity * Time.deltaTime;
        float stickY = look.y * gamepadSensitivity * Time.deltaTime;

        xRotation -= stickY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(Vector3.up * stickX);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    
    private void OnEnable() => InputSystem.onActionChange += OnActionChange;
    private void OnDisable() => InputSystem.onActionChange -= OnActionChange;

    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change != InputActionChange.ActionPerformed)
            return;

        var action = obj as InputAction;
        if (action == null || action.activeControl == null)
            return;

        gamepadMode = action.activeControl.device is Gamepad;
    }
}
