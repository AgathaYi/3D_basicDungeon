using System;
using UnityEngine;

// ������ ������ ����
public enum ItemType
{
    Equipable,
    Consumable,
    Resource,
}

// �Һ� ������ Ÿ��
public enum ConsumableType
{
    Caffeine,
    Hunger,
    Power,
}

//�Һ� ������ Ŭ���� ����ȭ
[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "ItemDatas", menuName = "New Item")]

// SciriptableObject ���
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;

}
