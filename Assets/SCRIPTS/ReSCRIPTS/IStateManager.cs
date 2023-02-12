using UnityEngine;

public interface IStateManager
{
    //Variables clave
    Animator Animator { get; set; }
    CharacterController CharacterController { get; set;}
    PlayerInput PlayerInput { get; set;}
    GameObject Character { get;}

    //Variables de input
    Vector2 CurrentMovementInput {get; set;}
    Vector3 CurrentMovement {get; set;}

    //Stats necesarias
    int Health { get; set; }
    float Speed { get; set; }
    int Attack { get; set; }
    int Power { get; set; }
    float AttackSpeed { get; set; }
    float AbilityCD { get; set; }

    //Funciones necesarias
    void SwitchState(BaseState state);
    void ExitState();
    
}
