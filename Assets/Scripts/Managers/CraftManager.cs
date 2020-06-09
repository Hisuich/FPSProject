using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CraftItem
{
    public string itemName;
    public int itemNumber;
}
public class CraftManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Item GetCraftItem(List<Item> craftItems, Recipe recipe)
    {
        if (craftItems.Count == 0) return null;
        foreach (CraftItem craftItem in recipe.needItems)
        {
            foreach (Item item in craftItems)
            {
                if (item.itemName != craftItem.itemName ||
                    item.stackCount < craftItem.itemNumber)
                    return null;
            }
        }
        return recipe.GetCraftedItem();
    }
}
