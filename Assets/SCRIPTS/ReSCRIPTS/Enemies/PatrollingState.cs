using UnityEngine;

public class PatrollingState : EnemyBaseState
{
    float elapsedTimePatrolling;
    Vector3 destination;
    public override void EnterState(IEnemyStateManager enemy)
    {
        elapsedTimePatrolling = 0f;

        //Cosas para cuando empecemos el estado
        enemy.Agent.speed = enemy.Stats.PatrollingSpeed;
        enemy.Animator.SetBool("isRunning", true);
        enemy.Animator.speed = 1f;

        //Seleccionamos un punto random
        Vector3 randomDirection = Random.insideUnitSphere * enemy.Stats.PatrollingRadius;
        destination = enemy.SpawnPosition + randomDirection;
        enemy.Agent.SetDestination(destination);
    }
    
    public override void UpdateState(IEnemyStateManager enemy)
    {
        if(enemy.Agent.remainingDistance < 0.5f)
        {
            elapsedTimePatrolling += Time.deltaTime;
            enemy.Animator.SetBool("isRunning", false);
            enemy.Animator.speed = 1f;

            if(Random.Range(enemy.Stats.PatrolWaitingInterval.x, enemy.Stats.PatrolWaitingInterval.y) < elapsedTimePatrolling)
            {
                Vector3 randomDirection = Random.insideUnitSphere * enemy.Stats.PatrollingRadius;
                destination = enemy.SpawnPosition + randomDirection;
                enemy.Animator.SetBool("isRunning", true);
                elapsedTimePatrolling = 0;
            }
        }
        enemy.Agent.SetDestination(destination);
    }

    public override void ExitState(IEnemyStateManager enemy)
    {
        enemy.Animator.SetBool("isRunning", false);
    }
}
