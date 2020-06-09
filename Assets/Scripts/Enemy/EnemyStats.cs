using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    private Creature creature;

    public float runSpeed
    {
        get { return creature.runSpeed; }
    }

    public float walkSpeed
    {
        get { return creature.walkSpeed; }
    }

    public float hitPoints
    {
        get { return creature.hitPoints; }
    }
    public float attackRange;

    private float attackTime;
    public float delayBetweenAttack;
    public float damage;

    private void Awake()
    {
        creature = GetComponent<Creature>();
        if (creature == null)
        {
            Debug.Log("Enemy has no Creature");
        }
    }

    public void TakeDamage(float damage)
    {
        creature.TakeDamage(damage);

    }

    public bool isDead()
    {
        return creature.isDead;
    }


}
