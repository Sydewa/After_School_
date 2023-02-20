using UnityEngine;

public class StunnedState : EnemyBaseState
{
    public override void EnterState(IEnemyStateManager enemy)
    {
        //triggear animacion de stun
        enemy.Animator.SetBool("isRunning", false);
        enemy.Agent.speed = 0f;
    }
    
    public override void UpdateState(IEnemyStateManager enemy)
    {

    }

    public override void ExitState(IEnemyStateManager enemy)
    {
        
    }
}
