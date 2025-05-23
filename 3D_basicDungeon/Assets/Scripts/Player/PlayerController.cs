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

        // ���� ���ӵ� ����
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

    // �̵� ����, �ӵ�
    void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;

        // �ִϸ��̼� �߰�
        float speed = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.z).magnitude;
        animator.SetFloat("Speed", speed);
    }

    // ���콺 Ŀ�� �������� ȸ�� (mnouseDelta)
    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minLook, maxLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    // WASD �̵�
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

    // ���콺 �̵�
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    // Spacebar ����
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            animator.SetTrigger("Jump");

            // �����е尡 �ִٸ�, JumpPad Ŭ������ jumpForce�� ���, �ƴϸ� �⺻ ����
            float force = (curJumpPad != null) ? curJumpPad.jumpForce : jumpPower;

            _rigidbody.AddForce(Vector3.up * force, ForceMode.Impulse);

            curJumpPad = null; // �����е� ����
        }
        else if (context.phase == InputActionPhase.Started && !IsGrounded())
        {
            // ���� ������ ���� ����
            return;
        }
    }

    // �����е忡�� ����
    public void EnterJumpPad(JumpPad jumpPad)
    {
        curJumpPad = jumpPad;
    }

    // �����е忡�� ���� ��
    public void ExitJumpPad(JumpPad jumpPad)
    {
        if (curJumpPad == jumpPad)
            curJumpPad = null;
    }

    // ���� ���ӵ� ����
    private void FallGravityVelocity()
    {
        float velocityY = _rigidbody.velocity.y;

        if (velocityY < 0f)
        {
            Vector3 fall = Vector3.up * Physics.gravity.y * (fallGravity - 1);
            _rigidbody.AddForce(fall, ForceMode.Acceleration);
        }
    }

    // �� ��Ҵ��� Ȯ�� (Raycast ���)
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

    // �κ��丮 ���� (E)
    public void OnInventoryButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    // Ŀ�� ��� �� ����
    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
