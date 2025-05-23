using System;
using UnityEngine;

// IDamageable 인터페이스 정의
public interface IDamageable
{
    public void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;

    Condition caffeine { get { return uiCondition.caffeine; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition power { get { return uiCondition.power; } }

    public float noHungerHealthDecay;
    public event Action onTakeDamage;

    [Header("Decay Rates")]
    public float caffeineDecayRate;
    public float hungerDecayRate;
    public float powerDecayRate;


    void Update()
    {
        // 배고픔 감소와 파워 자동 충전 처리 (Inspector창에서 Rate들 설정으로 감속 가능)
        hunger.Subtract(hunger.passiveValue * hungerDecayRate * Time.deltaTime);
        power.Add(power.passiveValue * powerDecayRate * Time.deltaTime);

        if (hunger.curValue == 0f)
        {
            caffeine.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (caffeine.curValue == 0f)
        {
            Die();
        }
    }

    // 체력역할 = caffeine
    public void Caffeine(float amount)
    {
        caffeine.Add(amount);
    }

    // 배고픔 = hunger
    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    // 파워 = power
    public void Drink(float amount)
    {
        power.Add(amount);
    }

    public void Die()
    {
        Debug.Log("Player has died.");
    }

    // 체력(카페인) 감소
    public void TakePhysicalDamage(int damage)
    {
        caffeine.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    // 파워사용 (장비 공격 및 채집 사용 시, 감소)
    public bool UsePower(float amount)
    {
        if (power.curValue - amount < 0f)
        {
            return false;
        }

        power.Subtract(amount);
        return true;
    }
}
