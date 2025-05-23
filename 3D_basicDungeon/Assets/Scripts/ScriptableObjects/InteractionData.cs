using System;
using UnityEngine;

// �Ϲ� ��ȣ�ۿ� ������ ����
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
    [TextArea] public string description; // TextArea�� ���� �� �Է� ����
    public InteractionType type;
}