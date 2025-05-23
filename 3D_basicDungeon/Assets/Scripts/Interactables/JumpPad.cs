using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour, IInteractable
{
    public InteractionData data; // ��ȣ�ۿ� ������

    public float jumpForce; // ���� ��

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

    // �����е� collisionEnter�� �÷��̾� ��Ʈ�ѷ����� ����
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
