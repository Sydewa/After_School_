using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossiAttackDashState : BaseState
{
    float clampedElapsedTime;

    List<int> hitEnemies = new List<int>();
    public override void EnterState(IStateManager character)
    {
        clampedElapsedTime = Mathf.Lerp(MossiStateManager.Instance.DashTime.x, MossiStateManager.Instance.DashTime.y, MossiStateManager.Instance.elapsedTimeAttack2);
    }
    
    public override void UpdateState(IStateManager character)
    {
        clampedElapsedTime -= Time.deltaTime;
        float dashForce = Mathf.Lerp(MossiStateManager.Instance.DashSpeed.x, MossiStateManager.Instance.DashSpeed.y, clampedElapsedTime);
        character.CharacterController.Move(character.Character.transform.forward * (character.Speed * dashForce * clampedElapsedTime) * Time.deltaTime);
        Collider[] colliders = Physics.OverlapSphere( MossiStateManager.Instance.AttackHitBox.position,  MossiStateManager.Instance.AttackRadius,  MossiStateManager.Instance.EnemyLayer);
        foreach (Collider collider in colliders)
        {
            int enemyID = collider.GetInstanceID();

            if(!hitEnemies.Contains(enemyID))
            {
                Vector3 direction = (collider.transform.position - character.Character.transform.position).normalized;
                EnemyDamaged _enemyDamaged = collider.GetComponent<EnemyDamaged>();
                float dashDMG = Mathf.Lerp(25f, (character.Attack) + 80f, MossiStateManager.Instance.elapsedTimeAttack2/2f);
                Debug.Log(dashDMG);
                if(_enemyDamaged != null)
                {
                    _enemyDamaged.OnEnemyDamaged(Mathf.CeilToInt((dashDMG)));
                    _enemyDamaged.OnEnemyPushed(MossiStateManager.Instance.PushForce * dashDMG, direction);
                }
                hitEnemies.Add(enemyID);
            }   
        }
        
        if(clampedElapsedTime <= 0f)
        {
            character.GoIdle();
        }
    }

    public override void ExitState(IStateManager character)
    {
        hitEnemies.Clear();
    }
}
