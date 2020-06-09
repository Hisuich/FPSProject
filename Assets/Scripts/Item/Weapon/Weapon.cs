using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Range,
    Melee,
    Throwing,
    ThrowingLeft
}

public class Weapon : Item
{
    public float damage;
    public float precision;

    public float efficientRange;
    public float maxRange;

    public float delayBetweenShoots;
    private float toNextShot;

    //if maxAmmo == 0 - the weapon is not needed in ammo
    public int maxAmmo;
    private int curAmmo;
    public Ammo ammo;
    public AmmoType ammoType;

    public bool isAttacking
    {
        get;
        set;
    }

    public bool needReload
    {
        get;
        set;
    }

    public bool reloading;

    [SerializeField]
    private bool isAutomatic;
    private bool isSingle;
    private bool isAiming;

    public WeaponType weaponType;

    public GameObject bullet;

    public Transform spawnBulletPosition;

    private WeaponHandler weaponHandler;

    // Start is called before the first frame update

    void Awake()
    {
       weaponHandler = GetComponent<WeaponHandler>();
       maxStack = 1;
       stackCount = 1;
    }

    void Start()
    {
        isSingle = false;
        itemType = ItemType.Weapon;
        
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
        Collider[] hitChecks = weaponHandler.hitCheck();
        if (hitChecks != null && hitChecks.Length > 0 && hitChecks[0])
        {
            Creature enemy = hitChecks[0].transform.gameObject.GetComponent<Creature>();
            weaponHandler.TurnOffAttackPoint();
            enemy.TakeDamage(damage);
        }

        if (toNextShot > 0)
        {
            toNextShot -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Reload"))
        {
            Reload();
        }
    }

    void Reload()
    {
        reloading = true;
        if (ammo == null)
        {
            needReload = true;
            return;
        }

        int needAmmo = maxAmmo - curAmmo;
        if (ammo.stackCount < needAmmo)
        {
            curAmmo += ammo.stackCount;
            ammo.stackCount = 0;
        }
        else
        {
            curAmmo += needAmmo;
            ammo.stackCount -= needAmmo;
        }
        if (ammo.stackCount == 0)
            needReload = true;
    }

    public void MainAttack()
    { 
        if (maxAmmo != 0 && curAmmo <= 0)
        {
            Reload();
            return;
        }
        if (toNextShot <= 0)
        {
            if (weaponType == WeaponType.Range)
            {
                GameObject bull = Instantiate(bullet);
                bull.transform.position = spawnBulletPosition.position;
                bull.GetComponent<ProjectileAttack>().precision = precision;
                bull.GetComponent<ProjectileAttack>().Launch(GetComponentInParent<Camera>());
                Destroy(bull, 5);
                curAmmo--;

            }
            if (weaponType == WeaponType.ThrowingLeft && isAiming)
            {
                GameObject spear = GameObject.Instantiate(bullet);
                spear.transform.position = spawnBulletPosition.position;
                spear.GetComponent<ProjectileAttack>().precision = precision;
                spear.GetComponent<ProjectileAttack>().Launch(GetComponentInParent<Camera>());
                Destroy(gameObject);
            }
            else
            {
                weaponHandler.MainAttack();
            }
            toNextShot += delayBetweenShoots;
        }

    }

    public void OffAttack()
    {

    }

    public void Aim(bool aim)
    {
        isAiming = aim;
        weaponHandler.Aim(aim);
    }

    override public string GetInfo()
    {
        string info = base.GetInfo();
        info += "Damage: " + damage + "\n";
        info += "Precision: " + precision + "\n";
        info += isAutomatic ? "Automatic\n" : "";
        return info;
    }
}
