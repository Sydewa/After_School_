using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    //Variables importantes
    PlayerInput PlayerInput;

    public static GameObject activeCharacter;

    [SerializeField]GameObject[] characters;

    //Stats personajes

    //Variables de input
    int characterSwitchInput;
    //public HUD_Controller hud_Controller;

    //Character Swap variables
    [SerializeField]public static float charSwapTime = 2f;
    bool canChange;

    void Awake() 
    {
        //hud_Controller = GameObject.Find("HealthBars").GetComponent<HUD_Controller>();
        PlayerInput = new PlayerInput();

        //Setea los player inputs
        PlayerInput.CharacterControls.Move.started += onCharacterSwitchInput;
        PlayerInput.CharacterControls.Move.canceled += onCharacterSwitchInput;
        PlayerInput.CharacterControls.Move.performed += onCharacterSwitchInput;    
    }

#region "Input functions"
    void onCharacterSwitchInput(InputAction.CallbackContext context)
    {
        characterSwitchInput = context.ReadValue<int>();
    }

#endregion

    void Start()
    {
        Reset();
    }

    void Reset()
    {
        activeCharacter = characters[0];
        activeCharacter.name = characters[0].name.ToString();
        characters[0].transform.position = new Vector3(activeCharacter.transform.position.x, activeCharacter.transform.position.y, activeCharacter.transform.position.z);
        characterSwitchInput = 0;
        foreach(GameObject character in characters)
        {
            character.SetActive(false);
        }
        activeCharacter.SetActive(true);

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
    
    void SwitchCharacter()
    {
        switch (characterSwitchInput)
        {
            case 1:
            //Desactivo char activo, copio su transform y se lo aplico al personaje que quiero. Activo el personaje y lo convierto en el personaje activo.
                characterSwitchInput = 0;
                CharacterSwap(characterSwitchInput);
            break;
            case 2:
                characterSwitchInput = 1;
                CharacterSwap(characterSwitchInput);
            break;
            case 3:
                characterSwitchInput = 2;
                CharacterSwap(characterSwitchInput);
            break;
            case 4:
                characterSwitchInput = 3;
                CharacterSwap(characterSwitchInput);
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

        //hud_Controller.ChangeActiveCharacter(i);
        StartCoroutine(CharSwapCD());
        CharChange_CoolDown._charChange_CoolDown.StartCoroutine("StartTimer");
    }

}
