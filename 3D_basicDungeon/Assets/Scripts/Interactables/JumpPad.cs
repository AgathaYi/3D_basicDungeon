using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour, IInteractable
{
    public InteractionData data; // 상호작용 데이터

    public float jumpForce; // 점프 힘

    // 상호작용시 표시할 정보
    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    public void OnInteract()
    {
        // 정보만 표시 할 것.
        // 획득 불가한 기물
    }

    // 점프패드 collisionEnter시 플레이어 점프를 JumpPad의 힘으로 점프
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
