using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct LootItem
{
    public string itemName;
    public float percentToDrop;
}

struct Loot
{
    public string enemyName;
    public LootItem[] lootItems;
}
public class LootManager : MonoBehaviour
{
    [SerializeField]
    private List<Loot> enemyPossibleLoot;

    public LootUI lootUI;

    // Start is called before the first frame update
    void Awake()
    {
        enemyPossibleLoot = new List<Loot>();
        Loot loot = new Loot();
        loot.enemyName = "Enemy";
        LootItem lootItem = new LootItem();
        lootItem.itemName = "Spear";
        lootItem.percentToDrop = 1.0f;
        loot.lootItems = new LootItem[1];
        loot.lootItems[0] = lootItem;
        Loot boxLoot = new Loot();
        boxLoot.enemyName = "Box";
        LootItem boxItems = new LootItem();
        boxItems.itemName = "RepairPack";
        boxItems.percentToDrop = 1.0f;
        boxLoot.lootItems = new LootItem[2];
        boxLoot.lootItems[0] = boxItems;
        boxLoot.lootItems[1] = lootItem;
        enemyPossibleLoot.Add(boxLoot);
        enemyPossibleLoot.Add(loot);
    }

    void Start()
    {
        lootUI.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("CloseAllUI"))
        {
            Close();
        }
    }

    private void Open()
    {
        lootUI.Open();
    }

    private void Close()
    {
        lootUI.Close();
    }

    public List<Item> GenerateLoot(string enemyName)
    {
        float currentPercent = 1.0f;
        Loot currentLoot = FindEnemy(enemyName);
        if (currentLoot.enemyName == null)
        {
            return null;
        }
        else
        {
            List<Item> loot = new List<Item>();
            ItemManager itemManager = GameObject.FindGameObjectWithTag("ItemManager").GetComponent<ItemManager>();
            for (int i = 0; i < currentLoot.lootItems.Length; i++)
            {
                if (Random.value <= currentPercent)
                {
                    if (Random.value <= currentLoot.lootItems[i].percentToDrop)
                    {
                        loot.Add(itemManager.GetItem(currentLoot.lootItems[i].itemName).GetComponent<Item>());
                        currentPercent -= currentPercent / 4;
                    }
                }

            }
            return loot;
            /*lootUI.SetLoot(loot.ToArray(), player);
            isLoot = true;*/
        }

    }

    public void SetLoot(List<Item> items, GameObject player)
    {
        Debug.Log("LootManager: Loot: " + items);
        if (items == null)
        {
        }
        if (items.Count != 0)
        {
            lootUI.SetLoot(items, player);
            Open();
        }
    }

    private Loot FindEnemy(string enemyName)
    {
        Debug.Log("EnemyName: LootManager, FindEnemy: " + enemyName);
        foreach (Loot loot in enemyPossibleLoot)
        {
            Debug.Log("Loot EnemyName: LootManager, FindEnemy: " + loot.enemyName );
            if (loot.enemyName == enemyName)
                return loot;
        }
        Debug.Log("There is no enemy with this name: FindEnemy, LootManager: " + enemyName);
        return enemyPossibleLoot[0];
    }
}
