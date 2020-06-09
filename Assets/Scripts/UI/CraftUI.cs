using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftUI : UIWindow
{
    // Start is called before the first frame update

    [SerializeField]
    private GameObject ItemSlot;

    private GameObject itemsToCraft;
    private GameObject craftedItem;
    private GameObject craftedItemInfo;
    private GameObject craftButton;

    private List<Item> craftItems;
    private List<Item> needItems;
    private PlayerInventory playerInventory;

    private Recipe currentRecipe;

    private bool isCraftReset;
    private GameObject interactee;
    private ProcessMachine currentProcessMachine;
    void Awake()
    {
        craftButton = transform.Find("CraftButton").gameObject;
        itemsToCraft = transform.Find("ItemsToCraft").Find("Viewport").Find("Content").gameObject;
        craftedItem = transform.Find("CreatedItem").gameObject;
        craftedItemInfo = transform.Find("CreatedItem").Find("ItemInfo").Find("Viewport").Find("Content").gameObject;
    }

    protected override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
        craftButton.GetComponent<Button>().onClick.AddListener(Craft);
    }

    protected override void Update()
    {
        base.Update();
        CheckEndedItems();
        if (isCraftReset)
        {
                RefreshCraftItems();
        }

    }

    public override void Open()
    {
        base.Open();
        playerInventory.inventoryUI.Open();
    }

    public override void Close()
    {
        base.Close();
        playerInventory.inventoryUI.Close();
    }
    private void CheckEndedItems()
    {
        for (int i = 0; i < craftItems.Count; i++)
        {

            if (craftItems[i] != null)
            {
                bool isEnded = craftItems[i].CheckEnd();
                if (isEnded)
                {
                    Debug.Log("Hoy, DELETE");
                    DeleteItem(i);
                    isCraftReset = true;
                }
            }
        }
    }

    private void DeleteItem(int index)
    {
        Destroy(craftItems[index].gameObject);
        craftItems.RemoveAt(index);
    }
    public void SetCraftItems()
    {
        foreach (Transform item in itemsToCraft.transform)
            Destroy(item.gameObject);

        RectTransform itemSlotTransform = ItemSlot.GetComponent<RectTransform>();
        RectTransform itemsToCraftTransform = itemsToCraft.GetComponent<RectTransform>();
        
        float slotWidth = itemSlotTransform.rect.width;
        float slotHeight = itemSlotTransform.rect.height;

        Debug.Log("ItemstoCraft Width: " + itemsToCraftTransform.rect.width);
        Debug.Log("itemtocraft Count: " + craftItems.Count);
        itemsToCraftTransform.sizeDelta = new Vector2(itemsToCraftTransform.rect.width, 
                                                    craftItems.Count * (slotHeight + slotHeight/2));
        itemsToCraftTransform.anchoredPosition = new Vector2(0, -craftItems.Count * (slotHeight / 2));

        for (int i = 0; i < craftItems.Count; i++)
        {
            RectTransform newItemSlot = Instantiate(ItemSlot, itemsToCraft.transform).GetComponent<RectTransform>();
            newItemSlot.anchoredPosition = new Vector2(slotWidth / 2 + slotWidth, i * (slotHeight + slotHeight / 2) + slotHeight / 2);
            ItemContainer itemContainer = newItemSlot.gameObject.GetComponent<ItemContainer>();
            itemContainer.item = craftItems[i];

            newItemSlot.gameObject.SetActive(true);

            ItemButtonUI butt = newItemSlot.gameObject.GetComponent<ItemButtonUI>();
            
            if (butt == null)
            {
                Debug.Log("No ItemButtonUI in ItemSlot Prefab: CraftUI, SetCraftItems");
                break;
            }

            Item curItem = craftItems[i];
            Image icon = newItemSlot.gameObject.GetComponent<Image>();

            icon.sprite = curItem.icon;

            butt.onRightMouseClick += onRightMouseClick;
        }
    }

    private void RefreshCraftItems()
    {
        SetCraftItems();
        if (playerInventory != null)
        { 
            playerInventory.inventoryUI.RefreshItems();
            playerInventory.inventoryUI.SetOnRightClick(OnInventoryRightMouseClick);
        }
        CheckCraft();
        isCraftReset = false;
    }

    private void onRightMouseClick(object sender, PointerEventData e)
    {
        if (isCraftReset)
            return;
        Debug.Log("OnRightMouseClick: CraftUI");
        GameObject itemSlot = e.pointerCurrentRaycast.gameObject;
        if (itemSlot != null)
        {
            ItemContainer itemContainer = itemSlot.GetComponent<ItemContainer>();
            if (itemContainer != null)
            {
                Debug.Log("item name: onRightMouseClick, CraftUI: " + itemContainer.item.name);
                Item item = itemContainer.item;
                ReleaseItemToInventory(item);
            }
        }
        //ReleaseItemToInventory(null);
    }

    private void OnInventoryRightMouseClick(object sender, PointerEventData e)
    {
        if (isCraftReset)
            return;
        Debug.Log("OnInventoryRightMouseClick: CraftUI");
        GameObject itemSlot = e.pointerCurrentRaycast.gameObject;
        if (itemSlot != null)
        {
            ItemContainer itemContainer = itemSlot.GetComponent<ItemContainer>();
            if (itemContainer != null)
            {
                Debug.Log("item name: onInventoryRightMouseClick, CraftUI: " + itemContainer.item.name);
                Item item = itemContainer.item;
                MoveInventoryItemToCraft(item);
            }
        }

    }

    public void ReleaseItemToInventory(Item item)
    {
        Inventory inventory = playerInventory.GetInventory();
        craftItems.Remove(item);
        inventory.AddItem(item);
        isCraftReset = true;
    }

    private void MoveInventoryItemToCraft(Item item)
    {
        Inventory inventory = playerInventory.GetInventory();
        item.stackCount--;
        ItemManager itemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
        AddCraftItem(itemManager.GetItem(item.itemName, 1).GetComponent<Item>());
        isCraftReset = true;
    }
    
    public void SetProcessMachine(ProcessMachine processMachine)
    {
        currentProcessMachine = processMachine;
        craftItems = processMachine.craftItems;
    }
    public void SetInteractee(GameObject interactee)
    {
        if (interactee.tag != "Player")
        {
            Debug.Log("What the hell, you are not the Player!!!: CraftUI, SetInteractee");
            return;
        }

        playerInventory = interactee.GetComponent<PlayerInventory>();
        playerInventory.inventoryUI.SetOnRightClick(OnInventoryRightMouseClick);
    }

    public void CheckCraft()
    {
        CraftManager craftManager = GameObject.FindGameObjectWithTag("CraftManager").GetComponent<CraftManager>();
        foreach(Recipe recipe in playerInventory.knownRecipes)
        {
            Item item = craftManager.GetCraftItem(craftItems, recipe);
            if (item != null)
            {
                craftedItem.GetComponent<ItemContainer>().item = item;
                craftedItem.transform.Find("ItemIcon").gameObject.GetComponent<Image>().sprite = item.icon;
                craftedItemInfo.GetComponent<Text>().text = item.GetInfo();
                currentRecipe = recipe;
                return;
            }
            
        }
        craftedItem.GetComponent<ItemContainer>().item = null;
        craftedItem.transform.Find("ItemIcon").GetComponent<Image>().sprite = null;
        craftedItemInfo.GetComponent<Text>().text = "";
    }

    public void Craft()
    {
        Item item = craftedItem.GetComponent<ItemContainer>().item;
        if (item != null)
        {
            RemoveRecipeItems();
            playerInventory.GetInventory().AddItem(item);
            isCraftReset = true;
            return;
        }
        Debug.Log("Nothing to Craft: Craft, CraftUI");
    }

    private void RemoveRecipeItems()
    {
        foreach (CraftItem craftItem in currentRecipe.needItems)
        {
            foreach (Item item in craftItems)
            {
                if (craftItem.itemName == item.itemName)
                    item.stackCount -= craftItem.itemNumber;
            }
        }
    }
    public void AddCraftItem(Item item)
    {
        foreach (Item craftItem in craftItems)
        {
            if (craftItem.itemName == item.itemName && !craftItem.isFull())
            {
                int newStackCount = craftItem.stackCount + item.stackCount;
                if (newStackCount > craftItem.maxStack)
                {
                    craftItem.stackCount = craftItem.maxStack;
                    newStackCount -= craftItem.maxStack;
                    item.stackCount = newStackCount;
                    AddCraftItem(item);
                    isCraftReset = true;
                    return;
                }
                else
                {
                    craftItem.stackCount += item.stackCount;
                    isCraftReset = true;
                    GameObject.Destroy(item.gameObject);
                    return;
                }
            }
        }
        craftItems.Add(item);
        isCraftReset = true;
    }
}
