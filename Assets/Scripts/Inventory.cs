using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public List<Item> items;
    public WeaponSlot[] weapons;
    public GameObject inventoryPool;
    public GameObject owner;

    public bool isItemReset;
    public bool isWeaponReset;

    public Inventory(GameObject owner, WeaponSlot[] weapons)
    {
        items = new List<Item>();
        inventoryPool = owner.transform.Find("Inventory").gameObject;
        if (inventoryPool == null)
        {
            inventoryPool = new GameObject("Inventory");
            inventoryPool.transform.parent = owner.transform;
        }

        Item[] poolItems = inventoryPool.GetComponentsInChildren<Item>();

        for (int i = 0; i < items.Count && i < poolItems.Length; i++)
        {
            items.Add(poolItems[i]);
        }
        this.owner = owner;
        this.weapons = weapons;
    }

    public bool isFull()
    {
        return false;
    }

    public void CheckEndedItems() 
    {
        for (int i = 0; i < items.Count; i++)
        {
            
            if (items[i] != null)
            {
                bool isEnded = items[i].CheckEnd();
                if (isEnded)
                {
                    Debug.Log("Hoy, DELETE");
                    DeleteItem(i);
                    isItemReset = true;
                }
            }
        }
    }

    public void UseItem(Item item)
    {
        if (item.itemType == ItemType.Weapon)
            EquipWeapon((Weapon)item);
        else 
        item.Use(owner);
    }

    public bool AddItem(Item item)
    {  
        foreach (Item inventoryItem in items)
        {
            if (inventoryItem.itemName == item.itemName  && !inventoryItem.isFull())
            {
                int newStackCount = inventoryItem.stackCount + item.stackCount;
                if (newStackCount > inventoryItem.maxStack)
                {
                    inventoryItem.stackCount = inventoryItem.maxStack;
                    newStackCount -= inventoryItem.maxStack;
                    item.stackCount = newStackCount;
                    AddItem(item);
                    return true;
                }
                else
                {
                    inventoryItem.stackCount += item.stackCount;
                    isItemReset = true;
                    GameObject.Destroy(item.gameObject);
                    return true;
                }
            }
        }
        items.Add(item);
        isItemReset = true;
        item.gameObject.transform.parent = inventoryPool.transform;
        item.gameObject.SetActive(false);
        return true;
    }

    public bool AddItem(Item item, int stackCount)
    {
        item.stackCount = stackCount;
        return AddItem(item);
    }

    public bool AddWeapon(Weapon weapon)
    {
        foreach (WeaponSlot weaponSlot in weapons)
        {
            if (weaponSlot.weapon == null)
            {
                weaponSlot.SetWeapon(weapon);
                weaponSlot.gameObject.SetActive(false);
                isWeaponReset = true;
                return true;
            }
        }
        Debug.Log("There is no space in weapon slot: Inventory, AddWeapon" + weapon.name);

        return false;


    }

    public Item GetItem(int index)
    {
        return items[index];
    }

    public void DeleteItem(int index)
    {
        GameObject.Destroy(items[index].gameObject);
        items.RemoveAt(index);
    }

    public void DropItem(int itemIndex)
    {
         
    }

    public Weapon[] GetWeapons()
    {
        Weapon[] weaponArray = new Weapon[weapons.Length];
        for (int i = 0; i < weapons.Length; i++)
            weaponArray[i] = weapons[i].weapon;

        return weaponArray;
    }

    public Weapon GetWeapon(int weaponIndex)
    {
        return weapons[weaponIndex].weapon;
    }

    public void EquipWeapon(Weapon weapon)
    {

            if (AddWeapon(weapon))
            {
                items.Remove(weapon);
                isItemReset = true;
                isWeaponReset = true;
            }
    }

    public void UnequipWeaponToInventory(int weaponIndex)
    {

        if (AddItem(weapons[weaponIndex].weapon))
        {
            weapons[weaponIndex].weapon = null;
            isItemReset = true;
            isWeaponReset = true;

        }
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }    
}
