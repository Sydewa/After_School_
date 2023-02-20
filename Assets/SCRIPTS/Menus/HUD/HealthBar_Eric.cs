using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Eric : MonoBehaviour
{
    [SerializeField]Stats ericStats;
    [SerializeField]int maxHealth;
    int minHealth;
    int currentHealth;
    [SerializeField]Image mask;
    void Start()
    {
        maxHealth = ericStats.vida;
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
