using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]string[] scenes;

    void Update()
    {
        //Hacer que alterne entre abrir y cerrar dependiendo de si ya esta abierto o no
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //Abrir menu opciones
            OpenOptions();
        }
    }

    // Update is called once per frame
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
        
    }
}
