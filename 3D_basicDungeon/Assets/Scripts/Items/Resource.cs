using UnityEngine;

public class Resource : MonoBehaviour, IInteractable
{
    public InteractionData data;

    public ItemData itemToGive;
    public int quantityPerHit = 1;
    public int capacity;

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

    // 채집시 드랍되는 아이템 생성
    public void Gether(Vector3 hitPoint, Vector3 hitNomal)
    {
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0) break;

            capacity -= 1;
            Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNomal, Vector3.up));
        }

        if (capacity <= 0)
            Destroy(gameObject);
    }
}