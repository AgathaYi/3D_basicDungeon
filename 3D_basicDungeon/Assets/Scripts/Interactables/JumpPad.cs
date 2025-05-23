using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour, IInteractable
{
    public InteractionData data; // 상호작용 데이터

    public float jumpForce = 20f; // 점프 힘
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

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 플레이어일 때
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플레이어의 Rigidbody 컴포넌트를 가져옴
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                // up 방향으로 점프 힘을 적용
                Vector3 force = transform.up * jumpForce;
                playerRigidbody.AddForce(force, ForceMode.Impulse);
            }
        }
    }
}
