using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoraAnimationEvents : MonoBehaviour
{

    public void Attack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, SoraStateManager.Instance.AttackRadius);
        foreach(Collider collider in colliders)
        {
            //get the direction from the character to the enemy
            Vector3 direction = (collider.transform.position - transform.position).normalized;

            float dot = Vector3.Dot(direction, transform.forward);
            if (dot > Mathf.Cos(SoraStateManager.Instance.AttackAngle / 2f * Mathf.Deg2Rad))
            {
                //do something to the enemy, such as damaging it
                //Debug.Log("Enemy in range: " + collider.gameObject.name);

                switch(collider.gameObject.layer)
                {
                    case 8: //la layer 8 son los enemigos
                        EnemyDamaged _enemyDamaged = collider.GetComponent<EnemyDamaged>();
                        float distance = Vector3.Distance(transform.position, collider.transform.position) + 1f;
                        if(_enemyDamaged != null)
                        {
                            _enemyDamaged.OnEnemyDamaged(Mathf.CeilToInt(SoraStateManager.Instance.Attack/distance));
                            _enemyDamaged.OnEnemyPushed(SoraStateManager.Instance.PushForce/distance, direction);
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
    }
}
