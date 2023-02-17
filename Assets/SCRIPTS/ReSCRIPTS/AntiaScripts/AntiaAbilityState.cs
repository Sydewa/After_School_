using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiaAbilityState : BaseState
{
    float elapsedTime;
    Vector3 dashDirection;
    public override void EnterState(IStateManager character)
    {
        elapsedTime = 0f;
        if(character.CurrentMovement == Vector3.zero)
        {
            dashDirection = character.Character.transform.forward * AntiaStateManager.Instance.DashLength;
        }
        else
        {
            dashDirection = character.CurrentMovement.normalized * AntiaStateManager.Instance.DashLength;
        }
        //StartCoroutine(Dash(character));
    }   
    
    public override void UpdateState(IStateManager character)
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime < AntiaStateManager.Instance.DashDuration)
        {
            character.CharacterController.Move(dashDirection * AntiaStateManager.Instance.DashForce * Time.deltaTime);
        }
        else
        {
            // End the dash state and return to the idle state
            character.GoIdle();
        }
    }

    public override void ExitState(IStateManager character)
    {
        
    }

    /*IEnumerator Dash(IStateManager character)
    {
        while (elapsedTime <= AntiaStateManager.Instance.DashDuration)
        {
            elapsedTime += Time.deltaTime;
            character.CharacterController.Move(dashDirection * AntiaStateManager.Instance.DashForce * Time.deltaTime);
        }
        character.GoIdle();
    }*/
}
