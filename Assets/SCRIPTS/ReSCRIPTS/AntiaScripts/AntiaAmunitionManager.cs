using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiaAmunitionManager : MonoBehaviour
{
    public float bulletSpawnInterval;
    public GameObject bullet;
    public Transform spawnBulletPosition;
    float elapsedTime;
    public float pushForce;

    public void SpawnBullet()
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > bulletSpawnInterval)
        {
            GameObject clone = Instantiate(bullet, spawnBulletPosition.position, Quaternion.Euler(0f, AntiaStateManager.Instance.Character.transform.eulerAngles.y, 0f), null);
            Rigidbody rb = clone.GetComponent<Rigidbody>();
            rb.AddForce(AntiaStateManager.Instance.Character.transform.forward * pushForce /* *(AntiaStateManager.Instance.currentWaterAmount/5f)*/, ForceMode.Impulse);
            AntiaStateManager.Instance.currentWaterAmount --;
            elapsedTime = 0f;
        }
    }

    void Reload()
    {

    }
}
