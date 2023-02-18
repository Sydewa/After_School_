using UnityEngine;
using UnityEngine.InputSystem;

public class SoraAttackState : BaseState
{
    float elapsedTime;
    public override void EnterState(IStateManager character)
    {
        character.Animator.SetBool("isAttacking", true);
    }
    
    public override void UpdateState(IStateManager character)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 direction = hit.point - character.Character.transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            character.Character.transform.rotation = 
                Quaternion.Slerp(character.Character.transform.rotation, 
                Quaternion.Euler(0f, rotation.eulerAngles.y, 0f), 
                Time.deltaTime * 10f);
        }

        //Moverse mientras dispara
        character.CharacterController.Move(character.CurrentMovement.normalized * character.Speed/6f * Time.deltaTime);
    }

    public override void ExitState(IStateManager character)
    {
        character.Animator.SetBool("isAttacking", false);
    }
}

