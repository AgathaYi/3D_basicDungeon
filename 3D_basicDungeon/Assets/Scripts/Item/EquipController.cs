using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipController : MonoBehaviour
{
    private GameObject curEquip;

    // ¿Â¬¯
    public void Equip(GameObject prefab)
    {
        if (curEquip != null)
            Destroy(curEquip);

        curEquip = Instantiate(prefab, transform);
        curEquip.transform.localPosition = Vector3.zero;
        curEquip.transform.localRotation = Quaternion.identity;
    }

    // ¿Â¬¯ «ÿ¡¶
    public void UnEquip()
    {
        if (curEquip != null)
            Destroy(curEquip);
    }
}
