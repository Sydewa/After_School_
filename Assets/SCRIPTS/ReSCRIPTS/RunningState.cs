using UnityEngine;

public class RunningState : BaseState
{

    float timePassed;    
    Vector3 turnSmoothVelocity;
    float turnSmoothTime = 0.1f;
    float accelerationTime = 0.5f;
    public override void EnterState(IStateManager character)
    {
        timePassed = 0f;
    }
    
    public override void UpdateState(IStateManager character)
    {
        //Rotacion del personaje
        /*float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        character.transform.rotation = Quaternion.Euler(0, angle, 0);
                
        //Movimiento
        float x = 0;
        timePassed += Time.deltaTime;
        float acceleration = timePassed / accelerationTime;

        x = Mathf.Lerp(0, 1, acceleration);

        float currentSpeed = Mathf.Lerp(0, character.Speed, x);
        character.CharacterController.Move(character.move.normalized * currentSpeed * Time.deltaTime);
        */
    }

    public override void ExitState(IStateManager character)
    {
        
    }
}
