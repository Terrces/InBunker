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
    [SerializeField] private float mouseSensitivity = 100.0f;
    [SerializeField] private Transform cameraTransform;

    private CharacterController characterController; // Ссылка на компонент CharacterController
    private Vector3 velocity; // Текущая вертикальная скорость (для гравитации/прыжка)
    private float xRotation = 0f; // Текущий поворот камеры по оси X (вверх/вниз)

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

    private void FixedUpdate() => HandleMovement();
    private void Update() => HandleCameraRotation();

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
        Vector2 mousePosition = lookAction.ReadValue<Vector2>();

        transform.Rotate(Vector3.up * (mousePosition.x * mouseSensitivity * Time.deltaTime));

        xRotation -= mousePosition.y * mouseSensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
