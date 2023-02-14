using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBars : MonoBehaviour
{
    [SerializeField]int maxHealth;
    [SerializeField]int minHealth;
    [SerializeField]int currentHealth;
    [SerializeField]Image mask;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
        //currentHealth = PlayerManager.ericVida;
    }

    void GetCurrentFill()
    {
        float fillAmount = (float)currentHealth / (float)maxHealth;
        mask.fillAmount = fillAmount;
    }
}
