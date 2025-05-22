using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;

    Condition hunger { get { return uiCondition.hunger; } }
    Condition caffeine { get { return uiCondition.health; } }
    Condition power { get { return uiCondition.power; } }

    public float noHungerHealthDecay;

    public event Action onTakeDamage;

    void Update()
    {
        if (caffeine == null) return; // 방어코드
        if (caffeine.curValue == 0f)
        {
            Die();
        }
    }

    public void TakePhysicalDamage(int damage)
    {
        caffeine.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public void Caffeine(float amount)
    {
        caffeine.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Drink(float amount)
    {
        power.Add(amount);
    }

    public bool CaffeineDecay(float amount)
    {
        if (caffeine.curValue - amount < 0f)
        {
            return false;
        }

        caffeine.Subtract(amount);
        return true;
    }

    public bool HungerDecay(float amount)
    {
        if (hunger.curValue - amount < 0f)
        {
            return false;
        }
        hunger.Subtract(amount);
        return true;
    }

    public bool PowerDecay(float amount)
    {
        if (power.curValue - amount < 0f)
        {
            return false;
        }
        power.Subtract(amount);
        return true;
    }

    public void Die()
    {
        Debug.Log("Player has died.");
    }
}
