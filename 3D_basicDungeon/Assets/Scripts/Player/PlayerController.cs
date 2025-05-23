using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    public float fallGravity = 20f;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minLook;
    public float maxLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;

    public Animator animator;

    public bool canLook = true;
    public Action inventory;

    private Rigidbody _rigidbody;
    private JumpPad curJumpPad;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        Move();

        // 낙하 가속도 조정
        FallGravityVelocity();
        animator.SetBool("IsGround", IsGrounded());
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    // 이동 방향, 속도
    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;

        // 애니메이션 추가
        float speed = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.z).magnitude;
        animator.SetFloat("Speed", speed);
    }

    // 마우스 커서 방향으로 회전 (mnouseDelta)
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minLook, maxLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    // WASD 이동
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    // 마우스 이동
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    // Spacebar 점프
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            animator.SetTrigger("Jump");

            // 점프패드가 있다면, JumpPad 클래스의 jumpForce를 사용, 아니면 기본 점프
            float force = (curJumpPad != null) ? curJumpPad.jumpForce : jumpPower;

            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);

            curJumpPad = null; // 점프패드 해제
        }
        else if (context.phase == InputActionPhase.Started && !IsGrounded())
        {
            // 더블 점프를 하지 않음
            return;
        }
    }

    // 점프패드에서 점프
    public void EnterJumpPad(JumpPad jumpPad)
    {
        curJumpPad = jumpPad;
    }

    // 점프패드에서 나올 때
    public void ExitJumpPad(JumpPad jumpPad)
    {
        if (curJumpPad == jumpPad)
            curJumpPad = null;
    }

    // 낙하 가속도 조정
    private void FallGravityVelocity()
    {
        float velocityY = _rigidbody.velocity.y;

        if (velocityY < 0f)
        {
            Vector3 fall = Vector3.up * Physics.gravity.y * (fallGravity - 1);
            _rigidbody.AddForce(fall, ForceMode.Acceleration);
        }
    }

    // 땅 닿았는지 확인 (Raycast 사용)
    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position - (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position - (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    // 인벤토리 열기 (E)
    public void OnInventoryButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    // 커서 잠금 및 해제
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
