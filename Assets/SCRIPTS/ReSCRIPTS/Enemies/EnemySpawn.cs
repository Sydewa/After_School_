using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]float spawnTime;
    [SerializeField]GameObject prefab;
    [SerializeField]float spawnRadius;
    float elapsedTime;

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if(elapsedTime > spawnTime)
        {
            Vector3 randomVector = Random.insideUnitSphere * spawnRadius;
            Vector3 enemyPosition = transform.position + randomVector;
            GameObject clone = Instantiate(prefab, enemyPosition, Quaternion.identity);
            elapsedTime = 0f;
        }
    }
}
