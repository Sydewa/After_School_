using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafSpawner : MonoBehaviour
{
    [SerializeField]GameObject leafPrefab;
    float elapsedTime;
    [SerializeField]float radius;
    [SerializeField]Vector2 spawnTimeInterval;
    float spawnDelay;

    void Start()
    {
        spawnDelay = Random.Range(spawnTimeInterval.x, spawnTimeInterval.y);
    }

    private void OnTriggerStay(Collider other) 
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime >= spawnDelay)
            {
                Vector3 spawnPos = RandomCircle(transform.position, radius);
                Instantiate(leafPrefab, spawnPos, Quaternion.identity);
                Vector3 spawnPos2 = RandomCircle(transform.position, radius);
                Instantiate(leafPrefab, spawnPos2, Quaternion.identity);
                spawnDelay = Random.Range(spawnTimeInterval.x, spawnTimeInterval.y);
                elapsedTime = 0f;
            }
        }
    }

    private Vector3 RandomCircle(Vector3 center, float radius)
    {
        float angle = Random.Range(0f, 360f);
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        return pos;
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.yellow;
        //Vector3 circlePos = new Vector3(transform.position.x, 0f, transform.position.z);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
