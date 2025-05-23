using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    public float fallGravity = 20f;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Boost")] //corutine 사용
    public float boostDuration = 5f; //부스트 지속시간
    public float boostSpeed = 3f; //배수
    public Image powerBarImage;

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

    // 카메라 시점 회전
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

    // 마우스 델타 회전
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    // Power 관련 아이템 Drink() 사용시 부스트 효과
    public void PowerBooster()
    {
        // PowerBar 100% 채우기
        CharacterManager.Instance.Player.condition.
            Drink(CharacterManager.Instance.Player.condition.uiCondition.power.maxValue);

        // Couroutine으로 부스트 효과 적용
        StartCoroutine(PowerBoostCoroutine());
    }

    private IEnumerator PowerBoostCoroutine()
    {
        // 원래 속도, 색상 저장
        float originalSpeed = moveSpeed;
        var originalColor = powerBarImage.color;

        // 부스트 속도 적용
        moveSpeed *= boostSpeed;
        powerBarImage.color = new Color(0.6f, 0, 1, 1); // 보라빛으로 강조
        powerBarImage.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f); // 스케일 강조

        // 지속시간동안 대기
        yield return new WaitForSeconds(boostDuration);

        // 원래 값으로 복원
        moveSpeed = originalSpeed;
        powerBarImage.color = originalColor; // 원래 색상으로 복원
    }

    // Spacebar 점프
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            animator.SetTrigger("Jump");

            // 점프패드에 닿았다면, JumpPad 클래스의 jumpForce를 사용, 아니면 기본 점프
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

    private void OnCollisionEnter(Collision collision)
    {
        // 점프패드에 닿았을 때, JumpPad 클래스의 jumpForce를 사용
        if (collision.collider.TryGetComponent<JumpPad>(out var jumpPad))
        {
            curJumpPad = jumpPad;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // 점프패드에서 떨어졌을 때, JumpPad 클래스의 jumpForce를 사용하지 않음
        if (collision.collider.TryGetComponent<JumpPad>(out var jumpPad) && curJumpPad == jumpPad)
        {
            curJumpPad = null;
        }
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
