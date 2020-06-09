using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponHandlerType {
    Gun,
    MachineGun,
    ShotGun,
    Spear,
    Grenade
}

public class WeaponHandler : MonoBehaviour
{
    public string weaponName;

    public WeaponHandlerType weaponHandler;

    private Animator anim;

    [SerializeField]
    private GameObject AttackPoint;
    
    private LayerMask layer;
    // Start is called before the first frame update

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void MainAttack()
    {
        anim.SetTrigger("Attack");
    }

    public void OffAttack()
    {

    }

    public void Aim(bool aim)
    {
        anim.SetBool("Aim", aim);
    }

    public void TurnOnAttackPoint()
    {
        AttackPoint.SetActive(true);
    }

    public void TurnOffAttackPoint()
    {
        if (AttackPoint.activeInHierarchy)
        {
            AttackPoint.SetActive(false);
        }

    }

    public Collider[] hitCheck()
    {
        if (AttackPoint.activeInHierarchy)
            return Physics.OverlapSphere(AttackPoint.transform.position, 0.5f, LayerMask.GetMask("Creature"));
        return null;
    }

   
}
