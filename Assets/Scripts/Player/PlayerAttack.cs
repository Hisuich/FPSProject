using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private PlayerInventory playerInventory;

    void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GlobalVar.isUIOpen)
        {
            Attack();
        }
        
    }

    void Attack()
    {
        playerInventory.GetCurrentWeapon().isAttacking = Input.GetMouseButton(0); 
        if (playerInventory.GetCurrentWeapon().isAttacking)
        {
            playerInventory.GetCurrentWeapon().MainAttack();
        }

        if (Input.GetMouseButtonDown(1))
        {
            playerInventory.GetCurrentWeapon().Aim(true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            playerInventory.GetCurrentWeapon().Aim(false);
        }


    }
}
