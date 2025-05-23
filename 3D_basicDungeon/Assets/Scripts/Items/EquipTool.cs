using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;
    public float usePower;

    [Header("Resource Gathering")]
    public bool doesGatherResource;

    [Header("Combat")]
    public bool doesDealDamage;
    public int damage;

    private Animator animator;
    private Camera _camera;

    void Start()
    {
        animator = GetComponent<Animator>();
        _camera = Camera.main;
    }

    // 공격 입력처리
    public override void OnAttackInput()
    {
        if (!attacking)
        {
            if (CharacterManager.Instance.Player.condition.UsePower(usePower))
            {
                attacking = true;
                animator.SetTrigger("Attack");
                Invoke("OnCanAttack", attackRate);
            }
        }
    }

    // 공격 애니메이션이 끝났을 때 호출되는 메서드
    void OnCanAttack()
    {
        attacking = false;
    }

    // 공격 시 적에게 데미지 주기. 애니메이션 이벤트 등록
    public void OnHit()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            // 리소스 채집 처리
            if (doesGatherResource && hit.collider.TryGetComponent(out Resource resource))
                resource.Gether(hit.point, hit.normal);
        }
    }
}
