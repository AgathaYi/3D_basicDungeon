using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSmoothTime = 0.1f;
    public float jumpForce = 7f;
    public LayerMask groundLayerMask; // �ٴ� üũ��

    [Header("Animations")]
    public Animator animator;

    [Header("Look, Rotate")]
    public float lookSensitivity = 1f; // ���콺 ����
    public float rotateSpeed = 90f; // ȸ�� �ӵ�

    private Rigidbody rigid;
    private PlayerInput playerInput;
    private Vector2 moveInput;
    private Vector2 lookInput; // ���콺 �Է°�
    private float rotateInput; // ȸ�� �Է°�(Q/EŰ)

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
        // ���콺 ���� ó�� (�¿�)
        if (lookInput.sqrMagnitude > 0.01f)
        {
            transform.Rotate(0f, lookInput.x * lookSensitivity * Time.deltaTime, 0f);
        }

        // �ִϸ����� �̵��ӵ� ��
        float horizontalSpeed = new Vector3(rigid.velocity.x, 0, rigid.velocity.z).magnitude; // �̵� �ӵ�
        animator.SetFloat("Speed", horizontalSpeed);
    }

    // ���� ������Ʈ���� �̵� ó��
    private void FixedUpdate()
    {
        Movement();
        Rotation();
    }

    // InputSystem: Move �̺�Ʈ�� ����
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed || context.started)
        {
            moveInput = context.ReadValue<Vector2>(); // �̵� �Է°� �б�
        }
        else
        {
            moveInput = Vector2.zero; // �Է��� ������ 0���� �ʱ�ȭ
        }
    }

    // InputSystem: Look �̺�Ʈ�� ����
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    // InputSystem: Rotate �̺�Ʈ�� ����
    public void OnRotate(InputAction.CallbackContext context)
    {
        rotateInput = context.ReadValue<float>();
    }

    // InputSystem: Jump �̺�Ʈ�� ����
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && CheckGrounded())
        {
            rigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump"); // ���� �ִϸ��̼� Ʈ����
        }
    }

    private void Movement()
    {
        // x : �¿� �̵�, y: �յ� �̵�
        // direction: �̵�����, moveInput: �̵� �Է°�, moveVelocity: �̵��ӵ�
        // �Է����� �̵� ���� ��� (������ǥ�� ����)
        Vector3 direction = transform.right * moveInput.x + transform.forward * moveInput.y;
        direction.Normalize(); // �밢�� �̵� �ӵ� ����

        Vector3 moveVelocity = direction * moveSpeed; // �̵� �ӵ� ���
        moveVelocity.y = rigid.velocity.y; // y�� �ӵ� ����

        rigid.velocity = moveVelocity;
    }

    private void Rotation()
    {
        if (Mathf.Abs(rotateInput) > 0.01f)
        {
            transform.Rotate(0f, rotateInput * rotateSpeed * Time.fixedDeltaTime, 0f); // Q/EŰ�� ȸ��
            // fixedDeltaTime���� �ϴ� ������ ���� ������Ʈ���� ȸ���ϱ� ����
            // ��, FixedUpdate()���� �����ӿ� ������� ������ �ӵ��� ȸ��
        }
    }

    // �ٴ� üũ - Raycast�� Ȯ��

    private bool CheckGrounded()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;
        float rayDistance = 0.2f;

        // �߾ӿ��� Ray���� ������ �ٴڿ� ����ִٰ� �Ǵ��Ͽ� true ��ȯ
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

        return false; // �ٴڿ� ������� ������ false ��ȯ
    }

}
