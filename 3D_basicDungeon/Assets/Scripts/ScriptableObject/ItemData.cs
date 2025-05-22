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
    public string displayName; // ������ �̸�
    public string description; // ������ ����
    public ItemType type; // ������ Ÿ��
    public Sprite icon; // ������ ������
    public GameObject dropPrefab; // ������ ��� ������

    [Header("Stacking")]
    public bool canStack; // ���� ���� ����
    public int maxStackAmount; // �ִ� ���� ��
    
    [Header("Equip")]
    public GameObject equipPrefab;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables; // �Һ� ������ ����
}
