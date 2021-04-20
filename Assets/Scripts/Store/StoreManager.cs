using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager instance;

    public List<Item> items = new List<Item>();
    public StoreItem[] storeItems;
    public Animator willYouBuy;
    public Animator store;

    public Action OnStoreChanged;

    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
        instance.OnStoreChanged += UpdateUI;
        storeItems = GetComponentsInChildren<StoreItem>(true);
        UpdateUI();
    }

    private void Start()
    {
        var item = FindObjectOfType<InteractItem>().item;
        instance.Add(item);
    }

    private void OnDestroy()
    {
        instance.OnStoreChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < storeItems.Length; i++)
        {
            if (i < items.Count)
            {
                storeItems[i].Add(items[i]);
            }
            else
            {
                storeItems[i]?.Remove();
            }
        }
    }

    public bool Add(Item item)
    {
        if (items.Count >= 4)
            return false;
        if (!items.Contains(item))
            items.Add(item);
        OnStoreChanged?.Invoke();
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        OnStoreChanged?.Invoke();
    }
}
