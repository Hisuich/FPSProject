using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    Weapon,
    Armor,
    Consumable,
    Material,
    Ammo,
    Miscellaneous
}

public class Item : MonoBehaviour
{
    public string itemName;
    public ItemType itemType;
    public Sprite icon;
    public bool isEnded
    {
        get;
        set;
    }

    public int maxStack;

    public int stackCount;
    
    public bool isFull()
    {
        return maxStack == stackCount;
    }

    virtual protected void Update()
    {
        isEnded = CheckEnd();
    }

    virtual public string GetInfo()
    {
        string info = "Name: " + itemName + "\n";
        info += "Type: " + itemType + "\n";
        info += "Stack Count: " + stackCount + "\n";
        return info;
    }

    virtual public void Use(GameObject owner)
    {
    }

    public bool CheckEnd()
    {
        if (stackCount <= 0)
        {
            return true;
        }
        return false;
    }

    virtual public bool Compare(Item item)
    {
        if (itemName != item.itemName ||
            itemType != item.itemType)
            return false;

        return true;
    }
}
