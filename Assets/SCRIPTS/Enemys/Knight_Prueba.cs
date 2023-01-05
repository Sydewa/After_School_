using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight_Prueba : MonoBehaviour
{

    public float followDistance;
    public float attackDistance;

    


    public bool isAttacking;

    void Update() 
    {
        FollowPlayer();
    }


    void FollowPlayer() {
        transform.position = Vector3.MoveTowards(transform.position, PlayerManager.activeCharacter.transform.position, Time.deltaTime);
        Vector3 playerPos = new Vector3(PlayerManager.activeCharacter.transform.position.x, PlayerManager.activeCharacter.transform.position.y, PlayerManager.activeCharacter.transform.position.z);
        Quaternion rotation = Quaternion.LookRotation(playerPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, rotation.eulerAngles.y, 0f), Time.deltaTime * 20f);
    }

    
}

