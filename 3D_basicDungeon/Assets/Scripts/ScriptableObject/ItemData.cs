using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Resource,
    Equipable,
    Consumable,
}

public enum  ConsumableType
{
    Hunger,
    Caffeine,
    Power,
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType Type;
    public float value;
}

[CreateAssetMenu(fileName = "ItemData", menuName = "New Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName; // 아이템 이름
    public string description; // 아이템 설명
    public ItemType type; // 아이템 타입
    public Sprite icon; // 아이템 아이콘
    public GameObject dropPrefab; // 아이템 드랍 프리팹

    [Header("Stacking")]
    public bool canStack; // 스택 가능 여부
    public int maxStackAmount; // 최대 스택 수
    
    [Header("Equip")]
    public GameObject equipPrefab;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables; // 소비 아이템 정보
}
