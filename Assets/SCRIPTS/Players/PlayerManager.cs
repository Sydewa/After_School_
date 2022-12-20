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
    string[] numInput;

    private Vector3 copyTransform;

    void Start()
    {
        activeCharacter = eric;
        antia.SetActive(false);
        mossi.SetActive(false);
        sora.SetActive(false);
    }
    
    void Update() 
    {
        /*if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Input correcto");
        }*/
        //Input.inputString is GOD, detecta en string el nombre de la tecla que pulses. Se acepta en swtich.
        switch (Input.inputString)
        {
            case "1":
            //Desactivo char activo, copio su transform y se lo aplico al personaje que quiero. Activo el personaje y lo convierto en el personaje activo.
                activeCharacter.SetActive(false);
                eric.transform.position = new Vector3(activeCharacter.transform.position.x, 0f, activeCharacter.transform.position.z);
                eric.SetActive(true);
                activeCharacter = eric;
            break;
            case "2":
                activeCharacter.SetActive(false);
                antia.transform.position = new Vector3(activeCharacter.transform.position.x, 0f, activeCharacter.transform.position.z);
                antia.SetActive(true);
                activeCharacter = antia;
            break;
            case "3":
                activeCharacter.SetActive(false);
                sora.transform.position = new Vector3(activeCharacter.transform.position.x, 0f, activeCharacter.transform.position.z);
                sora.SetActive(true);
                activeCharacter = sora;
            break;
            case "4":
                activeCharacter.SetActive(false);
                mossi.transform.position = new Vector3(activeCharacter.transform.position.x, 0f, activeCharacter.transform.position.z);
                mossi.SetActive(true);
                activeCharacter = mossi;
            break;

            default:
            break;
        }

    }
}
