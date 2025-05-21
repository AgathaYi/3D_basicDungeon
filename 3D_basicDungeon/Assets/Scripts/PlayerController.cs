using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSmoothTime = 0.1f;
    public float jumpForce = 7f;
    public LayerMask groundLayerMask; // 바닥 체크용

    [Header("Animations")]
    public Animator animator;

    [Header("Look, Rotate")]
    public float lookSensitivity = 1f; // 마우스 감도
    public float rotateSpeed = 90f; // 회전 속도

    private Rigidbody rigid;
    private PlayerInput playerInput;
    private Vector2 moveInput;
    private Vector2 lookInput; // 마우스 입력값
    private float rotateInput; // 회전 입력값(Q/E키)

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        if (animator == null) // Animator가 없으면 자동으로 추가
        {
            animator = GetComponent<Animator>();
        }

        if (playerInput == null) // PlayerInput이 없으면 자동으로 추가
        {
            playerInput = GetComponent<PlayerInput>();
        }
    }

    private void Update()
    {
        // 마우스 시점 처리 (좌우)
        if (lookInput.sqrMagnitude > 0.01f)
        {
            transform.Rotate(0f, lookInput.x * lookSensitivity * Time.deltaTime, 0f);
        }

        // 애니메이터 이동속도 값
        float horizontalSpeed = new Vector3(rigid.velocity.x, 0, rigid.velocity.z).magnitude; // 이동 속도
        animator.SetFloat("Speed", horizontalSpeed);
    }

    // 물리 업데이트에서 이동 처리
    private void FixedUpdate()
    {
        Movement();
        Rotation();
    }

    // InputSystem: Move 이벤트와 연결
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed || context.started)
        {
            moveInput = context.ReadValue<Vector2>(); // 이동 입력값 읽기
        }
        else
        {
            moveInput = Vector2.zero; // 입력이 없으면 0으로 초기화
        }
    }

    // InputSystem: Look 이벤트와 연결
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    // InputSystem: Rotate 이벤트와 연결
    public void OnRotate(InputAction.CallbackContext context)
    {
        rotateInput = context.ReadValue<float>();
    }

    // InputSystem: Jump 이벤트와 연결
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && CheckGrounded())
        {
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump"); // 점프 애니메이션 트리거
        }
    }

    private void Movement()
    {
        // x : 좌우 이동, y: 앞뒤 이동
        // direction: 이동방향, moveInput: 이동 입력값, moveVelocity: 이동속도
        // 입력으로 이동 방향 계산 (월드좌표계 기준)
        Vector3 direction = transform.right * moveInput.x + transform.forward * moveInput.y;
        direction.Normalize(); // 대각선 이동 속도 보정

        Vector3 moveVelocity = direction * moveSpeed; // 이동 속도 계산
        moveVelocity.y = rigid.velocity.y; // y축 속도 유지

        rigid.velocity = moveVelocity;
    }

    private void Rotation()
    {
        if (Mathf.Abs(rotateInput) > 0.01f)
        {
            transform.Rotate(0f, rotateInput * rotateSpeed * Time.fixedDeltaTime, 0f); // Q/E키로 회전
            // fixedDeltaTime으로 하는 이유는 물리 업데이트에서 회전하기 때문
            // 즉, FixedUpdate()에서 프레임에 상관없이 일정한 속도로 회전
        }
    }

    // 바닥 체크 - Raycast로 확인

    private bool CheckGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        float rayDistance = 0.2f;

        // 중앙에서 Ray쏴서 닿으면 바닥에 닿아있다고 판단하여 true 반환
        if (Physics.Raycast(origin, Vector3.down,
            rayDistance, groundLayerMask))
            return true;

        if (Physics.Raycast(origin, (transform.forward + Vector3.down).normalized,
            rayDistance, groundLayerMask))
            return true;

        if (Physics.Raycast(origin, (-transform.forward + Vector3.down).normalized,
            rayDistance, groundLayerMask))
            return true;

        if (Physics.Raycast(origin, (transform.right + Vector3.down).normalized,
            rayDistance, groundLayerMask))
            return true;

        if (Physics.Raycast(origin, (-transform.right + Vector3.down).normalized,
            rayDistance, groundLayerMask))
            return true;

        return false; // 바닥에 닿아있지 않으면 false 반환
    }

}
