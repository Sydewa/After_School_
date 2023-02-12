using UnityEngine;

public interface IStateManager
{
    Animator Animator { get; set; }
    CharacterController CharacterController { get; set;}

    int Health { get; set; }
    float Speed { get; set; }
    int Attack { get; set; }
    int Power { get; set; }

    float AttackSpeed { get; set; }
    float AbilityCD { get; set; }

    void SwitchState(BaseState state);

    void ExitState();
    
}
