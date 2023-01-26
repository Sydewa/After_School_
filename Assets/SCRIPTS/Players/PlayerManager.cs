using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static GameObject activeCharacter;

    [SerializeField]GameObject eric;
    [SerializeField]GameObject antia;
    [SerializeField]GameObject mossi;
    [SerializeField]GameObject sora;

    //Stats personajes
    public Stats ericStats;
    public Stats antiaStats;
    public Stats soraStats;
    public Stats mossiStats;
    public static int ericVida;
    public static int antiaVida;
    public static int soraVida;
    public static int mossiVida;

    void Start()
    {
        Reset();
    }
    void Reset()
    {
        activeCharacter = eric;
        activeCharacter.name = "Eric";
        eric.transform.position = new Vector3(activeCharacter.transform.position.x, 0.51f, activeCharacter.transform.position.z);
        antia.SetActive(false);
        mossi.SetActive(false);
        sora.SetActive(false);

        //Setear las variables
        ericVida = ericStats.vida;

        antiaVida = antiaStats.vida;

        soraVida = soraStats.vida;

        mossiVida = mossiStats.vida;
    }
    
    void Update() 
    {
        SwitchCharacter();
    }

    public static void CharacterDamaged(int damageTaken)
    {
        switch (activeCharacter.name)
        {
            case "Eric":
                ericVida -= damageTaken;
                Debug.Log("Eric:"+ericVida);
            break;
            case "Antia":
                antiaVida -= damageTaken;
                Debug.Log("Antia:"+antiaVida);
            break;
            case "Sora":
                soraVida -= damageTaken;
                Debug.Log("Sora:"+soraVida);
            break;
            case "Mossi":
                mossiVida -= damageTaken;
                Debug.Log("Mossi" + mossiVida);
            break;
            default:
            break;
        }
    }
    
    void SwitchCharacter()
    {
        switch (Input.inputString)
        {
            case "1":
            //Desactivo char activo, copio su transform y se lo aplico al personaje que quiero. Activo el personaje y lo convierto en el personaje activo.
                if(activeCharacter.name == "Eric")
                {
                    return;
                }
                eric.transform.position = new Vector3(activeCharacter.transform.position.x, 0.6f, activeCharacter.transform.position.z);
                eric.transform.rotation = Quaternion.Euler(0f, activeCharacter.transform.eulerAngles.y, 0f);
                activeCharacter.SetActive(false);
                eric.SetActive(true);
                activeCharacter = eric;
                activeCharacter.name = "Eric";
            break;
            case "2":
                if(activeCharacter.name == "Antia")
                {
                    return;
                }
                activeCharacter.SetActive(false);
                antia.transform.position = new Vector3(activeCharacter.transform.position.x, 0.69f, activeCharacter.transform.position.z);
                antia.transform.rotation = Quaternion.Euler(0f, activeCharacter.transform.eulerAngles.y, 0f);
                antia.SetActive(true);
                activeCharacter = antia;
                activeCharacter.name = "Antia";
            break;
            case "3":
                if(activeCharacter.name == "Sora")
                {
                    return;
                }
                activeCharacter.SetActive(false);
                sora.transform.position = new Vector3(activeCharacter.transform.position.x, 0f, activeCharacter.transform.position.z);
                sora.transform.rotation = Quaternion.Euler(0f, activeCharacter.transform.eulerAngles.y, 0f);
                sora.SetActive(true);
                activeCharacter = sora;
                activeCharacter.name = "Sora";
            break;
            case "4":
                if(activeCharacter.name == "Mossi")
                {
                    return;
                }
                activeCharacter.SetActive(false);
                mossi.transform.position = new Vector3(activeCharacter.transform.position.x, 1f, activeCharacter.transform.position.z);
                mossi.transform.rotation = Quaternion.Euler(0f, activeCharacter.transform.eulerAngles.y, 0f);
                mossi.SetActive(true);
                activeCharacter = mossi;
                activeCharacter.name = "Mossi";
            break;

            default:
            break;
        }
    }

}
