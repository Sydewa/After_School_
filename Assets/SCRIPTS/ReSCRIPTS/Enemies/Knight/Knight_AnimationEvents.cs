using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight_AnimationEvents : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]StatsEnemy _knightStats;

    bool charDamaged;
    [SerializeField] Transform attackPosition;
    [SerializeField]float attackRadius;

    KnightStateManager knight;
    
    void Awake()
    {
        knight = GetComponentInParent<KnightStateManager>();
    }

    void CheckSphere()
    {
        if(Physics.OverlapSphere(attackPosition.position, attackRadius, LayerMask.GetMask("Player")).Length > 0 && !charDamaged)
        {
            PlayerDamaged.CharacterDamaged(knight.Stats.Attack);
            //DealDamage
            charDamaged = true;
        }
    }

    void ResetCharDamaged()
    {
        charDamaged = false;
    }

    void GoIdle()
    {
        knight.GoIdle();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(attackPosition.position, attackRadius);
    }
}
