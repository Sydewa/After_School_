using UnityEngine;
using UnityEngine.AI;

public interface IEnemyStateManager
{
    StatsEnemy Stats {get; set;}
    NavMeshAgent Agent {get; set;}
    Animator Animator {get; set;}

    Vector3 SpawnPosition { get; set;}
    GameObject Enemy {get;}

    void SwitchState(EnemyBaseState state);
    void ExitState();
    void GoIdle();
    void GoChase();
}
