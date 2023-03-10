using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]Transform player;
    [SerializeField]Vector3 offset;


    // Update is called once per frame
    void Update()
    {
        player = PlayerManager.activeCharacter.transform;
        transform.position = new Vector3(player.position.x + offset.x, offset.y + player.position.y, player.position.z + offset.z);
    }
}
