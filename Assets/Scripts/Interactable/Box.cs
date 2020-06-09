using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Interactable
{
    List<Item> loot;
    bool isClosed;
    private LootManager lootManager;

    public string key;

    protected override void Start()
    {
        base.Start();
        isClosed = false;
        lootManager = GameObject.FindGameObjectWithTag("LootManager").GetComponent<LootManager>();
        loot = lootManager.GenerateLoot("Box");
    }

    protected virtual void Awake()
    {
        
    }

    public override void Interact(GameObject interactee)
    {
        base.Interact(interactee);
        if (isClosed)
        {
            PlayerInventory playerInventory = interactee.GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                if (playerInventory.GetItem(key) != null)
                    lootManager.SetLoot(loot, interactee);
                else
                    Debug.Log("I can't open it");
            }
            else
            {
                Debug.Log("No Player Inventory in this object: Box, Interact: " + interactee.name);
            }
        }
        else
        {
            lootManager.SetLoot(loot, interactee);
        }
    }
}
