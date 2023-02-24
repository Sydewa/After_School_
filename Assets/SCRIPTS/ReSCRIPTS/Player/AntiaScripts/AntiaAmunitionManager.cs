using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiaAmunitionManager : MonoBehaviour
{
    [Header ("Reload")]
    public float reloadInterval;


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
