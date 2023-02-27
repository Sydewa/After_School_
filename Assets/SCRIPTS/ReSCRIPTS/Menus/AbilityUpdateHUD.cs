using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUpdateHUD : MonoBehaviour
{
    [SerializeField] Image mask;
    [SerializeField] Ability ability;

    void Start()
    {
        ability.CooldownStarted += UpdateCooldown;
        mask.fillAmount = 1f;
    }

    void OnDestroy()
    {
        ability.CooldownStarted -= UpdateCooldown;
    }

    void UpdateCooldown()
    {
        StartCoroutine(UpdateCooldownCoroutine());
    }

    IEnumerator UpdateCooldownCoroutine()
    {
        while (!ability.IsAbilityReady())
        {
            float fillAmount = (ability.maxCooldown - ability.currentCooldown) / ability.maxCooldown;
            mask.fillAmount = fillAmount;
            yield return null;
        }
    }
}
