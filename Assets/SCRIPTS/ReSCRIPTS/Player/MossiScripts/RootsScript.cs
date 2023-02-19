using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootsScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            EnemyDamaged _enemyDamaged = other.GetComponent<EnemyDamaged>();
            _enemyDamaged.OnEnemyStunned(MossiStateManager.Instance.StunnedTime);
        }
    }
}
