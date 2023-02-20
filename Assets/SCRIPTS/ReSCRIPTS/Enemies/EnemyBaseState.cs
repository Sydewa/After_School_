using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void EnterState(IEnemyStateManager enemy);
    
    public abstract void UpdateState(IEnemyStateManager enemy);

    public abstract void ExitState(IEnemyStateManager enemy);
}
