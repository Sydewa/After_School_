using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAntia : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField]float explosionRadius;
    [SerializeField]Stats antiaStats;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            return;
        }
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, LayerMask.GetMask("Enemy"));
        if(colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                EnemyDamaged _enemyDamaged = colliders[i].GetComponent<EnemyDamaged>();
                if(_enemyDamaged != null)
                {
                    _enemyDamaged.OnEnemyDamaged(Mathf.CeilToInt((40f + antiaStats.attack/2)/5f)/2);
                    _enemyDamaged.OnEnemySlow();
                }
            }
        }
        Destroy(this.gameObject);
        
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);    
    }
}
