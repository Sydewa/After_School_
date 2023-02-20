using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossiAnimationEvents : MonoBehaviour
{
    public Transform spawnPosition;
    public GameObject rootsPrefab;
    public float rootsVelocity;
    public float rootsTime;
    public AnimationCurve rootsAnimation;
    float elapsedTime;

    void SpawnRoots()
    {
        elapsedTime = 0f;
        StartCoroutine(RootsCoroutine());
    }
    
    /*IEnumerator RootsCoroutine()
    {
        GameObject clone = Instantiate(rootsPrefab, spawnPosition.position, Quaternion.Euler(0f, transform.eulerAngles.y, 0f), null);
        float x = clone.transform.localPosition.x;
        while(elapsedTime < rootsVelocity)
        {
            elapsedTime += Time.deltaTime;
            
            clone.transform.localPosition = new Vector3(rootsAnimation.Evaluate(elapsedTime), 0f, 0f);
            yield return null;
        }
        Destroy(clone);
    }*/

    IEnumerator RootsCoroutine()
    {
        // Calculate starting and ending positions of the roots animation
        Vector3 startPos = transform.position + MossiStateManager.Instance.Character.transform.forward * 0.1f; // Offset slightly above character's feet
        Vector3 endPos = transform.position + MossiStateManager.Instance.Character.transform.forward * rootsVelocity; // Move 2 units in the direction the character is facing

        // Instantiate roots prefab at starting position
        GameObject clone = Instantiate(rootsPrefab, startPos, Quaternion.Euler(0f, transform.eulerAngles.y, 0f));

        // Move roots animation from starting position to ending position over time
        float elapsedTime = 0f;
        while (elapsedTime < rootsTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rootsTime;
            clone.transform.position = Vector3.Lerp(startPos, endPos, rootsAnimation.Evaluate(t));
            yield return null;
        }

        // Destroy roots animation when finished
        Destroy(clone);
    }

    void MossiGoIdle()
    {
        MossiStateManager.Instance.GoIdle();
    }
    
}
