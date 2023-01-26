using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temporal : MonoBehaviour
{
    int damage = 20;
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            PlayerManager.CharacterDamaged(damage);
            
        }
    }
}
