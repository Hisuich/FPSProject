using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum AmmoType
{
    light
}
public class Ammo : Item
{
    public AmmoType ammoType;
    public float damage;
    public float armorPenetration;

    void Start()
    {
        maxStack = 250;
    }

    protected override void Update()
    {
        base.Update();
    }

    public override bool Compare(Item item)
    {
        if (!base.Compare(item))
            return false;

        Ammo ammo = (Ammo)item;

        if (ammoType != ammo.ammoType ||
            damage != ammo.damage ||
            armorPenetration != ammo.armorPenetration)
            return false;

        return true;

    }
}
