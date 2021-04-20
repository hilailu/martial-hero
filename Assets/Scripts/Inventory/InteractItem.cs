using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractItem : MonoBehaviour
{
    public Item item;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (InventoryManager.instance.Add(item))
            Destroy(this.gameObject);
    }
}
