using UnityEngine;

public class AttackState : EnemyBaseState
{
    public override void EnterState(IEnemyStateManager enemy)
    {
        enemy.Animator.speed = 1f;
        enemy.Agent.speed = 0f;
        enemy.Animator.SetTrigger("Attack");
    }
    
    public override void UpdateState(IEnemyStateManager enemy)
    {

    }

    public override void ExitState(IEnemyStateManager enemy)
    {
        
    }
}
