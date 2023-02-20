using UnityEngine;

public class ChasingState : EnemyBaseState
{
    public override void EnterState(IEnemyStateManager enemy)
    {
        enemy.Agent.speed = enemy.Stats.ChasingSpeed;
        enemy.Animator.SetBool("isRunning", true);
        
    }
    
    public override void UpdateState(IEnemyStateManager enemy)
    {
        enemy.Agent.destination = PlayerManager.activeCharacter.transform.position;
        enemy.Animator.speed = Mathf.Lerp(enemy.Animator.speed, enemy.Agent.velocity.magnitude/1.3f, 0.1f * enemy.Agent.acceleration);
    }

    public override void ExitState(IEnemyStateManager enemy)
    {
        
    }
}
