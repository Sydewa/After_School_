using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class Ability : ScriptableObject
{
    [Header ("Habilidad")]
    
    public float maxCooldown;
    public float currentCooldown = 0f;

    public void PutOnCooldown()
    {
        CoolDownManager.Instance.StartCooldown(this);
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

    //Hacer en un futuro la funcion para que se muestre el cool down en el HUD
    //Calculo para mostrar en el cooldown (1f - currentCooldown/maxCooldown)
}
