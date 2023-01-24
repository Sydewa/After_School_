using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum KnightState { Patrolling , Dying, Chase, AttackStart, OnAttack }
public class Knight_Movement : MonoBehaviour
{
    KnightState _knightState;
    NavMeshAgent agent;
    Animator anim;
    Vector3 destination;
    [SerializeField]StatsEnemy _knightStats;

    [SerializeField]float visionRange;
    [SerializeField]float visionEndRange;
    [SerializeField]float attackRange;
    //Patrol variables ----------------
    [Header ("Patrolling")]

    [SerializeField]float patrollingSpeed;
    Vector3 spawnPosition;
    [SerializeField]float patrolRadius;
    float elapsedTimePatrolling;
    [SerializeField]Vector2 timeInterval;


    //Chasing variables------------------------
    [Header ("Chasing")]
    [SerializeField]float chasingSpeed;

    //Attack variables------------------------
    [Header ("Attack")]
    float elapsedTimeAttack;
    [SerializeField]Vector2 checkSphereTime;
    [SerializeField]float attackRadius;
    bool charDamaged;
    float _attackStartTime;
    float _nextAttack;
    float _checkSphereStart;
    float _checkSphereEnd;

    //[SerializeField]GameObject attackSphere;


    //Animacion variables-----------------------
    [Header ("Otros")]
    [SerializeField]float smoothTimeWalk;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        //attackSphere.SetActive(false);
        
    }
    
    void Start()
    {
        spawnPosition = transform.position;

        //Creamos un vector3 random para empezar el patrol
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        destination = spawnPosition + randomDirection;
    }

    // Update is called once per frame
    void Update()
    {
        StateSwitch();
        //AnimationChanges();
        //Rotation();
    }

    void StateSwitch()
    {
        switch (_knightState)
        {
            case KnightState.Patrolling:
                Patrol();
                if(Vector3.Distance(transform.position, PlayerManager.activeCharacter.transform.position) < visionRange)
                {
                    _knightState = KnightState.Chase;
                }
            break;

            case KnightState.Chase:
                Chase();
                if(Vector3.Distance(transform.position, PlayerManager.activeCharacter.transform.position) > visionEndRange)
                {
                    _knightState = KnightState.Patrolling;
                }
                if(Vector3.Distance(transform.position, PlayerManager.activeCharacter.transform.position) < attackRange)
                {
                    _knightState = KnightState.AttackStart;
                }
            break;

            case KnightState.AttackStart:
                AttackStart();
                _knightState = KnightState.OnAttack;
            break;

            case KnightState.OnAttack:
                OnAttack();
            break;

            case KnightState.Dying:
                Destroy(this.gameObject);
            break;

            default:
                _knightState = KnightState.Patrolling;
            break;
        }
    }

    void Patrol()
    {
        agent.speed = patrollingSpeed;
        anim.SetBool("isRunning", true);
        anim.speed = 1f;
        //Mathf.Lerp(anim.speed, agent.speed, smoothTimeWalk * Time.deltaTime);
        
        if(agent.remainingDistance < 0.5f)
        {
            elapsedTimePatrolling += Time.deltaTime;
            anim.SetBool("isRunning", false);
            anim.speed = 1f;

            if(Random.Range(timeInterval.x, timeInterval.y) < elapsedTimePatrolling)
            {
                
                //Vector3 desiredDestination = new Vector3(Random.Range(-4f, 4f) - spawnPosition.x, transform.position.y, Random.Range(-4f, 4f) - spawnPosition.z);
                Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
                destination = spawnPosition + randomDirection;
                //Debug.Log("Change");
                elapsedTimePatrolling = 0;
            }
        }
        agent.SetDestination(destination);
        
    }

    void Chase()
    {
        
        agent.destination = PlayerManager.activeCharacter.transform.position;
        agent.speed = chasingSpeed;
        anim.SetBool("isRunning", true);
        anim.speed = Mathf.Lerp(anim.speed, agent.velocity.magnitude/1.3f, smoothTimeWalk * agent.acceleration);

    }

    void AnimationChanges()
    {
        
        if(agent.velocity.magnitude > 0)
        {
            anim.SetBool("isRunning", true);
            anim.speed = Mathf.Lerp(anim.speed, agent.velocity.magnitude/2f, smoothTimeWalk * agent.acceleration);
            
        }
        else
        {
            anim.SetBool("isRunning", false);
            anim.speed = 1f;
        }
    }

    void AttackStart()
    {
        charDamaged = false;
        agent.destination = transform.position;
        anim.speed = 1f;
        _attackStartTime = Time.time;
        _nextAttack = _attackStartTime + _knightStats.attackSpeed;
        _checkSphereStart = _attackStartTime + checkSphereTime.x;
        _checkSphereEnd = _checkSphereStart + checkSphereTime.y;
        anim.SetTrigger("Attack");
        agent.speed = 0f;
    }

    void OnAttack()
    {
        if(Time.time > _checkSphereStart && Time.time < _checkSphereEnd)
        {
            //attackSphere.SetActive(true);
            
        }

        
        if(Time.time > _nextAttack)
        {
            _knightState = KnightState.Patrolling;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(spawnPosition, patrolRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    private void OnTriggerEnter(Collider col) 
    {
    
        if (!charDamaged)
        {
            
            if(col.tag == "Player")
            {
                PlayerManager.CharacterDamaged(_knightStats.attackDMG);
                charDamaged = true;
            }
            
        }
        
    }
}
