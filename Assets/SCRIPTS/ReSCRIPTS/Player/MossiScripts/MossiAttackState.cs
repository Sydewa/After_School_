using UnityEngine;
using UnityEngine.InputSystem;

public class MossiAttackState : BaseState
{

    public override void EnterState(IStateManager character)
    {
        MossiStateManager.Instance.elapsedTimeAttack = 1.5f;
        MossiStateManager.Instance.elapsedTimeAttack2 = 0f;
    }
    
    public override void UpdateState(IStateManager character)
    {
        //Sumamos el tiempo que pasa durante la
        MossiStateManager.Instance.elapsedTimeAttack += Time.deltaTime * 5f;
        MossiStateManager.Instance.elapsedTimeAttack2 += Time.deltaTime;
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
        character.CharacterController.Move(character.CurrentMovement.normalized * character.Speed/5f * Time.deltaTime);
    }

    public override void ExitState(IStateManager character)
    {
        
    }
}
