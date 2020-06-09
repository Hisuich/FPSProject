using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{

    [SerializeField]
    private GameObject AttackPoint;

    public float bulletSpeed;
    public float damage;
    public float precision
    {
        get;
        set;
    }

    private Rigidbody rigidbody;
    // Start is called before the first frame update
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Collider[] hitChecks = hitCheck();
        if (hitChecks.Length > 0)
        {
            EnemyStats stats = hitChecks[0].transform.gameObject.GetComponent<EnemyStats>();
            stats.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    public Collider[] hitCheck()
    {
        if (AttackPoint.activeInHierarchy)
            return Physics.OverlapSphere(AttackPoint.transform.position, 0.5f, LayerMask.GetMask("Enemy"));
        return null;
    }

    public void Launch(Camera camera)
    {
        rigidbody.velocity = (camera.transform.forward + new Vector3(Random.Range(0, 1-precision), Random.Range(0, 1 - precision))) * bulletSpeed;
        transform.LookAt(transform.position + rigidbody.velocity);
    }
}
