using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ��ȣ�ۿ��� �����۵��� �������� �������̽�
public interface IInteractable
{
    string GetInteractPrompt();
    void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    // UI �� ����� ��ȣ�ۿ� ������Ʈ�� ��ȯ
    public string GetInteractPrompt()
    {
        return $"{data.displayName}\n{data.description}";
    }

    // ��ȣ
    public void OnInteract()
    {
        // ȹ�� ������ ���� Player�� ����
        CharacterManager.Instance.Player.itemdata = data;

        // �κ��丮�� �߰�
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}