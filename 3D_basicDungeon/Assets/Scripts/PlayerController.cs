using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public LayerMask groundLayerMask; // 바닥 체크용

    [Header("Animations")]
    public Animator animator;
    public PlayerInput playerInput;

    private Vector2 moveInput;
    private Rigidbody rigid;
    private bool isGrounded; // 점프 상태

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
        float speed = new Vector3(rigid.velocity.x, 0, rigid.velocity.z).magnitude; // 이동 속도
        animator.SetFloat("Speed", speed);

        // 점프 애니메이션 처리
        animator.SetFloat("JumpVelocity", rigid.velocity.y); // 점프 가중치
        animator.SetBool("IsGrounded", isGrounded);
    }

    // 물리 업데이트에서 이동 처리
    private void FixedUpdate()
    {
        Movement();
    }

    // InputSystem: Move 이벤트와 연결
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); // 이동 입력값 읽기
    }

    // InputSystem: Jump 이벤트와 연결
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump"); // 점프 애니메이션 트리거
        }
    }

    private void Movement()
    {
        // direction: 이동방향, moveInput: 이동 입력값, velocity: 이동속도
        Vector3 direction = transform.forward * moveInput.y + transform.right * moveInput.x;
        Vector3 velocity = direction * moveSpeed; // dir에 속도 곱하기
        velocity.y = rigid.velocity.y; // y축 속도는 유지
        rigid.velocity = velocity; // Rigidbody에 속도 적용

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(direction); // 이동 방향으로 회전
        }
    }

    // 바닥 체크 - Raycast로 확인
    private bool IsGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        float distance = 0.2f;
        Vector3[] rays = new Vector3[]
        {
            Vector3.down,
            (transform.forward + Vector3.down).normalized,
            (-transform.forward + Vector3.down).normalized,
            (transform.right + Vector3.down).normalized,
            (-transform.right + Vector3.down.normalized)
        };

        foreach (var rayDirection in rays)
        {
            // Raycast를 통해 바닥 체크
            // 바닥에 닿아있으면 true 반환
            if (Physics.Raycast(origin, rayDirection, distance, groundLayerMask))
            {
                return true;

            }
        }
        return false; // 바닥에 닿아있지 않으면 false 반환
    }

}
