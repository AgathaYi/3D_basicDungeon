using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [Header("UI")]
    public Image icon;
    public TextMeshProUGUI quantityText;
    public Button button;

    [Header("Inspector �巡�� X")]
    public Inventory inventory;
    public int slotIndex;
    public ItemData data; // ���Կ� �� ������
    public int quantity; // ����
    public bool isEquipped; // ���� ����

    private void Awake()
    {
        button.onClick.AddListener(OnClick);
        Clear();
    }

    public void SetData(ItemData itemData, int amount)
    {
        data = itemData;
        quantity = amount;
        icon.sprite = data.icon;
        icon.gameObject.SetActive(true); // ������ Ȱ��ȭ
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty; // ������ 1���� ũ�� ǥ��
    }

    public void AddQuantity(int delta)
    {
        quantity = Mathf.Max(0, quantity + delta); // ���� ������Ʈ
        if (quantity == 0)
        {
            Clear();
        }
        else
        {
            quantityText.text = quantity.ToString();
        }
    }

    public void Clear()
    {
        data = null;
        quantity = 0;
        isEquipped = false;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty; // ���� �ؽ�Ʈ �ʱ�ȭ
    }

    private void OnClick()
    {
        inventory.SelectItem(slotIndex); // ���� ����
    }
}
