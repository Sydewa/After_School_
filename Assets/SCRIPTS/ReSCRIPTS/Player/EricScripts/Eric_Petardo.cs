using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eric_Petardo : MonoBehaviour
{
    [SerializeField]float explosionRadius;
    Rigidbody rb;
    //[SerializeField]float impulse;

    void Start()
    {
        //Debug.Log("Petardo");
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Ground"))
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, LayerMask.GetMask("Enemy"));
            if (colliders.Length > 0)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    EnemyDamaged _enemyDamaged = colliders[i].GetComponent<EnemyDamaged>();
                    if(_enemyDamaged != null)
                    {
                        _enemyDamaged.OnEnemyDamaged(Mathf.CeilToInt(EricStateManager.Instance.Attack + (EricStateManager.Instance.Power * 0.2f)));
                        Debug.Log(Mathf.CeilToInt(EricStateManager.Instance.Attack + (EricStateManager.Instance.Power * 0.15f)));
                    }
                }
            }
            Destroy(this.gameObject);
        }
        
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);    
    }
    
    
}
