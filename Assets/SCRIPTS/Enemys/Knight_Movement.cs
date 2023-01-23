using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum KnightState { Patrolling , Dying, Chase, AttackStart, OnAttack }
public class Knight_Movement : MonoBehaviour
{
    KnightState _knightState;
    NavMeshAgent agent;
    Vector3 destination;

    
    //Patrol variables ----------------
    Vector3 spawnPosition;
    float spawnPositionRadius;
    [SerializeField]float patrolRadius;
    float elapsedTime;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }
    
    void Start()
    {
        spawnPosition = transform.position;
        destination = new Vector3(Random.Range(-4f, 4f) - spawnPosition.x, transform.position.y, Random.Range(-4f, 4f) - spawnPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        StateSwitch();
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
        agent.destination = destination;
        if(Vector3.Distance(transform.position, destination) < 1f)
        {
            elapsedTime += Time.deltaTime;

            if(Random.Range(1f, 5f) < elapsedTime)
            {
                
                Vector3 desiredDestination = new Vector3(Random.Range(-4f, 4f) - spawnPosition.x, transform.position.y, Random.Range(-4f, 4f) - spawnPosition.z);
                //desiredDestination.x = Mathf.Clamp()

            }
        }
        else
        {
            elapsedTime = 0;
        }
    }

    void Chase()
    {
        agent.destination = PlayerManager.activeCharacter.transform.position;
        

    }
}
