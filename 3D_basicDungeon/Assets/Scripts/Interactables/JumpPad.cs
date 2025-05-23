using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour, IInteractable
{
    public InteractionData data; // ��ȣ�ۿ� ������

    public float jumpForce = 20f; // ���� ��
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

    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� ������Ʈ�� �÷��̾��� ��
        if (collision.gameObject.CompareTag("Player"))
        {
            // �÷��̾��� Rigidbody ������Ʈ�� ������
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // up �������� ���� ���� ����
                Vector3 force = transform.up * jumpForce;
                playerRigidbody.AddForce(force, ForceMode.Impulse);
            }
        }
    }
}
