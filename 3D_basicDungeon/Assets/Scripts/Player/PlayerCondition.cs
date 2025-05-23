using System;
using UnityEngine;

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

    public void Heal(float amount)
    {
        caffeine.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Die()
    {
        Debug.Log("Player has died.");
    }

    public void TakePhysicalDamage(int damage)
    {
        caffeine.Subtract(damage);
        onTakeDamage?.Invoke();
    }

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
