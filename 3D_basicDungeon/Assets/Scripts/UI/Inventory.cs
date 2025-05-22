using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private PlayerController controller;
    private PlayerCondition condition;

    [Header("Slots")]
    public GameObject inventoryUI; // 인벤토리 전체창
    public Transform slotPanel; //슬롯배치 부모
    public Transform dropPosition; // 버릴 위치
    public ItemSlot[] slots;

    [Header("Selected Item Info")]
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDescriptionText;
    public TextMeshProUGUI statNameText;
    public TextMeshProUGUI statValueText;
    
    private ItemSlot selectedItem; // 선택된 슬롯
    private int selectedItemIndex; // 선택된 슬롯 인덱스
    private int curEquipIndex; // 현재 장착된 슬롯 인덱스

    [Header("Buttons")]
    public Button useButton;
    public Button equipButton;
    public Button unEquipButton;
    public Button dropButton;

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropPosition;

        controller.inventory += ToggleInventoryUI;
        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryUI.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].slotIndex = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }

        ClearSelectionItem();

    }

    private void ClearSelectionItem()
    {
        selectedItem = null;

        // 빈 슬롯 선택 시, 아이템 정보 빈칸으로
        itemNameText.text = string.Empty;
        itemDescriptionText.text = string.Empty;
        statNameText.text = string.Empty;
        statValueText.text = string.Empty;

        // 버튼 비활성화
        useButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(false);
        unEquipButton.gameObject.SetActive(false);
        dropButton.gameObject.SetActive(false);
    }

    private void ToggleInventoryUI()
    {
        if (IsOpen())
        {
            inventoryUI.SetActive(false);
        }
        else
        {
            inventoryUI.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryUI.activeInHierarchy;
    }

    // 아이템 추가시 슬롯 업데이트
    private void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemdata;
        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateSlots();
                CharacterManager.Instance.Player.itemdata = null;
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();
        if (emptySlot != null)
        {
            emptySlot.data = data;
            emptySlot.quantity = 1;
            UpdateSlots();
            CharacterManager.Instance.Player.itemdata = null;
            return;
        }

        ThrowItem(data);
        CharacterManager.Instance.Player.itemdata = null;
    }

    private void UpdateSlots()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].data != null)
            {
                slots[i].SetData(slots[i].data, slots[i].quantity);
            }
            else
            {
                slots[i].Clear();
            }
        }
    }


    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].data == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].data == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int index)
    {
        if (slots[index].data == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        statNameText.text = selectedItem.data.displayName;
        statValueText.text = selectedItem.data.description;

        statNameText.text = string.Empty;
        statValueText.text = string.Empty;

        for (int i = 0; i < selectedItem.data.consumables.Length; i++)
        {
            statNameText.text += selectedItem.data.consumables[i].consumType.ToString() + "\n";
            statValueText.text += selectedItem.data.consumables[i].value.ToString() + "\n";
        }

        useButton.gameObject.SetActive(selectedItem.data.itemType == ItemType.Consumable);
        equipButton.gameObject.SetActive(selectedItem.data.itemType == ItemType.Equipable && !selectedItem.isEquipped);
        unEquipButton.gameObject.SetActive(selectedItem.data.itemType == ItemType.Equipable && selectedItem.isEquipped);
        dropButton.gameObject.SetActive(true);
    }

    public void OnUseButton()
    {
        if (selectedItem.data.itemType == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.data.consumables.Length; i++)
            {
                switch (selectedItem.data.consumables[i].consumType)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.data.consumables[i].value);
                        break;
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.data.consumables[i].value);
                        break;
                }
            }

            RemoveSelectedItem();
        }
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem.data);
        RemoveSelectedItem();
    }

    void RemoveSelectedItem()
    {
        selectedItem.quantity--;

        if (selectedItem.quantity <= 0)
        {
            if (slots[selectedItemIndex].isEquipped)
            {
                UnEquip(selectedItemIndex);
            }

            selectedItem.data = null;
            ClearSelectionItem();
        }

        UpdateSlots();
    }

    public void OnEquipButton()
    {
        if (slots[curEquipIndex].isEquipped)
        {
            UnEquip(curEquipIndex);
        }

        slots[selectedItemIndex].isEquipped = true;
        curEquipIndex = selectedItemIndex;
        CharacterManager.Instance.Player.equip.Equip(selectedItem.data.equipPrefab);
        UpdateSlots();

        SelectItem(selectedItemIndex);
    }

    void UnEquip(int index)
    {
        slots[index].isEquipped = false;
        CharacterManager.Instance.Player.equip.UnEquip();
        UpdateSlots();

        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }

    public bool HasItem(ItemData item, int quantity)
    {
        return false;
    }

}
