using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{

    public List<CraftItem> needItems;
    public string craftItem;
    public int itemNumber;

    public Recipe(List<CraftItem> needItems, string craftItem, int itemNumber = 1)
    {
        this.needItems = needItems;
        this.craftItem = craftItem;
        this.itemNumber = itemNumber;
    }

    public Item GetCraftedItem()
    {
        ItemManager itemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
        return itemManager.GetItem(craftItem, itemNumber).GetComponent<Item>();
    }
}
