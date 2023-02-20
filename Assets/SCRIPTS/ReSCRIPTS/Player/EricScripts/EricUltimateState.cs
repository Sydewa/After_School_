using UnityEngine;

public class EricUltimateState : BaseState
{
    public override void EnterState(IStateManager character)
    {
        //Hacer trigger de animacion y con eventos de animacion spawnear muchos petardos
        character.Animator.SetTrigger("Ultimate");
    }
    
    public override void UpdateState(IStateManager character)
    {
        //character.GoIdle();
    }

    public override void ExitState(IStateManager character)
    {
        
    }
}
