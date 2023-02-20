using UnityEngine;

public class SoraAbilityState : BaseState
{
    public override void EnterState(IStateManager character)
    {
        Collider[] colliders = Physics.OverlapSphere(character.Character.transform.position, SoraStateManager.Instance.AttackRadius, SoraStateManager.Instance.EnemyLayer);
        foreach (Collider collider in colliders)
        {
            //get the direction from the character to the enemy
            Vector3 direction = (collider.transform.position - character.Character.transform.position).normalized;
            //do something to the enemy, such as damaging it
            EnemyDamaged _enemyDamaged = collider.GetComponent<EnemyDamaged>();
            float distance = Vector3.Distance(character.Character.transform.position, collider.transform.position) + 1f;
            if(_enemyDamaged != null)
            {
                _enemyDamaged.OnEnemyDamaged(Mathf.CeilToInt((character.Power + character.Attack)/distance));
                _enemyDamaged.OnEnemyPushed(SoraStateManager.Instance.PushForceAbility * SoraStateManager.Instance.AttackRadius/distance, direction);
                Debug.Log(Mathf.CeilToInt((character.Attack + character.Power)/distance));
            }
            
        }
    }
    
    public override void UpdateState(IStateManager character)
    {
        
    }

    public override void ExitState(IStateManager character)
    {
        
    }
}

