using System;
using UnityEngine;

// 일반 상호작용 데이터 정의
public enum InteractionType
{
    Door,
    JumpPad,
    Fire,
    Water,
}

[CreateAssetMenu(fileName = "InteractabelData", menuName = "Item/Interaction Data")]
public class InteractionData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    [TextArea] public string description; // TextArea는 여러 줄 입력 가능
    public InteractionType type;
}