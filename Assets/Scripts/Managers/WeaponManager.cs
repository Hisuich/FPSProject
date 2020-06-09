using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public string[] weaponNames;

    public GameObject[] weaponPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetWeapon(string weaponName)
    {
        for (int i = 0; i < weaponName.Length; i++)
        {
            if (weaponName == weaponNames[i])
            {
                return Instantiate(weaponPrefabs[i]);
            }
        }
        Debug.Log("There is no weapon prefab: " + weaponName);
        return null;
    }
}
