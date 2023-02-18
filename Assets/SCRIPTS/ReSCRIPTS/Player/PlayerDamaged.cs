using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamaged : MonoBehaviour
{
    public static void CharacterDamaged(int damageTaken)
    {
        switch (PlayerManager.activeCharacter.name)
        {
            case "Eric":
                EricStateManager.Instance.CurrentHealth -= damageTaken;
                //Debug.Log("Eric:"+ericVida);
            break;
            case "Antia":
                AntiaStateManager.Instance.CurrentHealth -= damageTaken;
                //Debug.Log("Antia:"+antiaVida);
            break;
            case "Sora":
                SoraStateManager.Instance.CurrentHealth -= damageTaken;
                //Debug.Log("Sora:"+soraVida);
            break;
            case "Mossi":
                MossiStateManager.Instance.CurrentHealth -= damageTaken;
                //Debug.Log("Mossi" + mossiVida);
            break;
            default:
            break;
        }
    }
}
