using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Antia : MonoBehaviour
{
    [SerializeField]Stats antiaStats;
    [SerializeField]int maxHealth;
    int minHealth;
    int currentHealth;
    [SerializeField]Image mask;
    void Start()
    {
        maxHealth = antiaStats.vida;
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
        //currentHealth = PlayerManager.antiaVida;
    }

    void GetCurrentFill()
    {
        float fillAmount = (float)currentHealth / (float)maxHealth;
        mask.fillAmount = fillAmount;
    }
}
