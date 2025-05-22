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
    Condition health { get { return uiCondition.health; } }
    Condition power { get { return uiCondition.power; } }

    public float noHungerHealthDecay;

    public event Action onTakeDamage;

    void Update()
    {
        if (health == null) return; // 방어코드
        if (health.curValue == 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }
    public void Die()
    {
        Debug.Log("Player has died.");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Drink(float amount)
    {
        power.Add(amount);
    }
}
