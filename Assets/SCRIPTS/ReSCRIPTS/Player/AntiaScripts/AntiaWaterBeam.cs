using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiaWaterBeam : MonoBehaviour
{
    LineRenderer lineRenderer;
    public Transform startPosition;
    Vector3 endPosition;
    Vector3 currentPosition;
    public float maxLength;

    public float maxAnimationTime;
    public float dmgIntervalTime;
    public float radius;

    float elapsedTime;

    void Awake() 
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.SetPosition(0, startPosition.position);
        lineRenderer.SetPosition(1, startPosition.position);
    }
    
    void Update()
    {
        lineRenderer.SetPosition(0, startPosition.position);
    }

    bool RayCast(out RaycastHit hit)
    {
        return Physics.Raycast(startPosition.position, startPosition.forward, out hit, maxLength);
    }

    public void StartLaser()
    {
        StartCoroutine(StartLaserCoroutine());
    }

    public void ExitLaser()
    {
        StartCoroutine(ExitLaserCoroutine());
    }
    public IEnumerator StartLaserCoroutine()
    {
        lineRenderer.enabled = true;
        float currentAnimationTime = 0f;
        RaycastHit hit;
        //StartCoroutine(CheckCollidersEveryInterval());
        while(currentAnimationTime < maxAnimationTime)
        {
            float t = currentAnimationTime/maxAnimationTime;
            currentAnimationTime += Time.deltaTime;
            if(RayCast(out hit))
            {
                endPosition = hit.point;
            }
            else
            {
                Vector3 playerDirection = startPosition.transform.forward;
                Vector3 playerPosition = startPosition.transform.position;
                endPosition = playerPosition + (playerDirection * maxLength);
            }
            currentPosition = Vector3.Lerp(startPosition.position, endPosition, t);
            lineRenderer.SetPosition(1, currentPosition);
            yield return null;
        }
    }

    public void UpdateLaser()
    {
        RaycastHit hit;
        if(RayCast(out hit))
        {
            endPosition = hit.point;
            currentPosition = hit.point;
        }
        else
        {
            Vector3 playerDirection = startPosition.transform.forward;
            Vector3 playerPosition = startPosition.transform.position;
            endPosition = playerPosition + (playerDirection * maxLength);
            currentPosition = playerPosition + (playerDirection * maxLength);
        }
        lineRenderer.SetPosition(1, endPosition);

        elapsedTime += Time.deltaTime;
        if(elapsedTime >= dmgIntervalTime)
        {
            AntiaStateManager.Instance.currentWaterAmount --;
            Debug.Log("Dmg enemy");
            CheckCollidersInRadius();
            elapsedTime = 0f;
        }
    }

    public IEnumerator ExitLaserCoroutine()
    {
        float currentAnimationTime = maxAnimationTime;
        RaycastHit hit;
        while (currentAnimationTime > 0f)
        {
            float t = currentAnimationTime / maxAnimationTime;
            currentAnimationTime -= Time.deltaTime;
            if(RayCast(out hit))
            {
                endPosition = hit.point;
            }
            else
            {
                Vector3 playerDirection = startPosition.transform.forward;
                Vector3 playerPosition = startPosition.transform.position;
                endPosition = playerPosition + (playerDirection * maxLength);
            }
            currentPosition = Vector3.Lerp(startPosition.position, endPosition, t);
            lineRenderer.SetPosition(1, currentPosition);
            yield return null;
        }
        //StopCoroutine(CheckCollidersEveryInterval());
        lineRenderer.enabled = false;
    }

    void CheckCollidersInRadius()
    {
        Collider[] colliders = Physics.OverlapSphere(currentPosition, radius);
        foreach (Collider collider in colliders)
        {
           for (int i = 0; i < colliders.Length; i++)
            {
                EnemyDamaged _enemyDamaged = colliders[i].GetComponent<EnemyDamaged>();
                if(_enemyDamaged != null)
                {
                    _enemyDamaged.OnEnemyDamaged(Mathf.CeilToInt((40f + AntiaStateManager.Instance.Attack/2)/5f));
                    _enemyDamaged.OnEnemySlow();
                }
            }
        }
    }
    IEnumerator CheckCollidersEveryInterval()
    {
        CheckCollidersInRadius();
        while(true)
        {
            yield return new WaitForSeconds(dmgIntervalTime);
            Debug.Log("Check");
            CheckCollidersInRadius();
        }
    }
}
