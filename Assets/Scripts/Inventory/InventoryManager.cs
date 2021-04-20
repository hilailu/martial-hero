using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public List<Item> items = new List<Item>();
    private int storage = 3;
    public InventoryItem[] inventoryItems;

    public Action OnInventoryChanged;

    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
        instance.OnInventoryChanged += UpdateUI;
        inventoryItems = GetComponentsInChildren<InventoryItem>(true);
        UpdateUI();
    }

    private void OnDestroy()
    {
        instance.OnInventoryChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (i < items.Count)
            {
                inventoryItems[i].Add(items[i]);
            }
            else
            {
                inventoryItems[i]?.Remove();
            }
        }
    }

    public bool Add(Item item)
    {
        if (items.Count >= storage)
            return false;
        if (!items.Contains(item))
            items.Add(item);
        OnInventoryChanged?.Invoke();
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        OnInventoryChanged?.Invoke();
    }

}
