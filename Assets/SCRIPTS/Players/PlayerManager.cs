using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static GameObject activeCharacter;

    [SerializeField]GameObject[] characters;

    //Stats personajes
    public Stats ericStats;
    public Stats antiaStats;
    public Stats soraStats;
    public Stats mossiStats;
    public static int ericVida;
    public static int antiaVida;
    public static int soraVida;
    public static int mossiVida;

    int characterOrder;
    public HUD_Controller hud_Controller;

    //Character Swap variables
    [SerializeField]public static float charSwapTime = 2f;
    bool canChange;

    void Awake() 
    {
        hud_Controller = GameObject.Find("HealthBars").GetComponent<HUD_Controller>();    
    }
    void Start()
    {
        Reset();
    }

    void Reset()
    {
        activeCharacter = characters[0];
        activeCharacter.name = characters[0].name.ToString();
        characters[0].transform.position = new Vector3(activeCharacter.transform.position.x, activeCharacter.transform.position.y, activeCharacter.transform.position.z);
        characterOrder = 0;
        foreach(GameObject character in characters)
        {
            character.SetActive(false);
        }
        activeCharacter.SetActive(true);

        //Setear las variables
        ericVida = ericStats.vida;

        antiaVida = antiaStats.vida;

        soraVida = soraStats.vida;

        mossiVida = mossiStats.vida;

        //-----
        canChange = true;
    }
    
    void Update() 
    {
        if(canChange)
        {
            SwitchCharacter();
        }
        
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
                characterOrder = 0;
                CharacterSwap(characterOrder);
            break;
            case "2":
                characterOrder = 1;
                CharacterSwap(characterOrder);
            break;
            case "3":
                characterOrder = 2;
                CharacterSwap(characterOrder);
            break;
            case "4":
                characterOrder = 3;
                CharacterSwap(characterOrder);
            break;

            default:
            break;
        }
    }

    IEnumerator CharSwapCD()
    {
        canChange = false;
        yield return new WaitForSeconds(charSwapTime);
        canChange = true;
    }

    void CharacterSwap(int i)
    {
        if(activeCharacter.name == characters[i].name)
        {
            return;
        }
        characters[i].transform.position = new Vector3(activeCharacter.transform.position.x, activeCharacter.transform.position.y, activeCharacter.transform.position.z);
        characters[i].transform.rotation = Quaternion.Euler(0f, activeCharacter.transform.eulerAngles.y, 0f);
        activeCharacter.SetActive(false);
        characters[i].SetActive(true);
        activeCharacter = characters[i];
        activeCharacter.name = characters[i].name.ToString();

        hud_Controller.ChangeActiveCharacter(i);
        StartCoroutine(CharSwapCD());
        CharChange_CoolDown._charChange_CoolDown.StartCoroutine("StartTimer");
    }

}
