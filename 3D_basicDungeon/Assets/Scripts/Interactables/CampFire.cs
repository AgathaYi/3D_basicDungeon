using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour, IInteractable
{
    public InteractionData data;

    public int damage;
    public float damageRate;

    List<IDamageable> things = new List<IDamageable>();

    void Start()
    {
        InvokeRepeating("DealDamage", 0, damageRate);
    }

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        // 정보만 표시 할 것.
        // 획득 불가한 기물
    }

    // 주기적으로 데미지 주기
    void DealDamage()
    {
        for (int i = 0; i < things.Count; i++)
        {
            things[i].TakePhysicalDamage(damage);
        }
    }

    // Trigger에 들어온 물체가 IDamageable을 구현하고 있는지 확인
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            things.Add(damageable);
        }
    }

    // Trigger에서 나간 물체가 IDamageable을 구현하고 있는지 확인
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            things.Remove(damageable);
        }
    }
}
