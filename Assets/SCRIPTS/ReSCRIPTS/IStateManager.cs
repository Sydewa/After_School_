using UnityEngine;

public interface IStateManager
{
    int Health { get; set; }
    float Speed { get; set; }
    int Attack { get; set; }
    int Power { get; set; }

    float AttackSpeed { get; set; }
    float AbilityCD { get; set; }

    void SwitchState(BaseState state);

    void ExitState();
    
}
