using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class Ability : ScriptableObject
{
    [Header ("Habilidad")]
    
    public float maxCooldown;
    public float currentCooldown = 0f;
    public Sprite Sprite;

    public event System.Action CooldownStarted;

    public void PutOnCooldown()
    {
        CoolDownManager.Instance.StartCooldown(this);
        if (CooldownStarted != null)
        {
            CooldownStarted();
        }
    }

    //Reseteamos la variable para que cada vez que reiniciemos el juego los cooldowns esten reiniciados
    private void OnEnable() 
    {
        currentCooldown = 0f;    
    }

    //Nos devuelve una booleana que nos comprueba si la habilidad esta disponible
    public bool IsAbilityReady()
    {
        return currentCooldown <= 0f;
    }
}
