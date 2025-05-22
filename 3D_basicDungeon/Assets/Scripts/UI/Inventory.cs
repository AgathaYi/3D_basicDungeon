using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Slots")]
    public GameObject inventoryUI; // �κ��丮 ��üâ
    public Transform slotPanel; //���Թ�ġ �θ�
    public GameObject slotPrefab; // ���� 1��

    [Header("ItemInfo")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI statNameText;
    public TextMeshProUGUI statValueText;

    [Header("Buttons")]
    public Button useButton;
    public Button equipButton;
    public Button unEquipButton;
    public Button dropButton;

    private List<ItemSlot> slots = new List<ItemSlot>();
    private ItemSlot selectedSlot; // ���õ� ����
    private Player player => CharacterManager.Instance.Player;

    private void Awake()
    {
        // ���� ����
        foreach (Transform child in slotPanel) Destroy(child.gameObject);
        for (int i = 0; i < 12; i++)
        {
            var slotObj = Instantiate(slotPrefab, slotPanel);
            var slot = slotObj.GetComponent<ItemSlot>();
            slot.slotIndex = i;
            slot.inventory = this;
            slots.Add(slot);
        }

        // ��ư �̺�Ʈ
        useButton.onClick.AddListener(OnUse);
        equipButton.onClick.AddListener(OnEquip);
        unEquipButton.onClick.AddListener(OnUnEquip);
        dropButton.onClick.AddListener(OnDrop);

        // �κ��丮 UI ��Ȱ��ȭ
        inventoryUI.SetActive(false);
        ClearInfo();
    }

    private void Start()
    {
        player.controller.inventory += ToggleInventoryUI; // �κ��丮 ����
        player.addItem += UpdateSlots; // ������ �߰�
    }

    private void ToggleInventoryUI()
    {
        bool open = !inventoryUI.activeSelf;
        inventoryUI.SetActive(open);

        // ������, look ��� �� Ŀ�� Ȱ��ȭ
        player.controller.canLook = !open;
        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void UpdateSlots()
    {
        // ����� ������ �� ���Կ� �߰�
        var data = player.itemdata;
        if (data == null) return;

        // �߰� ���� ���� Ȯ��(stack ���� Ȯ��)
        if (data.canStack)
        {
            var exist = slots.Find(s => s.data == data && s.quantity < data.maxStackAmount);
            if (exist != null)
            {
                exist.AddQuantity(1); // ���� ����
                ClearSelection();
                return;
            }
        }

        // �� ���� ã��
        var empty = slots.Find(s => s.data == null);
        if (empty != null)
        {
            empty.SetData(data, 1); // ���Կ� ������ �߰�
            ClearSelection();
            return;
        }

        // ������ ���� á�� ��, ���
        Instantiate(data.dropPrefab, player.dropPosition.position, Quaternion.identity);
        player.itemdata = null; // �÷��̾� itemData �ʱ�ȭ

    }

    private void ClearSelection()
    {
        selectedSlot = null;
        ClearInfo();
    }

    private void ClearInfo()
    {
        // �� ���� ���� ��, ������ ���� ��ĭ����
        // ��ư ��Ȱ��ȭ
    }

    private void OnUse()
    {
        if (selectedSlot == null) return;
        if (selectedSlot.data == null) return;
        // ���
        player.itemdata = selectedSlot.data;
        selectedSlot.AddQuantity(-1); // ���� ���� ����
        if (selectedSlot.quantity <= 0) selectedSlot.Clear(); // ���� ����
    }

    private void OnEquip()
    {
        if (selectedSlot == null) return;
        if (selectedSlot.data == null) return;
        // ����
        player.itemdata = selectedSlot.data;
        selectedSlot.isEquipped = true; // ���� ���� ���� ����
        selectedSlot.AddQuantity(-1); // ���� ���� ����
        if (selectedSlot.quantity <= 0) selectedSlot.Clear(); // ���� ����
    }

    private void OnUnEquip()
    {
        if (selectedSlot == null) return;
        if (selectedSlot.data == null) return;
        // ���� ����
        player.itemdata = selectedSlot.data;
        selectedSlot.isEquipped = false; // ���� ���� ���� ���� ����
        selectedSlot.AddQuantity(1); // ���� ���� ����
    }

    private void OnDrop()
    {
        if (selectedSlot == null) return;
        if (selectedSlot.data == null) return;
        // ���
        Instantiate(selectedSlot.data.dropPrefab, player.dropPosition.position, Quaternion.identity);
        selectedSlot.AddQuantity(-1); // ���� ���� ����
        if (selectedSlot.quantity <= 0) selectedSlot.Clear(); // ���� ����
    }
}
