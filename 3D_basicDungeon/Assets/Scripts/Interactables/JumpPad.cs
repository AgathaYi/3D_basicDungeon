using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour, IInteractable
{
    public InteractionData data; // 상호작용 데이터

    public float jumpForce = 15f; // 점프 힘

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

    // 점프패드 진입시 - 플레이어 컨트롤러에서 설정
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>()?.EnterJumpPad(this);
        }
    }

    // 점프패드 이탈시 - 플레이어 컨트롤러에서 설정
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>()?.ExitJumpPad(this);
        }
    }
}
