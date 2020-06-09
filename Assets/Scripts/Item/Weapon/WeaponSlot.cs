using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    // Start is called before the first frame update

    private Weapon _weapon;
    public Weapon weapon
    {
        get { return _weapon; }
        set { _weapon = value; }
    }

    void Awake()
    {
        weapon = GetComponentInChildren<Weapon>();
    }

    public void SetWeapon(Weapon newWeapon)
    {
        if (newWeapon == null)
        {
            Debug.Log("Can't set null newWeapon: WeaponSlot");
            return;
        }
        GameObject weaponObject = newWeapon.gameObject;
        weaponObject.transform.parent = transform;
        weaponObject.transform.localPosition = Vector3.zero;
        weaponObject.transform.localRotation = Quaternion.identity;
        weaponObject.SetActive(true);
        weapon = newWeapon;
    }

    public void ResetInfo()
    {
        if (weapon == null)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        else
            SetWeapon(weapon);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
