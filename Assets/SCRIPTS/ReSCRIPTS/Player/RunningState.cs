using UnityEngine;

public class RunningState : BaseState
{

    float timePassed;    
    float turnSmoothVelocity;
    float turnSmoothTime = 0.1f;
    float accelerationTime = 0.5f;

    public override void EnterState(IStateManager character)
    {
        timePassed = 0f;
        character.Animator.SetBool("isRunning", true);
        //Debug.Log("Entramos a running");
    }
    
    public override void UpdateState(IStateManager character)
    {
        character.Animator.SetBool("isRunning", true);
        //Rotacion del personaje
        float targetAngle = Mathf.Atan2(character.CurrentMovement.x, character.CurrentMovement.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(character.Character.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        character.Character.transform.rotation = Quaternion.Euler(0, angle, 0);
                
        //Movimiento
        timePassed += Time.deltaTime;
        float acceleration = timePassed / accelerationTime;

        float currentSpeed = Mathf.Lerp(0, character.Speed, acceleration);
        character.CharacterController.Move(character.CurrentMovement.normalized * currentSpeed * Time.deltaTime);
        
    }

    public override void ExitState(IStateManager character)
    {
        timePassed = 0f;
        character.Animator.SetBool("isRunning", false);
        //Debug.Log("Salimos de running");
    }
}
