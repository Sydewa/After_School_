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
                if(EricStateManager.Instance.CurrentHealth <= 0f)
                    return;
                EricStateManager.Instance.CurrentHealth -= damageTaken;
                if(EricStateManager.Instance.CurrentHealth <= 0f)
                {
                    PlayerManager.activeCharacter.GetComponent<IStateManager>().isDead = true;
                    PlayerManager.activeCharacter.GetComponent<IStateManager>().SwitchState(new DyingState());

                }
                //Debug.Log("Eric:"+ericVida);
            break;
            case "Antia":
                if(AntiaStateManager.Instance.CurrentHealth <= 0f)
                    return;
                AntiaStateManager.Instance.CurrentHealth -= damageTaken;
                if(AntiaStateManager.Instance.CurrentHealth <= 0f)
                {
                    PlayerManager.activeCharacter.GetComponent<IStateManager>().isDead = true;
                    PlayerManager.activeCharacter.GetComponent<IStateManager>().SwitchState(new DyingState());

                }
                //Debug.Log("Antia:"+antiaVida);
            break;
            case "Sora":
                if(SoraStateManager.Instance.CurrentHealth <= 0f)
                    return;
                SoraStateManager.Instance.CurrentHealth -= damageTaken;
                if(SoraStateManager.Instance.CurrentHealth <= 0f)
                {
                    PlayerManager.activeCharacter.GetComponent<IStateManager>().isDead = true;
                    PlayerManager.activeCharacter.GetComponent<IStateManager>().SwitchState(new DyingState());

                }
                //Debug.Log("Sora:"+soraVida);
            break;
            case "Mossi":
                if(MossiStateManager.Instance.CurrentHealth <= 0f)
                    return;
                MossiStateManager.Instance.CurrentHealth -= damageTaken;
                if(MossiStateManager.Instance.CurrentHealth <= 0f)
                {
                    PlayerManager.activeCharacter.GetComponent<IStateManager>().isDead = true;
                    PlayerManager.activeCharacter.GetComponent<IStateManager>().SwitchState(new DyingState());

                }
                //Debug.Log("Mossi" + mossiVida);
            break;
            default:
            break;
        }
    }
}
