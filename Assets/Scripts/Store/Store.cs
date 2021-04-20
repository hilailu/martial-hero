using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        StoreManager.instance.store.SetBool("Open", true);
    }
}

