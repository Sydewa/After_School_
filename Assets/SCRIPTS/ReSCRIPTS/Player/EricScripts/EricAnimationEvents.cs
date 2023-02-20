using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EricAnimationEvents : MonoBehaviour
{
    //Referencia al state manager para acceder a sus variables
    [SerializeField]GameObject petardo;

    [Header ("Attack")]

    [SerializeField]Transform petardoSpawnPosition;
    [SerializeField]float petardoForce;
    [SerializeField]float rotationForce;

    [Header ("Ultimate")]
    [SerializeField]int numPetardos;
    [SerializeField]float maxRadius;
    [SerializeField]float ultimateForce;

    void AttackFinish()
    {
        //_stateManager._nextAttack += _stateManager.AttackSpeed;
        EricStateManager.Instance.GoIdle();
    }

    void SpawnPetardo()
    {
        GameObject clone = Instantiate(petardo, petardoSpawnPosition.position, Quaternion.Euler(0f, transform.eulerAngles.y, 0f), null);
        Rigidbody rb = clone.GetComponent<Rigidbody>();
        Vector3 force = transform.forward + Vector3.down/7f;
        rb.AddForce(force * petardoForce, ForceMode.Impulse);
        rb.AddTorque(new Vector3(Random.value, Random.value, Random.value) * rotationForce, ForceMode.Impulse);
    }

    void Ultimate()
    {
        for (int i = 0; i < numPetardos; i++)
        {
            GameObject clone = Instantiate(petardo, petardoSpawnPosition.position, Quaternion.identity, null);
            Rigidbody rb = clone.GetComponent<Rigidbody>();

            Vector3 direction = Random.insideUnitSphere.normalized * maxRadius;
            Vector3 force = direction + Vector3.up;
            rb.AddForce(force * ultimateForce, ForceMode.Impulse);

            Vector3 torque = new Vector3(Random.value, Random.value, Random.value) * rotationForce;
            rb.AddTorque(torque, ForceMode.Impulse);
        }
    }
}
