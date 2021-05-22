using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Store : MonoBehaviour
{
    [SerializeField] private GameObject selected;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StoreManager.instance.store.SetBool("Open", true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selected);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        StoreManager.instance.store.SetBool("Open", false);
        StoreManager.instance.willYouBuy.SetBool("Open", false);
        EventSystem.current.SetSelectedGameObject(null);
    }
}

