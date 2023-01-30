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
    
    void CheckSphere()
    {
        if(Physics.OverlapSphere(attackPosition.position, attackRadius, LayerMask.GetMask("Player")).Length > 0 && !charDamaged)
        {
            PlayerManager.CharacterDamaged(_knightStats.attackDMG);
            charDamaged = true;
        }
    }

    void ResetCharDamaged()
    {
        charDamaged = false;
    }
}
