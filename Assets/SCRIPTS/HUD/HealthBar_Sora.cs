using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Sora : MonoBehaviour
{
    [SerializeField]Stats soraStats;
    [SerializeField]int maxHealth;
    int minHealth;
    int currentHealth;
    [SerializeField]Image mask;
    void Start()
    {
        maxHealth = soraStats.vida;
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentFill();
        currentHealth = PlayerManager.soraVida;
    }

    void GetCurrentFill()
    {
        float fillAmount = (float)currentHealth / (float)maxHealth;
        mask.fillAmount = fillAmount;
    }
}
