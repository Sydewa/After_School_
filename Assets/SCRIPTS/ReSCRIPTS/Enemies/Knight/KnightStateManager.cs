using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class KnightStateManager : MonoBehaviour, IEnemyStateManager
{
    public StatsEnemy Stats {get { return stats;}  set{ stats = value;}}
    public NavMeshAgent Agent {get; set;}
    public Animator Animator {get; set;}
    public Vector3 SpawnPosition {get; set;}
    public GameObject Enemy { get{return this.gameObject;}}

    //Stats
    [SerializeField]StatsEnemy stats;

    //States
    EnemyBaseState currentState;
    public PatrollingState PatrollingState = new PatrollingState();
    public ChasingState ChasingState = new ChasingState();
    public AttackState AttackState = new AttackState();
    public StunnedState StunnedState = new StunnedState();

    void Awake()
    {
        //Seteamos los stats y otras variables
        Agent = GetComponent<NavMeshAgent>();
        Animator = GetComponentInChildren<Animator>();
        SpawnPosition = Enemy.transform.position;
    }

    void Start()
    {
        currentState = PatrollingState;
        currentState.EnterState(this);
    }

#region State changes and Update

    void Update()
    {
        StateMachine();
        currentState.UpdateState(this);
    }

    void StateMachine()
    {
        switch (currentState.GetType().Name)
        {
            case "PatrollingState":
                if(Vector3.Distance(transform.position, PlayerManager.activeCharacter.transform.position) < Stats.VisionRange)
                {
                    SwitchState(ChasingState);
                }
            break;

            case "ChasingState":
                if(Vector3.Distance(transform.position, PlayerManager.activeCharacter.transform.position) < Stats.AttackRadius)
                {
                    SwitchState(AttackState);
                }
            break;

            case "AttackState":
                Vector3 direction = PlayerManager.activeCharacter.transform.position - transform.position;
                if (direction.magnitude > 0.1f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 20);
                }
            break;

            case "StunnedState":
            break;

            default:
            break;
        }
    }

    public void SwitchState(EnemyBaseState state)
    {
        ExitState();
        currentState = state;
        state.EnterState(this);
    }

    public void ExitState()
    {
        currentState.ExitState(this);
    }

    public void GoIdle()
    {
        SwitchState(PatrollingState);
    }

    public void GoChase()
    {
        SwitchState(ChasingState);
    }

    public void GoStunned()
    {
        SwitchState(StunnedState);
    }
#endregion

}
