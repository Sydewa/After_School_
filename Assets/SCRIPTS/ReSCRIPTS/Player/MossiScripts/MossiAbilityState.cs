using UnityEngine;
using UnityEngine.InputSystem;

public class MossiAbilityState : BaseState
{
    float startTime;
    //float _nextAttack = 0f;
    Vector3 hitPosition;
    Vector3 direction;
    public override void EnterState(IStateManager character)
    {
        character.Animator.SetTrigger("Ability");

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            hitPosition = hit.point;
            direction = hitPosition - character.Character.transform.position;
            
        }
    }
    
    public override void UpdateState(IStateManager character)
    {
        Quaternion rotation = Quaternion.LookRotation(direction);
        character.Character.transform.rotation = 
            Quaternion.Slerp(character.Character.transform.rotation, 
            Quaternion.Euler(0f, rotation.eulerAngles.y, 0f), 
            Time.deltaTime * 18f);
    }

    public override void ExitState(IStateManager character)
    {

    }
}