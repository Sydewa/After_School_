using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight_Prueba : MonoBehaviour
{

    public float followDistance;
    public float attackDistance;

    [SerializeField]Transform player;


    public bool isAttacking;

    void Update() {
        // Get the distance between the enemy and the player
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // If the distance is less than the follow distance, switch to following mode
        if (distance < followDistance) {
            FollowPlayer();
        }
        else{

        }

        // If the distance is less than the attack distance, switch to attacking mode
        if (distance < attackDistance) {
            AttackPlayer();
        }
        else{
            
        }
    }

    // Method for following the player
    void FollowPlayer() {
        // Move towards the player's position
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime);

        // Set isAttacking to false
        isAttacking = false;
    }

    // Method for attacking the player
    void AttackPlayer() {
        // Play an animation
        //GetComponent<Animator>().Play("AttackAnimation");

        // Set isAttacking to true
        isAttacking = true;

        // Check if the enemy has touched the player
        if (Vector3.Distance(transform.position, player.transform.position) < 10f) {
            // Reduce the player's health points

            // Set a timer before returning to following mode 
            Invoke("FollowPlayer", 1f); 
        } else { 
            // Pause for a second before returning to following mode 
            Invoke("FollowPlayer", 1f); 
        } 
    } 
}

