using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MunicionAntia : MonoBehaviour
{
    [SerializeField]Image mask;
    void Start()
    {
        MunicionDisplay(50f);
    }

    // Update is called once per frame
    public void MunicionDisplay(float currentAmount)
    {
        float fillAmount = currentAmount/50f;
        mask.fillAmount = fillAmount;
    }
}
