using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiaReload : MonoBehaviour
{
    float elapsedTimeReload;
    [SerializeField]MunicionAntia _municionAntia;
    [SerializeField]float reloadTime;
    Antia_Movement antiaMovement;
    float currentTankAmount;


    void Awake()
    {
        antiaMovement = GetComponentInChildren<Antia_Movement>();
    }
    
    public IEnumerator TriggerReload()
    {
        currentTankAmount = 0f;
        while(currentTankAmount <50f)
        {
            elapsedTimeReload += Time.deltaTime;
            //float currentTankAmount = waterIconCurve.Evaluate(elapsedTimeReload);
            if(elapsedTimeReload > reloadTime)
            {
                elapsedTimeReload = 0f;
                currentTankAmount +=1f;
                antiaMovement.waterTankAmount = currentTankAmount;
                _municionAntia.MunicionDisplay(currentTankAmount);
            }
            yield return null;
        }
        antiaMovement.isReloaded = true;
        elapsedTimeReload = 0f;
    }
}
