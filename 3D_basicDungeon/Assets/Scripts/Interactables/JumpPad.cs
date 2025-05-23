using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour, IInteractable
{
    public InteractionData data; // ��ȣ�ۿ� ������

    public float jumpForce = 15f; // ���� ��

    // ��ȣ�ۿ�� ǥ���� ����
    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        // ������ ǥ�� �� ��.
        // ȹ�� �Ұ��� �⹰
    }

    // �����е� ���Խ� - �÷��̾� ��Ʈ�ѷ����� ����
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>()?.EnterJumpPad(this);
        }
    }

    // �����е� ��Ż�� - �÷��̾� ��Ʈ�ѷ����� ����
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>()?.ExitJumpPad(this);
        }
    }
}
