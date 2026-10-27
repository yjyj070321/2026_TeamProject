using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    // 짧게 눌렀을 때 적용되는 최소 점프력
    public float minJumpPower = 5f;

    // 오래 눌렀을 때 적용되는 최대 점프력
    public float maxJumpPower = 12f;

    // 점프력을 증가시킬 수 있는 최대 누름 시간
    public float maxJumpHoldTime = 0.3f;

    public float gravity = -20f;
    public float mouseSensitivity = 0.2f;

    public Vector3 cameraOffset = new Vector3(0f, 1.5f, -5f);

    public Transform cameraPivot;
    public Transform cameraTransform;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isRunning;

    // 스페이스바를 누르고 있는지 여부
    private bool isJumping;

    // 스페이스바를 누른 시간
    private float jumpHoldTime;

    private float pitch = 20f;
    private float verticalVelocity;
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;

        cameraTransform.localPosition = cameraOffset;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        // 스페이스바를 누른 순간 점프 시작
        if (value.isPressed &&
            controller.isGrounded &&
            !isJumping)
        {
            isJumping = true;
            jumpHoldTime = 0f;
            verticalVelocity = minJumpPower;
        }
    }
            
    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    private void Update()
    {
        transform.Rotate(
            0f,
            lookInput.x * mouseSensitivity,
            0f);

        pitch -= lookInput.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -20f, 60f);

        cameraPivot.localEulerAngles =
            new Vector3(pitch, 0f, 0f);

        // 현재 스페이스바가 눌려 있는지 확인
        bool isSpaceHeld =
            Keyboard.current != null &&
            Keyboard.current.spaceKey.isPressed;

        // 스페이스바를 떼면 점프력 증가 중지
        if (!isSpaceHeld)
        {
            isJumping = false;
        }

        // 스페이스바를 누르고 있는 동안 점프력 증가
        if (isJumping &&
            isSpaceHeld &&
            jumpHoldTime < maxJumpHoldTime)
        {
            jumpHoldTime += Time.deltaTime;

            float jumpRatio =
                jumpHoldTime / maxJumpHoldTime;

            verticalVelocity = Mathf.Lerp(
                minJumpPower,
                maxJumpPower,
                jumpRatio);
        }

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 move =
            transform.forward * moveInput.y +
            transform.right * moveInput.x;

        move *= moveSpeed;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }
}
