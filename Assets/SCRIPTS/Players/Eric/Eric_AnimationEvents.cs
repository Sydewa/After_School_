using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eric_AnimationEvents : MonoBehaviour
{
    Eric_Movement _ericMovement;
    [SerializeField]Stats _ericStats;

    [SerializeField]GameObject petardo;
    [SerializeField]Transform petardoSpawnPosition;
    [SerializeField]float petardoForce;
    [SerializeField]float rotationForce;

    void Awake()
    {
        _ericMovement = GetComponentInParent<Eric_Movement>();
    }
    
    public void AttackFinish()
    {
        _ericMovement.isOnAction = false;
        SpawnPetardo2();
        _ericMovement._nextAttack = Time.time + _ericStats.attackSpeed;
    }

    public void GoIdle()
    {
        _ericMovement._EricState = EricCharacterState.Idle;
    }
    
    public void SpawnPetardo2()
    {
        
        GameObject clone = Instantiate(petardo, petardoSpawnPosition.position, Quaternion.Euler(0f, transform.eulerAngles.y, 0f), null);
        Rigidbody rb = clone.GetComponent<Rigidbody>();
        Vector3 force = transform.forward + Vector3.down/7f;
        rb.AddForce(force * petardoForce, ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.value, Random.value, Random.value) * rotationForce, ForceMode.Impulse);
    }
}
