using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]string[] scenes;

    public GameObject Buttons;

    public GameObject Options;
    public GameObject ControlsMenu;
    public GameObject SoundMenu;

    void Awake()
    {
        CloseOptions();
    }

    public void PlayButton()
    {
        SceneManager.LoadScene(scenes[0]);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void OpenOptions()
    {
        Buttons.SetActive(false);
        Options.SetActive(true);
        ControlsMenu.SetActive(false);
        SoundMenu.SetActive(false);
    }

    public void CloseOptions()
    {
        Buttons.SetActive(true);
        Options.SetActive(false);
        ControlsMenu.SetActive(false);
        SoundMenu.SetActive(false);
    }

    public void OpenControlsMenu()
    {
        Options.SetActive(false);
        ControlsMenu.SetActive(true);
        SoundMenu.SetActive(false);
    }

    public void OpenSoundMenu()
    {
        Options.SetActive(false);
        ControlsMenu.SetActive(false);
        SoundMenu.SetActive(true);
    }
}
