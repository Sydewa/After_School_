using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamaged : MonoBehaviour
{
    [SerializeField]StatsEnemy _stats;
    int enemyHealth;
    Rigidbody rb;

    void Awake()
    {
        enemyHealth = _stats.vida;
        rb = GetComponent<Rigidbody>();
    }
    
    public void OnEnemyDamaged(int dmgTaken)
    {
        enemyHealth -= dmgTaken;
        Debug.Log("VidaKnight: "+ enemyHealth);
        if(enemyHealth < 0)
        {
            //Hacer cosas
            Destroy(this.gameObject);
        }
    }

    public void OnEnemyPushed(float pushForce, Vector3 direction)
    {
        rb.AddForce(direction * pushForce, ForceMode.Impulse);
    }
}
