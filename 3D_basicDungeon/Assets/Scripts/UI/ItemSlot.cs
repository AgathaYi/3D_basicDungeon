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

    [Header("Inspector 드래그 X")]
    public Inventory inventory;
    public int slotIndex;
    public ItemData data; // 슬롯에 들어갈 데이터
    public int quantity; // 수량
    public bool isEquipped; // 장착 여부

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
        icon.gameObject.SetActive(true); // 아이콘 활성화
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty; // 수량이 1보다 크면 표시
    }

    public void AddQuantity(int delta)
    {
        quantity = Mathf.Max(0, quantity + delta); // 수량 업데이트
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
        quantityText.text = string.Empty; // 수량 텍스트 초기화
    }

    private void OnClick()
    {
        inventory.SelectItem(slotIndex); // 슬롯 선택
    }
}
