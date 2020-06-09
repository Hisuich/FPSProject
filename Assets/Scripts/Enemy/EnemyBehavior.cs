using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Behavior
{
    Agressive,
    Normal,
    Neutral,
    PeaceFull
}

struct AgrObject
{
    public GameObject gameObject;
    public uint agr;
}

public class EnemyBehavior : MonoBehaviour
{

    private EnemyState state;

    private NavMeshAgent navMeshAgent;

    private EnemyEquip equip;
    private EnemyStats stats;

    private float patrolTime;
    public float timeToChangeDestination;

    private float chaseTime;
    public float maxChaseTime;

    public float patrolMinRadius;
    public float patrolMaxRadius;

    public float agrDistance;

    private float checkTime;
    private float timeToCheck;

    private Vector3 startPosition;

    private List<CreatureType> agrUnitTypes;
    private List<string> agrUnits;

    private List<AgrObject> unitsInAgrRadius;
    private GameObject agrUnit;

    // Start is called before the first frame update

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        equip = GetComponent<EnemyEquip>();
        if (navMeshAgent == null)
        {
            navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        }

        stats = GetComponent<EnemyStats>();
    }

    void Start()
    {
        
        checkTime = 3.0f;

        state = EnemyState.Patrol;
        startPosition = transform.position;

        unitsInAgrRadius = new List<AgrObject>();
        agrUnits = new List<string>();

        agrUnits.Add("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("EnemyBehavior, Update: enemyState: " + state);
        if (stats.isDead())
        {
            state = EnemyState.Dead;
        }
        if (state != EnemyState.Dead)
        {
            if (state == EnemyState.Patrol)
            {
                if (timeToCheck >= checkTime)
                {
                    CheckEnemyUnitsAround();
                    SetAgrUnit();
                    timeToCheck -= checkTime;
                }
                timeToCheck += Time.deltaTime;
            }

            if (agrUnit != null)
            {
                if (Vector3.Distance(transform.position, agrUnit.transform.position) <= equip.weapon.efficientRange)
                {
                    state = EnemyState.Attacking;
                }
                else
                {
                    state = EnemyState.Chase;
                }
            }
            if (chaseTime >= maxChaseTime)
            {
                chaseTime -= maxChaseTime;
                state = EnemyState.Back;
            }
        }

        switch (state)
        {
            case EnemyState.Patrol:
            {
                Patrol();
                break;
            }
            case EnemyState.Walking:
            {
                break;
            }
            case EnemyState.Attacking:
            {
                Attack();
                break;
            }
            case EnemyState.Dead:
            {
                break;
            }
            case EnemyState.Chase:
            {
                Chase();
                break;
            }
            case EnemyState.Back:
            {
                Back();
                break;
            }
            default: break;

        }
    }

    virtual public void Patrol()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = stats.walkSpeed;

        patrolTime += Time.deltaTime;

        if (patrolTime >= timeToChangeDestination)
        {
            SetNewDestination();
            patrolTime = 0f;
        }
    }

    virtual public void Chase()
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = stats.runSpeed;

        navMeshAgent.SetDestination(agrUnit.transform.position);
        chaseTime += Time.deltaTime;

    }

    virtual public void Attack()
    {
        navMeshAgent.isStopped = true;
        gameObject.transform.LookAt(agrUnit.transform);
        equip.weapon.MainAttack();
    }

    virtual public void Back()
    {
        unitsInAgrRadius.Clear();
        agrUnit = null;

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = stats.runSpeed;

        navMeshAgent.SetDestination(startPosition);
        if (Vector3.Distance(transform.position, startPosition) <= 1.0f)
            state = EnemyState.Patrol;
    }

    virtual public void SetNewDestination()
    {
        float path = Random.Range(patrolMinRadius, patrolMaxRadius);
        Vector3 randDir = Random.insideUnitSphere * path;
        randDir += transform.position;

        NavMeshHit hit;

        NavMesh.SamplePosition(randDir, out hit, path, -1);

        navMeshAgent.SetDestination(hit.position);
    }

    public void CheckEnemyUnitsAround()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, agrDistance, LayerMask.GetMask("Creature"));
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                Creature creature = collider.gameObject.GetComponent<Creature>();
                foreach (string name in agrUnits)
                {
                    if (name == creature.creatureName)
                    {
                        AgrObject agrObject = new AgrObject();
                        agrObject.gameObject = creature.gameObject;
                        agrObject.agr = (uint)Vector3.Distance(creature.transform.position, transform.position);
                        if (!(unitsInAgrRadius.Contains(agrObject)))
                            unitsInAgrRadius.Add(agrObject);
                    }
                }
            }
        }
    }

    private void SetAgrUnit()
    {
        if (unitsInAgrRadius.Count == 0)
        {
            return;
        }

        AgrObject agrUnit = unitsInAgrRadius[0];
        Debug.Log("SetAgrUnit, EnemyBehaviour: agrUnit has: " + agrUnit.gameObject.name);
        foreach (AgrObject agrObject in unitsInAgrRadius)
        {
            if (agrUnit.agr < agrObject.agr)
            {
                agrUnit = agrObject;
            }
        }

        this.agrUnit = agrUnit.gameObject;
    }

}
