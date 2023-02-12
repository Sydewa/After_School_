using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EricStateManager : MonoBehaviour, IStateManager
{
    //Componentes importantes del personaje
    BaseState currentState;
    public Animator Animator { get; set;}
    public CharacterController CharacterController { get; set; }

    //Variables personaje
    public EricStats ericStats;

    //Para añadir más variables se debe cambiar el IStateManager y añadir la variable a EriStats para tenerlo asi todo bonito
    public int Health { get { return ericStats.Health; } set { ericStats.Health = value; } }
    public float Speed { get { return ericStats.Speed; } set { ericStats.Speed = value; } }
    public int Attack { get { return ericStats.Attack; } set { ericStats.Attack = value; } }
    public int Power { get { return ericStats.Power; } set { ericStats.Power = value; } }
    public float AttackSpeed { get { return ericStats.AttackSpeed; } set { ericStats.AttackSpeed = value; } }
    public float AbilityCD { get { return ericStats.AbilityCD; } set { ericStats.AbilityCD = value; } }

    //Aqui estan todos los estados que hay, el Idle y el Running se comparten
    public IdleState IdleState = new IdleState();
    public RunningState RunningState = new RunningState();
    public EricAttackState AttackState = new EricAttackState();

    //Variables temporales de input
    
    void Awake() 
    {
        Animator = GetComponent<Animator>();
        CharacterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        currentState = IdleState;
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
        
    }

    public void SwitchState(BaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public void ExitState()
    {
        currentState.ExitState(this);
    }

}
