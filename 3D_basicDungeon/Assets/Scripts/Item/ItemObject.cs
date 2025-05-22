using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 상호작용할 아이템들을 구현해줄 인터페이스
public interface IInteractable
{
    string GetInteractPrompt();
    void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    // UI 상에 출력할 상호작용 프롬프트를 반환
    public string GetInteractPrompt()
    {
        return $"{data.displayName}\n{data.description}";
    }

    // 상호
    public void OnInteract()
    {
        // 획득 아이템 정보 Player에 저장
        CharacterManager.Instance.Player.itemdata = data;

        // 인벤토리에 추가
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}