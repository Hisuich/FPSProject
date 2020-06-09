using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Walking,
    Attacking,
    Patrol,
    Chase,
    Back,
    Dead
}



public class EnemyScript : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rigidbody;

    private EnemyBehavior enemyBehavior;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        enemyBehavior = GetComponent<EnemyBehavior>();
    }

    void Start()
    {  
    }
    // Update is called once per frame
    void Update()
    {
    }
}
