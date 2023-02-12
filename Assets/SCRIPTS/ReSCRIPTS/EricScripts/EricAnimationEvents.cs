using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EricAnimationEvents : MonoBehaviour
{
    //Referencia al state manager para acceder a sus variables
    EricStateManager _stateManager;

    [SerializeField]GameObject petardo;
    [SerializeField]Transform petardoSpawnPosition;
    [SerializeField]float petardoForce;
    [SerializeField]float rotationForce;

    void Awake()
    {
        _stateManager = GetComponentInParent<EricStateManager>();    
    }

    void AttackFinish()
    {
        //_stateManager._nextAttack += _stateManager.AttackSpeed;
        _stateManager.SwitchState(_stateManager.IdleState);
    }

    void SpawnPetardo()
    {
        GameObject clone = Instantiate(petardo, petardoSpawnPosition.position, Quaternion.Euler(0f, transform.eulerAngles.y, 0f), null);
        Rigidbody rb = clone.GetComponent<Rigidbody>();
        Vector3 force = transform.forward + Vector3.down/7f;
        rb.AddForce(force * petardoForce, ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.value, Random.value, Random.value) * rotationForce, ForceMode.Impulse);
    }
}
