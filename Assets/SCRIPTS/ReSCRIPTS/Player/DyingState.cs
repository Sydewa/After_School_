using UnityEngine;

public class DyingState : BaseState
{
    float elapsedTime;
    public override void EnterState(IStateManager character)
    {
        //Trigger animation
        Debug.Log("Dead");
        elapsedTime = 0f;
    }
    
    public override void UpdateState(IStateManager character)
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= 2f)
        {
            PlayerManager.Instance.ForceCharacterSwap();
        }
    }

    public override void ExitState(IStateManager character)
    {
        
    }
}
