using UnityEngine;

public class RunningState : BaseState
{
    public override void EnterState(IStateManager character)
    {

    }
    
    public override void UpdateState(IStateManager character)
    {
        //Rotacion del personaje
        /*float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0, angle, 0);
                
        //Movimiento
        float x = 0;
        timePassed += Time.deltaTime;
        float acceleration = timePassed / accelerationTime;

        x = Mathf.Lerp(0, 1, acceleration);

        float currentSpeed = Mathf.Lerp(0, ericStats.speed, x);
        characterController.Move(move.normalized * currentSpeed * Time.deltaTime);
        */
    }

    public override void ExitState(IStateManager character)
    {
        
    }
}
