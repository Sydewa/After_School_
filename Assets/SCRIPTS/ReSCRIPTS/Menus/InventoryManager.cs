using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryManager : MonoBehaviour
{
    public GameObject Inventory;
    PlayerInput PlayerInput;

    //Pestanyas
    public GameObject CharactersWindow;
    public GameObject ItemsWindow;
    public GameObject EnemiesWindow;

    //bool checkInventoryInput;
    bool isInventoryOpen;
    // Start is called before the first frame update
    void Awake()
    {
        PlayerInput = new PlayerInput();
        Inventory.SetActive(false);
        EnableControls();
        PlayerInput.InventoryControls.OpenClose.performed += onInventoryInput;
    }
#region "Inputs"
    void onInventoryInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isInventoryOpen)
            {
                isInventoryOpen = false;
                Time.timeScale = 1f;
                CloseInventory();
                PlayerInput.CharacterControls.Enable();
            }
            else
            {
                PlayerInput.CharacterControls.Disable();
                Time.timeScale = 0f;
                isInventoryOpen = true;
                OpenInventory();
                OpenCharactersWindow();
            }
        }
    }

    public void EnableControls()
    {
        PlayerInput.InventoryControls.Enable();
    }
    public void DisableControls()
    {
        PlayerInput.InventoryControls.Disable();
    }
#endregion

    void OpenInventory()
    {
        Inventory.SetActive(true);
    }

    void CloseInventory()
    {
        Inventory.SetActive(false);
    }

#region Windows del Inventario
    public void OpenCharactersWindow()
    {
        CharactersWindow.SetActive(true);
        ItemsWindow.SetActive(false);
        EnemiesWindow.SetActive(false);
    }

    public void OpenItemsWindow()
    {
        CharactersWindow.SetActive(false);
        ItemsWindow.SetActive(true);
        EnemiesWindow.SetActive(false);
    }

    public void OpenEnemiesWindow()
    {
        CharactersWindow.SetActive(false);
        ItemsWindow.SetActive(false);
        EnemiesWindow.SetActive(true);
    }
#endregion

    
}
