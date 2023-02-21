using UnityEngine;

public abstract class LeafBaseState 
{
    public abstract void EnterState(GameObject leaf);
    
    public abstract void UpdateState(GameObject leaf);

    public abstract void ExitState(GameObject leaf);
}
