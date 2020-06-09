using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Material : Item
{
    public float quality;
    public float purity;
    public float transcalency;
    public float electroconductivity;
    public float hardness;

    override public string  GetInfo()
    {
        string str = base.GetInfo();
        str += "Quality: " + quality + "\n";
        str += "Purity: " + purity + "\n";
        str += "Transcalency: " + transcalency + "\n";
        str += "Electroconductivity: " + electroconductivity + "\n";
        str += "Hardness: " + hardness + "\n";
        return str;
    }

    public override bool Compare(Item item)
    {
        Debug.Log("Material");
        if (!base.Compare(item))
            return false;
        Material material = (Material)item;

        if (quality != material.quality ||
            purity != material.purity ||
            transcalency != material.transcalency ||
            electroconductivity != material.electroconductivity ||
            hardness != material.hardness)
            return false;

        return true;
    }
}
