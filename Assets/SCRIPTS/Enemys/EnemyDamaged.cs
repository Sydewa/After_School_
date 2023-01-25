using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamaged : MonoBehaviour
{
    [SerializeField]StatsEnemy _stats;
    int enemyHealth;

    void Awake()
    {
        enemyHealth = _stats.vida;
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
}
