using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterState(IStateManager character);
    
    public abstract void UpdateState(IStateManager character);

    public abstract void ExitState(IStateManager character);
}
