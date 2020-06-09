using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootUI : UIWindow
{
    
    public GameObject itemSlotSample;
    private GameObject loot;

    private GameObject player;

    private List<Item> items;
    // Start is called before the first frame update

    void Awake()
    {
        loot = gameObject.transform.Find("Viewport").Find("Content").gameObject;
    }
    public void SetLoot(List<Item> items, GameObject player)
    {
        if (player.tag != "Player")
        {
            Debug.Log("This object can't loot: LootUI, SetLoot" + player.name);
            return;
        }
        foreach (Transform child in loot.transform)
        {
            Destroy(child.gameObject);
        }

        this.items = items;
        this.player = player;

        BuildLootUI();
    }

    private void RefreshLoot()
    {
        foreach (Transform child in loot.transform)
        {
            Destroy(child.gameObject);
        }

        BuildLootUI();
    }

    private void BuildLootUI()
    {
        foreach (Transform lootItem in loot.transform)
        {
            Destroy(lootItem.gameObject);
        }
        float itemSlotSize = itemSlotSample.GetComponent<RectTransform>().rect.width;
        Vector2 windowSize = loot.GetComponent<RectTransform>().rect.size;
        loot.GetComponent<RectTransform>().sizeDelta = new Vector2(windowSize.x, items.Count * (itemSlotSize + itemSlotSize / 2));        for (int i = 0; i < items.Count; i++)
        loot.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -items.Count * (itemSlotSize / 2));
        for (int i = 0; i < items.Count; i++)
        {
                RectTransform ItemSlotRect = Instantiate(itemSlotSample, loot.transform).GetComponent<RectTransform>();

                ItemSlotRect.gameObject.SetActive(true);
                ItemSlotRect.anchoredPosition = new Vector2(((itemSlotSize / 2) + itemSlotSize),
                                                            windowSize.y - ((itemSlotSize / 2) + itemSlotSize) * i);

                Item curItem = items[i];

                if (curItem != null)
                {
                    ItemButtonUI butt = ItemSlotRect.gameObject.GetComponent<ItemButtonUI>();

                    butt.onLeftMouseClick += onLeftMouseClick;

                    Image icon = ItemSlotRect.gameObject.GetComponent<Image>();
                    icon.sprite = curItem.icon;

                ItemSlotRect.gameObject.GetComponent<ItemContainer>().item = curItem;
                }
        }
    }

    private void onLeftMouseClick(object sender, PointerEventData e)
    {
        GameObject slot = e.pointerCurrentRaycast.gameObject;
        ItemContainer itemContainer = slot.GetComponent<ItemContainer>();
        if (itemContainer != null)
        {
            TakeItem(itemContainer.item);
        }
        
    }

    private void TakeItem(Item item)
    {
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        inventory.TakeItem(item);
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == item)
            {
                items[i] = null;
                RefreshLoot();
            }
        }
    }
    // Update is called once per frame
}
