using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Mossi : MonoBehaviour
{
    [SerializeField]Stats mossiStats;
    [SerializeField]int maxHealth;
    int minHealth;
    int currentHealth;
    [SerializeField]Image mask;
    void Start()
    {
        //maxHealth = mossiStats.vida;
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
        //currentHealth = PlayerManager.mossiVida;
    }

    void GetCurrentFill()
    {
        //float fillAmount = (float)currentHealth / (float)maxHealth;
        //mask.fillAmount = fillAmount;
    }
}
