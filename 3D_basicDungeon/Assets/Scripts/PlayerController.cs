using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public LayerMask groundLayerMask; // �ٴ� üũ��

    [Header("Animations")]
    public Animator animator;
    public PlayerInput playerInput;

    private Vector2 moveInput;
    private Rigidbody rigid;
    private bool isGrounded; // ���� ����

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        
        if (animator == null) // Animator�� ������ �ڵ����� �߰�
        {
            animator = GetComponent<Animator>();
        }

        if (playerInput == null) // PlayerInput�� ������ �ڵ����� �߰�
        {
            playerInput = GetComponent<PlayerInput>();
        }
    }

    private void Update()
    {
        float speed = new Vector3(rigid.velocity.x, 0, rigid.velocity.z).magnitude; // �̵� �ӵ�
        animator.SetFloat("Speed", speed);

        // ���� �ִϸ��̼� ó��
        animator.SetFloat("JumpVelocity", rigid.velocity.y); // ���� ����ġ
        animator.SetBool("IsGrounded", isGrounded);
    }

    // ���� ������Ʈ���� �̵� ó��
    private void FixedUpdate()
    {
        Movement();
    }

    // InputSystem: Move �̺�Ʈ�� ����
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>(); // �̵� �Է°� �б�
    }

    // InputSystem: Jump �̺�Ʈ�� ����
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump"); // ���� �ִϸ��̼� Ʈ����
        }
    }

    private void Movement()
    {
        // direction: �̵�����, moveInput: �̵� �Է°�, velocity: �̵��ӵ�
        Vector3 direction = transform.forward * moveInput.y + transform.right * moveInput.x;
        Vector3 velocity = direction * moveSpeed; // dir�� �ӵ� ���ϱ�
        velocity.y = rigid.velocity.y; // y�� �ӵ��� ����
        rigid.velocity = velocity; // Rigidbody�� �ӵ� ����

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.rotation = Quaternion.LookRotation(direction); // �̵� �������� ȸ��
        }
    }

    // �ٴ� üũ - Raycast�� Ȯ��
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
            // Raycast�� ���� �ٴ� üũ
            // �ٴڿ� ��������� true ��ȯ
            if (Physics.Raycast(origin, rayDirection, distance, groundLayerMask))
            {
                return true;

            }
        }
        return false; // �ٴڿ� ������� ������ false ��ȯ
    }

}
