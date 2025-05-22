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

    [Header("��ũ��Ʈ���� ó��")]
    public Inventory inventory;
    public int slotIndex;
    public ItemData item; // ���Կ� �� ������
    public int quantity; // ����
    public bool isEquipped; // ���� ����

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
        icon.gameObject.SetActive(true); // ������ Ȱ��ȭ
        icon.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty; // ������ 1���� ũ�� ǥ��
        
        if (outline != null)
            outline.enabled = isEquipped;
    }

    public void Clear()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty; // ���� �ؽ�Ʈ �ʱ�ȭ
    }

    public void OnClickButton()
    {
        inventory.SelectItem(slotIndex);
    }
}
