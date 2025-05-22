using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition hunger;
    public Condition health;
    public Condition power;

    void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
