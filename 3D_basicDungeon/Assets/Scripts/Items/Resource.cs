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
        // ������ ǥ�� �� ��.
        // ȹ�� �Ұ��� �⹰
    }

    // ä���� ����Ǵ� ������ ����
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