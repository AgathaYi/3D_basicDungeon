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
    private Outline outline;

    [Header("스크립트에서 처리")]
    public Inventory inventory;
    public int slotIndex;
    public ItemData item; // 슬롯에 들어갈 데이터
    public int quantity; // 수량
    public bool isEquipped; // 장착 여부

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    private void OnEnable()
    {
        outline.enabled = isEquipped;
    }

    public void SetData()
    {
        icon.gameObject.SetActive(true); // 아이콘 활성화
        icon.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty; // 수량이 1보다 크면 표시
        
        if (outline != null)
            outline.enabled = isEquipped;
    }

    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty; // 수량 텍스트 초기화
    }

    public void OnClickButton()
    {
        inventory.SelectItem(slotIndex);
    }
}
