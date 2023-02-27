using UnityEngine;

public class SoraAbilityState : BaseState
{
    public override void EnterState(IStateManager character)
    {
        Collider[] colliders = Physics.OverlapSphere(character.Character.transform.position, SoraStateManager.Instance.AttackRadius);
        foreach (Collider collider in colliders)
        {
            Vector3 direction = (collider.transform.position - character.Character.transform.position).normalized;
            switch(collider.gameObject.layer)
            {
                case 8: //la layer 8 son los enemigos
                    EnemyDamaged _enemyDamaged = collider.GetComponent<EnemyDamaged>();
                    float distance = Vector3.Distance(character.Character.transform.position, collider.transform.position) + 1f;
                    if(_enemyDamaged != null)
                    {
                        _enemyDamaged.OnEnemyDamaged(Mathf.CeilToInt((SoraStateManager.Instance.Attack + SoraStateManager.Instance.Power)/distance));
                        _enemyDamaged.OnEnemyPushed(SoraStateManager.Instance.PushForceAbility/distance, direction);
                        //Debug.Log(Mathf.CeilToInt(SoraStateManager.Instance.Attack/distance));
                    }
                break;

                case 9: //La layer 9 es las leafs
                    LeafScript _leafScript  = collider.GetComponent<LeafScript>();
                    if(_leafScript != null)
                    {
                        Vector3 newDirection = new Vector3(direction.x, 0f, direction.z);
                        _leafScript.OnLeafPushed(newDirection);
                    }
                break;
                
                default:
                break;
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

