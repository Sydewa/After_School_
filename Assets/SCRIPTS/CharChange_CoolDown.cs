using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharChange_CoolDown : MonoBehaviour
{
    float maxTime;
    [SerializeField]Image[] masks;
    public static CharChange_CoolDown _charChange_CoolDown;

    void Awake()
    {
        maxTime = PlayerManager.charSwapTime;
        foreach(Image mask in masks)
        {
            mask.fillAmount = 1f;
        }
        _charChange_CoolDown = this;
    }

    public IEnumerator StartTimer()
    {
        //Debug.Log("StartTimer");
        float elapsedTime = 0f;

        while(elapsedTime < maxTime)
        {
            elapsedTime += Time.deltaTime;
            foreach(Image mask in masks)
            {
                mask.fillAmount = elapsedTime / maxTime;
            }
            yield return null;
        }

    }
    
}
