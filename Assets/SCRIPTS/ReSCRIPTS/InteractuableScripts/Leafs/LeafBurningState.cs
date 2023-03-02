using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafBurningState : LeafBaseState
{
    float maxTime = 5f;
    float elapsedTime;
    float radius;

    float dmgInterval;
    float elapsedTimeDMG;
    public override void EnterState(GameObject leaf)
    {
        elapsedTime = 0f;
        elapsedTimeDMG = 0f;
    }

    public override void UpdateState(GameObject leaf)
    {
        elapsedTime += Time.deltaTime;
        elapsedTimeDMG += Time.deltaTime;

        if (elapsedTimeDMG >= 0.3f)
        {
            Collider[] colliders = Physics.OverlapSphere(leaf.transform.position, radius);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    PlayerDamaged.CharacterDamaged(Mathf.CeilToInt((EricStateManager.Instance.Power * 0.2f)/2));
                }
                else if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyDamaged _enemyDamaged = collider.GetComponent<EnemyDamaged>();
                    _enemyDamaged.OnEnemyDamaged(Mathf.CeilToInt(EricStateManager.Instance.Power * 0.2f));
                }
            }
            elapsedTimeDMG = 0.0f;
        }

        if(elapsedTime >= maxTime)
        {
            //Destroy(leaf);
        }
    }

    public override void ExitState(GameObject leaf)
    {

    }
}
