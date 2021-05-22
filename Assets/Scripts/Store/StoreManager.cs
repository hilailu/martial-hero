using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public static StoreManager instance;

    [SerializeField] private Text descText;
    [SerializeField] private GameObject panel;
    [SerializeField] private PlayerController player;
    public List<Item> items = new List<Item>();
    public StoreItem[] storeItems;
    public Animator willYouBuy;
    public Animator store;
    private string prevClickedButton = "";
    public Item selectedItem;

    private LocalizedString description = new LocalizedString { TableReference = "Store", Arguments = new List<object>() };

    public Action OnStoreChanged;

    void UpdateString(string translatedValue)
    {
        descText.text = translatedValue;
    }

    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
        instance.OnStoreChanged += UpdateUI;
        description.StringChanged += UpdateString;
        description.Arguments.Add(this);
        storeItems = GetComponentsInChildren<StoreItem>(true);
        //UpdateUI();
    }

    private void Start()
    {
        foreach (StoreItem si in storeItems)
        {
            instance.Add(si._item);
        }
        UpdateUI();
    }

    private void OnDestroy()
    {
        instance.OnStoreChanged -= UpdateUI;
        description.StringChanged -= UpdateString;
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
        if (item == null)
            return false;
        if (!items.Contains(item))
            items.Add(item);
        //OnStoreChanged?.Invoke();
        return true;
    }

    public void Remove(Item item)
    {
        items.Remove(item);
        OnStoreChanged?.Invoke();
    }

    public void OnClick()
    {        
        var button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        selectedItem = button.GetComponentInChildren<StoreItem>()._item;
        if (selectedItem == null)
            instance.willYouBuy.SetBool("Open", false);
        else
        {
            if (instance.willYouBuy.GetBool("Open") && selectedItem.itemName == prevClickedButton)
                instance.willYouBuy.SetBool("Open", false);
            else
            {
                instance.willYouBuy.SetBool("Open", true);
                description.TableEntryReference = "text";
                description.GetLocalizedString(description.Arguments);
                description.RefreshString();
            }
            prevClickedButton = selectedItem.itemName;
        }
    }

    public void OnBuy()
    {
        StoreItem storeItem = default;
        storeItems = panel.GetComponentsInChildren<StoreItem>(true);
        foreach (StoreItem t in storeItems)
        {
            if (t._item.itemName == prevClickedButton)
            {
                storeItem = t;
                break;
            }
        }
        if (storeItem._item.cost <= PermanentUI.perm.coins)
        {
            PermanentUI.perm.coins -= storeItem._item.cost;
            PermanentUI.perm.coinsNum.text = PermanentUI.perm.coins.ToString();
            InventoryManager.instance.items.Add(storeItem._item);
            InventoryManager.instance.OnInventoryChanged?.Invoke();
            Remove(storeItem._item);
            instance.willYouBuy.SetBool("Open", false);
        }
    }

    public void OnClose()
    {
        instance.willYouBuy.SetBool("Open", false);
    }

}
