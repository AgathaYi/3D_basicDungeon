using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition caffeine;
    public Condition hunger;
    public Condition power;


    void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }

    void Update()
    {

    }
}