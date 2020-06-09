using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class RepairPack : Item
{
    public float healPower;
    public float maxDurabilityFix;

    public override void Use(GameObject owner)
    {
        Creature creature = owner.GetComponent<Creature>();
        if (creature == null)
        {
            Debug.Log("I can't repair it: RepairPack, Use: " + owner.name);
            return;
        }

        if (creature.AddHitPoints(healPower))
        {
            stackCount--;
        }
    }

    public override string GetInfo()
    {
        string str = base.GetInfo();
        str += "Heal Power: " + healPower + "\n";
        return str;
    }
}

