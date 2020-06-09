using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Start is called before the first frame update

    public InventoryUI inventoryUI;
    private Inventory inventory;
    public GameObject testObject;
    public List<Recipe> knownRecipes;

    public int currentWeaponIndex;
    public bool isInventoryActive;


    void Awake()
    {
        WeaponSlot[] weapons = GetComponentsInChildren<WeaponSlot>();
        inventory = new Inventory(gameObject, weapons);
    }

    void Start()
    {
        ItemManager itemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
        inventory.AddItem(itemManager.GetItem("Spear").GetComponent<Item>());
        inventory.AddItem(itemManager.GetItem("LightAmmo").GetComponent<Item>(), 50);
        inventoryUI.SetInventory(inventory);
        currentWeaponIndex = 0;
        inventoryUI.gameObject.SetActive(false);

        knownRecipes = new List<Recipe>();
        List<CraftItem> craftItems = new List<CraftItem>();
        CraftItem craftItem = new CraftItem();
        craftItem.itemName = "Ferum";
        craftItem.itemNumber = 1;
        craftItems.Add(craftItem);
        Recipe bulletRecipe = new Recipe(craftItems, "LightAmmo", 5);
        knownRecipes.Add(bulletRecipe);

    }

    public Item GetItem(string itemName)
    {
        for (int i = 0; i < inventory.items.Count; i++)
        {
            if (inventory.items[i] != null && inventory.items[i].name == itemName)
                return inventory.items[i];
        }

        Debug.Log("There is no item with this name: PlayerInventory, GetItem: " + itemName);
        return null;
    }
    // Update is called once per frame
    void Update()
    {
        WeaponManage();
        if(inventory.isItemReset)
        {
            inventoryUI.SetItems(inventory.items);
            inventory.isItemReset = false;
        }
        Debug.Log("Weapon Reset: " + inventory.isWeaponReset);
        if(inventory.isWeaponReset)
        {
            inventoryUI.SetWeaponsSlot(inventory.GetWeapons(), currentWeaponIndex);
            WeaponReset();
            inventory.isWeaponReset = false;
        }

        if (isInventoryActive && Input.GetButtonDown("Inventory"))
        {
            Close();
        }

        if (GetCurrentWeapon() != null)
        {
            if (GetCurrentWeapon().needReload)
            {
                GetCurrentWeapon().ammo = FindAmmo(GetCurrentWeapon().ammoType);
            }
            if (GetCurrentWeapon().reloading)
            {
                inventory.isItemReset = true;
                GetCurrentWeapon().reloading = false;
            }

        }
        inventory.CheckEndedItems();
    }

    public void Open()
    {
        GlobalVar.isUIOpen = true;
        inventoryUI.gameObject.SetActive(true);
    }
    public void Close()
    {
        GlobalVar.isUIOpen = false;
        inventoryUI.gameObject.SetActive(false);
    }

    private void WeaponManage()
    {
        int index = currentWeaponIndex;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            index = 0;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            index = 1;
        }
        else if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            index = 2;
        }

        if (inventory.weapons[index].weapon != null && index != currentWeaponIndex)
        {
            currentWeaponIndex = index;
            SetCurrentWeapon(index);
        }

        inventory.weapons[currentWeaponIndex].gameObject.SetActive(true);
    }

    public Weapon GetCurrentWeapon()
    {
        return inventory.weapons[currentWeaponIndex].weapon;
    }

    private void SetCurrentWeapon(int index)
    {
        for (int i = 0; i < inventory.weapons.Length; i++)
        {    
            inventory.weapons[i].gameObject.SetActive(false);
        }
    }

    public void TakeItem(Item item)
    {
        ItemManager itemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
        inventory.AddItem(itemManager.GetItem(item.itemName).GetComponent<Item>());
        Destroy(item.gameObject);
    }

    private void WeaponReset()
    {
        foreach (WeaponSlot weaponSlot in inventory.weapons)
        {
            weaponSlot.ResetInfo();
        }
        SetCurrentWeapon(currentWeaponIndex);
    }

    public Ammo FindAmmo(AmmoType ammoType)
    {
        List<Item> items = inventory.items;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemType == ItemType.Ammo)
            {
                if (((Ammo)items[i]).ammoType == ammoType)
                {
                    return (Ammo)items[i];
                }
            }
        }
        Debug.Log("Ammo Type is not found in inventory: PlayerInventory, FindAmmo: " + ammoType);
        return null;
    }

    public Inventory GetInventory() // ЗА КОСТЫЛИ!!!(Для крафта)
    {
        return inventory;
    }
}
