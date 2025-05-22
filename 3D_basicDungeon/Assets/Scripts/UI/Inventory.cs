using TMPro;
using UnityEngine;

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

    private ItemSlot selectedItem;
    private int selectedItemIndex;

    int curEquipIndex; // 현재 장착된 슬롯 인덱스

    [Header("Buttons")]
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    void Start()
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

    void ClearSelectionItem()
    {
        selectedItem = null;

        // 빈 슬롯 선택 시, 아이템 정보 빈칸으로
        itemNameText.text = string.Empty;
        itemDescriptionText.text = string.Empty;
        statNameText.text = string.Empty;
        statValueText.text = string.Empty;

        // 버튼 비활성화
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void ToggleInventoryUI()
    {
        if (IsOpen())
            inventoryUI.SetActive(false);

        else
            inventoryUI.SetActive(true);
    }

    public bool IsOpen()
    {
        return inventoryUI.activeInHierarchy;
    }

    // 아이템 추가시 슬롯 업데이트
    void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemdata;

        // 아이템 중복 체크
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

        // 빈슬롯 가져오기
        ItemSlot emptySlot = GetEmptySlot();

        // 빈슬롯이 있다면 슬롯에 아이템 추가
        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateSlots();
            CharacterManager.Instance.Player.itemdata = null;
            return;
        }

        // 슬롯이 없다면 아이템 버리기
        ThrowItem(data);
        CharacterManager.Instance.Player.itemdata = null;
    }

    void UpdateSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
                slots[i].SetData();

            else
                slots[i].Clear();
        }
    }

    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
                return slots[i];
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
                return slots[i];
        }
        return null;
    }

    void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        itemNameText.text = selectedItem.item.displayName;
        itemDescriptionText.text = selectedItem.item.description;

        statNameText.text = string.Empty;
        statValueText.text = string.Empty;

        for (int i = 0; i < selectedItem.item.consumables.Length; i++)
        {
            statNameText.text += selectedItem.item.consumables[i].Type.ToString() + "\n";
            statValueText.text += selectedItem.item.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !slots[index].isEquipped);
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && slots[index].isEquipped);
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if (selectedItem.item.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.item.consumables.Length; i++)
            {
                switch (selectedItem.item.consumables[i].Type)
                {
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.item.consumables[i].value);
                        break;
                    case ConsumableType.Caffeine:
                        condition.Caffeine(selectedItem.item.consumables[i].value);
                        break;
                    case ConsumableType.Power:
                        condition.Drink(selectedItem.item.consumables[i].value);
                        break;
                }
            }

            RemoveSelectedItem();
        }
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);
        RemoveSelectedItem();
    }

    void RemoveSelectedItem()
    {
        selectedItem.quantity--;

        if (selectedItem.quantity <= 0)
        {
            if (selectedItem.isEquipped)
                UnEquip(selectedItemIndex);

            selectedItem = null;
            ClearSelectionItem();
        }

        UpdateSlots();
    }

    public void OnEquipButton()
    {
        if (slots[curEquipIndex].isEquipped)
            UnEquip(curEquipIndex);

        slots[selectedItemIndex].isEquipped = true;
        curEquipIndex = selectedItemIndex;
        CharacterManager.Instance.Player.equip.Equip(selectedItem.item);
        UpdateSlots();

        SelectItem(selectedItemIndex);
    }

    void UnEquip(int index)
    {
        slots[index].isEquipped = false;
        CharacterManager.Instance.Player.equip.UnEquip();
        UpdateSlots();

        if (selectedItemIndex == index)
            SelectItem(selectedItemIndex);
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
