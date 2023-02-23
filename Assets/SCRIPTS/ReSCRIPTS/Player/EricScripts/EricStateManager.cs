using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EricStateManager : MonoBehaviour, IStateManager
{
#region "Variables"
    public static EricStateManager Instance;
    //Componentes importantes del personaje
    BaseState currentState;
    public Animator Animator { get; set;}
    public CharacterController CharacterController { get; set; }
    public PlayerInput PlayerInput { get; set; }
    //Con esta variable accedemos al transform del personaje, ejemplo character.Character.transform
    public GameObject Character { get{ return this.gameObject; } }

    //Variables personaje
    public EricStats ericStats;
    public int CurrentHealth;

    //Para añadir más variables se debe cambiar el IStateManager y añadir la variable a EriStats para tenerlo asi todo bonito
    public int Health { get { return ericStats.Health; } set { ericStats.Health = value; } }
    public float Speed { get { return ericStats.Speed; } set { ericStats.Speed = value; } }
    public int Attack { get { return ericStats.Attack; } set { ericStats.Attack = value; } }
    public int Power { get { return ericStats.Power; } set { ericStats.Power = value; } }
    public float AttackSpeed { get { return ericStats.AttackSpeed; } set { ericStats.AttackSpeed = value; } }

    public bool isDead { get; set; }

    //Habilidades
    public Ability basicAbility { get { return ericStats.basicAbility; } }
    public Ability ultimateAbility { get { return ericStats.ultimateAbility; } }

    //Aqui estan todos los estados que hay, el Idle y el Running se comparten
    public IdleState IdleState = new IdleState();
    public RunningState RunningState = new RunningState();
    public EricAttackState AttackState = new EricAttackState();
    public EricAbilityState AbilityState = new EricAbilityState();
    public EricUltimateState UltimateState = new EricUltimateState();
    public DyingState DyingState = new DyingState();

    //Variables temporales de input
    public Vector2 CurrentMovementInput { get; set; }
    public Vector3 CurrentMovement { get; set; }
    
        //Variables de ataque
        bool isAttackPressed;
        public float _nextAttack;

        //Variables de ability
        bool isAbilityPressed;

        //Variables de la ultimate
        bool isUltimatePressed;

    //Script referente para hacer los evento de animacion
    
#endregion

    void Awake() 
    {
#region Singelton
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
#endregion
        //set animator, character controller and player input
        Animator = GetComponentInChildren<Animator>();
        CharacterController = GetComponent<CharacterController>();
        PlayerInput = new PlayerInput();

        //Otras variables que por si acaso ponemos en el awake
        CurrentHealth = Health;
        isDead = false;

        // enable controls and set player inputs
        EnableControls();
        PlayerInput.CharacterControls.Move.started += onMovementInput;
        PlayerInput.CharacterControls.Move.canceled += onMovementInput;
        PlayerInput.CharacterControls.Move.performed += onMovementInput;

        PlayerInput.CharacterControls.Attack.started += onAttackInput;
        PlayerInput.CharacterControls.Attack.canceled += onAttackInput;

        PlayerInput.CharacterControls.Ability.started += onAbilityInput;
        PlayerInput.CharacterControls.Ability.canceled += onAbilityInput;

        PlayerInput.CharacterControls.Ultimate.started += onUltimateInput;
        PlayerInput.CharacterControls.Ultimate.canceled += onUltimateInput;
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

    void onAbilityInput(InputAction.CallbackContext context)
    {
        isAbilityPressed = context.ReadValueAsButton();
    }

    void onUltimateInput(InputAction.CallbackContext context)
    {
        isUltimatePressed = context.ReadValueAsButton();
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
        // Anyadimos la state machine que nos cambiará de estado
        StateMachine();

        //Updateamos el state
        currentState.UpdateState(this);

        //Le anyadimos gravedad al personaje
        ApplyGravity();

        //Debugs y cosas
        //Debug.Log(isAbilityPressed);
    }
    
    void StateMachine()
    {

        switch (currentState.GetType().Name)
        {
            case "IdleState":
                //En idle puede cambiar a running, attack o ability
                if(CurrentMovement != Vector3.zero)
                {
                    SwitchState(RunningState);
                }
                if(isAttackPressed && Time.time > _nextAttack)
                {
                    _nextAttack = Time.time + AttackSpeed;
                    SwitchState(AttackState);
                }
                if(isAbilityPressed && basicAbility.IsAbilityReady())
                {
                    SwitchState(AbilityState);
                }
                if(isUltimatePressed && ultimateAbility.IsAbilityReady())
                {
                    SwitchState(UltimateState);
                }
            break;

            case "RunningState":
                //En running puede cambiar a idle, attack o ability
                if(CurrentMovement == Vector3.zero)
                {
                    SwitchState(IdleState);
                }
                if(isAttackPressed && Time.time > _nextAttack)
                {
                    _nextAttack = Time.time + AttackSpeed;
                    SwitchState(AttackState);
                }
                if(isAbilityPressed && basicAbility.IsAbilityReady())
                {
                    SwitchState(AbilityState);
                }
                if(isUltimatePressed && ultimateAbility.IsAbilityReady())
                {
                    SwitchState(UltimateState);
                }
            break;

            case "EricAttackState":

            break;

            case "EricAbilityState":
                //Debug.Log("Ability state");
                if(isAbilityPressed && basicAbility.IsAbilityReady())
                {
                    return;
                }
                else
                {
                    basicAbility.PutOnCooldown();
                    GoIdle();
                    //Debug.Log("Going Idle");
                }
            break;

            case "EricUltimateState":
                Debug.Log("Ultimate");
                ultimateAbility.PutOnCooldown();
            break;

            case "DyingState":
            break;

            default:
                //Intentar no poner nada en default, da problemas cuando se esta cambiando de estado
            break;
        }
    }

    public void SwitchState(BaseState state)
    {
        ExitState();
        currentState = state;
        state.EnterState(this);
    }

    public void GoIdle()
    {
        SwitchState(IdleState);
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
