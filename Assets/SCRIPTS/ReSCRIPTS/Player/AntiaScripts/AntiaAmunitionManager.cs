using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiaAmunitionManager : MonoBehaviour
{
    public float bulletSpawnInterval { get { return AntiaStateManager.Instance.AttackSpeed; } set { AntiaStateManager.Instance.AttackSpeed = value; }}
    public GameObject bullet;
    public Transform spawnBulletPosition;
    float elapsedTime;
    public float pushForce;

    [Header ("Reload")]
    public float reloadInterval;

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

    public void Reload()
    {
        AntiaStateManager.Instance.isReloading = true;
        StartCoroutine(ReloadCoroutine());
    }

    IEnumerator ReloadCoroutine()
    {
        //float elapsedTime2;
        //float increment = AntiaStateManager.Instance.maxWaterAmount / reloadingTime;

        while(AntiaStateManager.Instance.currentWaterAmount < AntiaStateManager.Instance.maxWaterAmount)
        {
            yield return new WaitForSeconds(reloadInterval);
            AntiaStateManager.Instance.currentWaterAmount ++/*= Mathf.Min(AntiaStateManager.Instance.currentWaterAmount + (int)increment, AntiaStateManager.Instance.maxWaterAmount)*/;
        }

        AntiaStateManager.Instance.isReloading = false;
        Debug.Log(AntiaStateManager.Instance.isReloading);
    }
}
