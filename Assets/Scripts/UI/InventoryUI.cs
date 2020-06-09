using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : UIWindow
{

    public GameObject ItemSlotSample;
    private GameObject bag;
    private GameObject weapons;
    private GameObject info;

    private Inventory inventory;
    private int dragItemIndex;

    // Start is called before the first frame update
    void Awake()
    {
        bag = transform.Find("Bag").Find("Viewport").Find("Content").gameObject;
        weapons = transform.Find("Weapons").gameObject;
        info = transform.Find("Info").gameObject;
    }

    protected override void Update()
    {
        base.Update();
        inventory.CheckEndedItems();
    }

    public void SetWeaponsSlot(Weapon[] weapons, int activeWeapon)
    {
        foreach(Transform child in this.weapons.transform)
        {
            Destroy(child.gameObject);
        }

        float itemSlotSize = ItemSlotSample.GetComponent<RectTransform>().rect.width;
        for (int i = 0; i < weapons.Length; i++)
        {
            RectTransform weaponSlotRect = Instantiate(ItemSlotSample, this.weapons.transform).GetComponent<RectTransform>();
            weaponSlotRect.gameObject.SetActive(true);
            weaponSlotRect.anchoredPosition = new Vector2(((itemSlotSize / 2) + itemSlotSize) * i + itemSlotSize,
                                                            ((itemSlotSize / 2) + itemSlotSize));
            ItemButtonUI butt = weaponSlotRect.gameObject.GetComponent<ItemButtonUI>();

            if (weapons[i] != null)
            {
                weaponSlotRect.gameObject.GetComponent<Image>().sprite = Instantiate(weapons[i].icon);

                butt.onRightMouseClick += onWeaponRightMouseClick;
            }
        }
    }
    public void SetItems(List<Item> items)
    {
        foreach (Transform child in bag.transform)
        {
            Destroy(child.gameObject);
        }

        RectTransform bagRectTransform = bag.GetComponent<RectTransform>();
        bagRectTransform.sizeDelta = new Vector2(bagRectTransform.rect.width, items.Count * 75);
        bagRectTransform.anchoredPosition = new Vector2(0, -items.Count * 25);
        float itemSlotSize = ItemSlotSample.GetComponent<RectTransform>().rect.width;
        for (int i = 0; i < items.Count; i++)
        {
                RectTransform ItemSlotRect = Instantiate(ItemSlotSample, bag.transform).GetComponent<RectTransform>();
                
                ItemSlotRect.gameObject.SetActive(true);
               
                ItemSlotRect.anchoredPosition = new Vector2((itemSlotSize / 2) + itemSlotSize,
                                                            (((itemSlotSize / 2) + itemSlotSize) * i) + itemSlotSize/2);

                Item curItem = inventory.GetItem(i);

                if (curItem != null)
                {
                    ItemContainer itemContainer = ItemSlotRect.gameObject.GetComponent<ItemContainer>();
                    itemContainer.item = curItem;
                    ItemButtonUI butt = ItemSlotRect.gameObject.GetComponent<ItemButtonUI>();

                    butt.onLeftMouseClick += onLeftMouseClick;
                    butt.onRightMouseClick += onRightMouseClick;
                    butt.onDoubleLeftMouseClick += onDoubleMouseClick;

                    Image icon = ItemSlotRect.gameObject.GetComponent<Image>();
                    if (curItem.stackCount > 1)
                    {
                        Text stackCount = ItemSlotRect.gameObject.transform.Find("StackCount").gameObject.GetComponent<Text>();
                        stackCount.text = curItem.stackCount.ToString();
                    }
                    icon.sprite = curItem.icon;
                }
        }
    }

    public void RefreshItems()
    {
        SetItems(inventory.items);
        inventory.isItemReset = false;
    }
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        SetWeaponsSlot(inventory.GetWeapons(), 0);
        SetItems(inventory.items);
    }

    private void onDoubleMouseClick(object sender, PointerEventData e)
    {
        GameObject itemSlot = e.pointerCurrentRaycast.gameObject;
        if (itemSlot != null)
        {
            ItemContainer itemContainer = itemSlot.GetComponent<ItemContainer>();
            if (itemContainer != null)
            {
                Debug.Log("item name: onRightMouseClick, CraftUI: " + itemContainer.item.name);
                Item item = itemContainer.item;
                UseItem(item);
            }
        }
    }
    private void onLeftMouseClick(object sender, PointerEventData e)
    {
        GameObject itemSlot = e.pointerCurrentRaycast.gameObject;
        if (itemSlot != null)
        {
            ItemContainer itemContainer = itemSlot.GetComponent<ItemContainer>();
            if (itemContainer != null)
            {
                Debug.Log("item name: onRightMouseClick, CraftUI: " + itemContainer.item.name);
                Item item = itemContainer.item;
                ShowInfo(item);
            }
        }
    }

    private void onWeaponRightMouseClick(object sender, PointerEventData e)
    {
        Debug.Log("Left Mouse Click");
        GameObject obj = e.pointerCurrentRaycast.gameObject;
        if (obj != null)
        {
            Vector2 pos = obj.GetComponent<RectTransform>().anchoredPosition;
            Debug.Log("Position: InventoryUI, onLeftMouseClick: " + pos);
            Debug.Log("Object: InventoryUI, onLeftMouseClick: " + obj.name);
            int itemIndex = GetIndexByAnchoredPosition(pos, obj.transform.parent.gameObject);
            Unequip(itemIndex);
        }
    }

    private void onRightMouseClick(object sender, PointerEventData e)
    {
        Debug.Log("Right Mouse Click");

    }

    private void ShowInfo(Item item)
    {
        Text info = this.info.transform.Find("Info").gameObject.GetComponent<Text>();
        Image icon = this.info.transform.Find("Icon").gameObject.GetComponent<Image>();
        if (item == null)
        {
            info.text = "";
            icon.sprite = null;
        }
        else
        {
            info.text = item.GetInfo();
            icon.sprite = item.icon;
        }

    }

    private void UseItem(Item item)
    {
        inventory.UseItem(item);
    }

    private void Unequip(int itemIndex)
    {
        inventory.UnequipWeaponToInventory(itemIndex);
    }
    private void EquipWeapon(Weapon item)
    {
        inventory.EquipWeapon(item);
    }

    private int GetIndexByAnchoredPosition(Vector2 position, GameObject parent)
    {
        if (parent.name == bag.name)
        {
            Debug.Log("GetIndexByAnchoredPosition: " + GetSlotIndexByAnchoredPosition(position));
            return GetSlotIndexByAnchoredPosition(position);
        }
        if (parent.name == weapons.name)
        {
           return GetWeaponIndexByAnchoredPosition(position);
        }
        Debug.Log("There is no such parent: InventoryUI, GetIndexByAnchoredPosition: " + parent.name);
        return -1;
    }

    private int GetSlotIndexByAnchoredPosition(Vector2 position)
    {
        return ((int)(position.y / 75));
    }

    private int GetWeaponIndexByAnchoredPosition(Vector2 position)
    {
        return (int)(position.x / 75);
    }

    public void SetOnRightClick(EventHandler<PointerEventData> eventHandler)
    {
        foreach (Transform item in bag.transform)
        {
            ItemButtonUI butt = item.gameObject.GetComponent<ItemButtonUI>();
            butt.onRightMouseClick -= onRightMouseClick;
            butt.onRightMouseClick += eventHandler;
        }
    }

}
