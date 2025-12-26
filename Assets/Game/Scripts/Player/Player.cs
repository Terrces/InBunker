// Simple 3D character controller for a Unity project.
// Uses Unity's new Input System.
// Work in progress, some features are not implemented yet.

using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
public class Player : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpForce = 2.0f;

    [Header("Camera Settings")]
    // mouse properties
    [SerializeField] private float mouseSensitivity = 25.0f;
    [SerializeField] private float mouseSmoothTime = 0.05f;

    // gamepad properties
    [SerializeField] private bool gamepadMode = false;
    [SerializeField] private float gamepadSensitivity = 250.0f;
    [SerializeField] private float gamepadDeadZone = 0.001f;
    public Transform cameraTransform;

    private CharacterController characterController;
    private Vector3 velocity;
    private float xRotation = 0f;
    private float xRotationVelocity;
    private float yRotationVelocity;
    
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;
    private InputAction interactAction;
    private Interaction interactionComponent;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        lookAction = InputSystem.actions.FindAction("Look");
        // if you need interact
        interactionComponent = GetComponent<Interaction>();
        interactAction = InputSystem.actions.FindAction("Interact");

    }

    private void Start()
    {
        if (tag == "Untagged") tag = "Player";

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    { 
        HandleCameraRotation();
        HandleMovement();
        if (interactAction.WasPressedThisFrame()) interactionComponent.OnInteract();
    }
    
    #region Input

    private void OnEnable() => InputSystem.onActionChange += OnActionChange;
    private void OnDisable() => InputSystem.onActionChange -= OnActionChange;

    private void OnActionChange(object obj, InputActionChange change)
    {
        var action = obj as InputAction;

        if (change != InputActionChange.ActionPerformed) return;
        if (action == null || action.activeControl == null) return;

        gamepadMode = action.activeControl.device is Gamepad;
    }

    #endregion

    #region Player Movement

    private void HandleMovement()
    {
        Vector3 move = transform.right * moveAction.ReadValue<Vector2>().x + transform.forward * moveAction.ReadValue<Vector2>().y;

        if(characterController.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (jumpAction.WasPressedThisFrame() && characterController.isGrounded && jumpForce != 0)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }
        else if(!characterController.isGrounded)
        {
            velocity.y += Physics.gravity.y * Time.deltaTime;
        }

        Vector3 motion = move.normalized * moveSpeed;
        motion.y = velocity.y;

        CollisionFlags flags = characterController.Move(motion * Time.deltaTime);

        if ((flags & CollisionFlags.Above) != 0 && velocity.y > 0)
        {
            velocity.y = -1f;
        }
    }

    #endregion

    #region Handle camera rotations

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

        float smoothYDelta = Mathf.SmoothDamp(0f, mouseX, ref yRotationVelocity, mouseSmoothTime );
        xRotation = Mathf.SmoothDamp(xRotation, targetXRotation, ref xRotationVelocity, mouseSmoothTime);

        transform.Rotate(Vector3.up * smoothYDelta);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    private void HandleGamepadCamera(Vector2 look)
    {
        if (look.sqrMagnitude < gamepadDeadZone) return;

        float stickX = look.x * gamepadSensitivity * Time.deltaTime;
        float stickY = look.y * gamepadSensitivity * Time.deltaTime;

        xRotation -= stickY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(Vector3.up * stickX);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    
    #endregion

}
