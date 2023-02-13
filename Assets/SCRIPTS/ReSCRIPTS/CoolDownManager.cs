using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolDownManager : MonoBehaviour
{
    public static CoolDownManager Instance;

    private List<Ability> abilitiesOnCooldown = new List<Ability>();
    
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        for(int i = 0; i < abilitiesOnCooldown.Count; i++)
        {
            abilitiesOnCooldown[i].currentCooldown -= Time.deltaTime;
            if(abilitiesOnCooldown[i].currentCooldown <= 0f)
            {
                abilitiesOnCooldown[i].currentCooldown = 0f;
                abilitiesOnCooldown.Remove(abilitiesOnCooldown[i]);
            }
        }
    }

    public void StartCooldown(Ability ability)
    {
        if(!abilitiesOnCooldown.Contains(ability))
        {
            ability.currentCooldown = ability.maxCooldown;
            abilitiesOnCooldown.Add(ability);
        }
    }
}
