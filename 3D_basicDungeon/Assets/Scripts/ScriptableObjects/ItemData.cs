using System;
using UnityEngine;

// 아이템 데이터 정의
public enum ItemType
{
    Equipable,
    Consumable,
    Resource,
}

// 소비 아이템 타입
public enum ConsumableType
{
    Caffeine,
    Hunger,
    Power,
}

//소비 아이템 클래스 직렬화
[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/New Data")]
public class ItemData : ScriptableObject // SciriptableObject 상속
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



