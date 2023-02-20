using UnityEngine;
using UnityEngine.InputSystem;

public class EricAttackState : BaseState
{
    float startTime;
    //float _nextAttack = 0f;
    Vector3 hitPosition;
    Vector3 direction;
    public override void EnterState(IStateManager character)
    {
        character.Animator.SetTrigger("Attack");

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            hitPosition = hit.point;
            direction = hitPosition - character.Character.transform.position;
            //Vector3 direction = hit.point - character.Character.transform.position;
            //Quaternion rotation = Quaternion.LookRotation(direction);
            //character.Character.transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
        }
    }
    
    public override void UpdateState(IStateManager character)
    {
       //Rota a la direccion del puntero en Update
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
