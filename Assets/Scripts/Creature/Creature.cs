using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum CreatureType
{
    Spider,
    Humanoid
}

public class Creature : MonoBehaviour
{
    public string creatureName;

    public float hitPoints;
    public float maxHitPoints;

    public float walkSpeed;
    public float runSpeed;

    public bool isDead;
    public bool isLootable;
    List<Item> items;
    LootManager lootManager;

    private CreatureType creatureType
    {
        get { return creatureType; }
        set { }
    }

    void Awake()
    {
        lootManager = GameObject.FindGameObjectWithTag("LootManager").GetComponent<LootManager>();
    }

    void Update()
    {
        if (!isDead)
        {
            isDead = IsDead();
            if (isDead)
                Dead();
        }
    }

    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
    }

    private void Dead()
    {

        isLootable = true;
        items = lootManager.GenerateLoot(creatureName);
    }
    private bool IsDead()
    {
        return hitPoints <= 0;
    }

    public void Loot(GameObject player)
    {
        
        lootManager.SetLoot(items, player);
    }

    public bool AddHitPoints(float hitPoints)
    {
        if (this.hitPoints == maxHitPoints)
            return false;

        this.hitPoints += hitPoints;
        if (this.hitPoints > maxHitPoints)
        {
            this.hitPoints = maxHitPoints;
        }
        return true;

    }
}