using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Slots")]
    public GameObject inventoryUI; // 인벤토리 전체창
    public Transform slotPanel; //슬롯배치 부모
    public GameObject slotPrefab; // 슬롯 1개

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
    private ItemSlot selectedSlot; // 선택된 슬롯
    private Player player => CharacterManager.Instance.Player;

    private void Awake()
    {
        // 슬롯 생성
        foreach (Transform child in slotPanel) Destroy(child.gameObject);
        for (int i = 0; i < 12; i++)
        {
            var slotObj = Instantiate(slotPrefab, slotPanel);
            var slot = slotObj.GetComponent<ItemSlot>();
            slot.slotIndex = i;
            slot.inventory = this;
            slots.Add(slot);
        }

        // 버튼 이벤트
        useButton.onClick.AddListener(OnUse);
        equipButton.onClick.AddListener(OnEquip);
        unEquipButton.onClick.AddListener(OnUnEquip);
        dropButton.onClick.AddListener(OnDrop);

        // 인벤토리 UI 비활성화
        inventoryUI.SetActive(false);
        ClearInfo();
    }

    private void Start()
    {
        player.controller.inventory += ToggleInventoryUI; // 인벤토리 열기
        player.addItem += UpdateSlots; // 아이템 추가
    }

    private void ToggleInventoryUI()
    {
        bool open = !inventoryUI.activeSelf;
        inventoryUI.SetActive(open);

        // 열리면, look 잠금 및 커서 활성화
        player.controller.canLook = !open;
        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void UpdateSlots()
    {
        // 저장된 아이템 빈 슬롯에 추가
        var data = player.itemdata;
        if (data == null) return;

        // 추가 가능 여부 확인(stack 갯수 확인)
        if (data.canStack)
        {
            var exist = slots.Find(s => s.data == data && s.quantity < data.maxStackAmount);
            if (exist != null)
            {
                exist.AddQuantity(1); // 수량 증가
                ClearSelection();
                return;
            }
        }

        // 빈 슬롯 찾기
        var empty = slots.Find(s => s.data == null);
        if (empty != null)
        {
            empty.SetData(data, 1); // 슬롯에 데이터 추가
            ClearSelection();
            return;
        }

        // 슬롯이 가득 찼을 때, 드랍
        Instantiate(data.dropPrefab, player.dropPosition.position, Quaternion.identity);
        player.itemdata = null; // 플레이어 itemData 초기화

    }

    private void ClearSelection()
    {
        selectedSlot = null;
        ClearInfo();
    }

    private void ClearInfo()
    {
        // 빈 슬롯 선택 시, 아이템 정보 빈칸으로
        // 버튼 비활성화
    }

    private void OnUse()
    {
        if (selectedSlot == null) return;
        if (selectedSlot.data == null) return;
        // 사용
        player.itemdata = selectedSlot.data;
        selectedSlot.AddQuantity(-1); // 슬롯 수량 감소
        if (selectedSlot.quantity <= 0) selectedSlot.Clear(); // 슬롯 비우기
    }

    private void OnEquip()
    {
        if (selectedSlot == null) return;
        if (selectedSlot.data == null) return;
        // 장착
        player.itemdata = selectedSlot.data;
        selectedSlot.isEquipped = true; // 슬롯 장착 상태 변경
        selectedSlot.AddQuantity(-1); // 슬롯 수량 감소
        if (selectedSlot.quantity <= 0) selectedSlot.Clear(); // 슬롯 비우기
    }

    private void OnUnEquip()
    {
        if (selectedSlot == null) return;
        if (selectedSlot.data == null) return;
        // 장착 해제
        player.itemdata = selectedSlot.data;
        selectedSlot.isEquipped = false; // 슬롯 장착 해제 상태 변경
        selectedSlot.AddQuantity(1); // 슬롯 수량 증가
    }

    private void OnDrop()
    {
        if (selectedSlot == null) return;
        if (selectedSlot.data == null) return;
        // 드랍
        Instantiate(selectedSlot.data.dropPrefab, player.dropPosition.position, Quaternion.identity);
        selectedSlot.AddQuantity(-1); // 슬롯 수량 감소
        if (selectedSlot.quantity <= 0) selectedSlot.Clear(); // 슬롯 비우기
    }
}
