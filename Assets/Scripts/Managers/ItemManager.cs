using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    public string[] itemNames;

    public GameObject[] itemPrefabs;

    public GameObject GetItem(string itemName)
    {
        for (int i = 0; i < itemNames.Length; i++)
        {
            if (itemName == itemNames[i])
            {
                return Instantiate(itemPrefabs[i]);
            }
        }
        Debug.Log("There is no item prefab: " + itemName);
        return null;

    }

    public GameObject GetItem(string itemName, int itemNumber)
    {
        GameObject itemObject = GetItem(itemName);
        if (itemObject != null)
        {
            itemObject.GetComponent<Item>().stackCount = itemNumber;
            return itemObject;
        }

        return null;
    }
}
