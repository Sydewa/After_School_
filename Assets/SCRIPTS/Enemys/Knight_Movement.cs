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

    
    //Patrol variables ----------------
    Vector3 spawnPosition;
    [SerializeField]float patrolRadius;
    float elapsedTime;
    [SerializeField]Vector2 timeInterval;

    //Animacion variables-----------------------
    [SerializeField]float smoothTimeWalk;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        
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
        AnimationChanges();
        //Rotation();
    }

    void StateSwitch()
    {
        switch (_knightState)
        {
            case KnightState.Patrolling:
                Patrol();
                if(Vector3.Distance(transform.position, PlayerManager.activeCharacter.transform.position) < 2f)
                {
                    _knightState = KnightState.Chase;
                }
            break;

            case KnightState.Chase:
                Chase();
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
        agent.speed = 1.5f;
        
        if(agent.remainingDistance < 0.5f)
        {
            elapsedTime += Time.deltaTime;

            if(Random.Range(timeInterval.x, timeInterval.y) < elapsedTime)
            {
                
                //Vector3 desiredDestination = new Vector3(Random.Range(-4f, 4f) - spawnPosition.x, transform.position.y, Random.Range(-4f, 4f) - spawnPosition.z);
                Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
                destination = spawnPosition + randomDirection;
                //Debug.Log("Change");
                elapsedTime = 0;
            }
        }
        agent.SetDestination(destination);
        
    }

    void Chase()
    {
        agent.destination = PlayerManager.activeCharacter.transform.position;
        agent.speed = 3f;
        

    }

    void AnimationChanges()
    {
        
        if(agent.velocity.magnitude > 0)
        {
            anim.SetBool("isRunning", true);
            anim.speed = Mathf.Lerp(anim.speed, agent.velocity.magnitude/2f, smoothTimeWalk * Time.deltaTime);
            
        }
        else
        {
            anim.SetBool("isRunning", false);
            anim.speed = 1f;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnPosition, patrolRadius);
    }
}
