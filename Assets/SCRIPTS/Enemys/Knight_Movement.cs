using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum KnightState { Patrolling , Dying, AttackStart, OnAttack }
public class Knight_Movement : MonoBehaviour
{
    KnightState _knightState;
    NavMeshAgent agent;

    //Patrol variables ----------------
    Transform spawnPosition;
    float patrolRadius;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    
    void Start()
    {
        spawnPosition.position = transform.position;
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
            break;

            case KnightState.AttackStart:
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
        //Creamos un vector 3 cada vez que queramos hacer patrol
        Vector3 destination = new Vector3(Random.Range(1f, 10f) + spawnPosition.position.x, transform.position.y, Random.Range(1f, 10f) + spawnPosition.position.z);

        agent.destination = destination;
    }
}
