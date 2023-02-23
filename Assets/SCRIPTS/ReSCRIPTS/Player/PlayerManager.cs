using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    //Variables importantes
    PlayerInput PlayerInput;
    public static GameObject activeCharacter;
    [SerializeField]GameObject[] characters;

    //Stats personajes

    //Variables de input
    int characterOrder;
    string buttonName;
    //public HUD_Controller hud_Controller;

    //Character Swap variables
    [SerializeField]public static float charSwapTime = 2f;
    bool canChange;

    void Awake() 
    {
        
#region Singelton
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
#endregion
        
        //hud_Controller = GameObject.Find("HealthBars").GetComponent<HUD_Controller>();
        PlayerInput = new PlayerInput();
        OnEnable();
        Reset();

        //Setea los player inputs
        PlayerInput.CharacterControls.SwitchCharacter.started += onCharacterSwitchInput;
        PlayerInput.CharacterControls.SwitchCharacter.canceled += onCharacterSwitchInput;  
    }

#region "Input functions"
    void OnEnable()
    {
        PlayerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        PlayerInput.CharacterControls.Disable();
    }

    void onCharacterSwitchInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            buttonName = context.control.displayName;
        }
        else
        {
            buttonName = null;
        }
    }

#endregion

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

        //-----
        canChange = true;
    }
    
    void Update() 
    {
        if(canChange)
        {
            SwitchCharacter();
        }
        //Debug.Log(buttonName);
    }
    
    void SwitchCharacter()
    {
        
        switch (buttonName)
        {
            case "1":
                if(EricStateManager.Instance.isDead)
                {
                    return;
                }
                characterOrder = 0;
                CharacterSwap(characterOrder);
            break;
            case "2":
                if(AntiaStateManager.Instance.isDead)
                {
                    return;
                }
                characterOrder = 1;
                CharacterSwap(characterOrder);
            break;
            case "3":
                if(SoraStateManager.Instance.isDead)
                {
                    return;
                }
                characterOrder = 2;
                CharacterSwap(characterOrder);
            break;
            case "4":
                if(MossiStateManager.Instance.isDead)
                {
                    return;
                }
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
        //Debug.Log("Character swap");
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

        HUDManager.Instance.ChangeActiveCharacter(i);
        StartCoroutine(CharSwapCD());
        //CharChange_CoolDown._charChange_CoolDown.StartCoroutine("StartTimer");
    }

    public void ForceCharacterSwap()
    {
        int nextCharacterIndex = characterOrder;
        do {
            nextCharacterIndex = (nextCharacterIndex + 1) % characters.Length;
        } while (characters[nextCharacterIndex].GetComponent<IStateManager>().isDead == true);
        CharacterSwap(nextCharacterIndex);
    }

}
