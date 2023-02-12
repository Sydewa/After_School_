using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EricStateManager : MonoBehaviour, IStateManager
{
#region "Variables"
    //Componentes importantes del personaje
    BaseState currentState;
    public Animator Animator { get; set;}
    public CharacterController CharacterController { get; set; }
    public PlayerInput PlayerInput { get; set; }
    //Con esta variable accedemos al transform del personaje, ejemplo character.Character.transform
    public GameObject Character { get{ return this.gameObject; } }

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
    public Vector2 CurrentMovementInput { get; set; }
    public Vector3 CurrentMovement { get; set; }
    
        //Variables de ataque
        bool isAttackPressed;
        bool attackEnd;

#endregion

    void Awake() 
    {
        //set animator, character controller and player input
        Animator = GetComponentInChildren<Animator>();
        CharacterController = GetComponent<CharacterController>();
        PlayerInput = new PlayerInput();

        // enable controls and set player inputs
        EnableControls();
        PlayerInput.CharacterControls.Move.started += onMovementInput;
        PlayerInput.CharacterControls.Move.canceled += onMovementInput;
        PlayerInput.CharacterControls.Move.performed += onMovementInput;

        PlayerInput.CharacterControls.Attack.started += onAttackInput;
        PlayerInput.CharacterControls.Attack.canceled += onAttackInput;
    }
#region "Input functions"
    void onMovementInput(InputAction.CallbackContext context)
    {
        CurrentMovementInput = context.ReadValue<Vector2>();
        CurrentMovement = new Vector3(CurrentMovementInput.x, 0, CurrentMovementInput.y);
    }

    void onAttackInput(InputAction.CallbackContext context)
    {
        isAttackPressed = context.ReadValueAsButton();
    }
#endregion

#region "State changes and Update"    
    // Todas las funciones relacionadas con cambiar de estado o cosas de estados
    void Start()
    {
        currentState = IdleState;
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);

        // Anyadimos la state machine que nos cambiará de estado
        StateMachine();

        //Le anyadimos gravedad al personaje
        ApplyGravity();
    }
    
    void StateMachine()
    {
        /*if(isAttackPressed)
        {
            SwitchState(AttackState);
        }*/
        
        switch (currentState.GetType().Name)
        {
            case "IdleState":
                if(CurrentMovement != Vector3.zero)
                {
                    SwitchState(RunningState);
                }
            break;

            case "RunningState":
                if(CurrentMovement == Vector3.zero)
                {
                    SwitchState(IdleState);
                }
            break;

            case "AttackState":

            break;
            default:
            break;
        }
    }

    public void SwitchState(BaseState state)
    {
        ExitState();
        currentState = state;
        state.EnterState(this);
    }

    public void ExitState()
    {
        currentState.ExitState(this);
    }
#endregion

    //Para activar y desactivar los controles del personaje
    public void EnableControls()
    {
        PlayerInput.CharacterControls.Enable();
    }
    public void DisableControls()
    {
        PlayerInput.CharacterControls.Disable();
    }

    // Otras funciones necesarias
    void ApplyGravity()
    {
        if(CharacterController.isGrounded)
        {
            CharacterController.SimpleMove(new Vector3(0f,-0.5f,0f));
            //Debug.Log("IsGrounded true");
        }
        else
        {
            CharacterController.SimpleMove(new Vector3(0f,-9.81f,0f));
        }
    }
}
