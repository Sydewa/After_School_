using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTemporal : MonoBehaviour
{
    public GameObject Shop;
    public GameObject HUD;
    public Camera mainCamera;
    public Camera secondaryCamera;
    bool isShopOpen;
    bool isPlayerInsideShopTrigger;
    // Start is called before the first frame update
    void Awake()
    {
        Shop.SetActive(false);
        secondaryCamera.enabled = false;
    }

    void OpenShop()
    {
        Shop.SetActive(true);
        HUD.SetActive(false);
    }

    void CloseShop()
    {
        Shop.SetActive(false);
        HUD.SetActive(true);
    }

   void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OpenShop();
            mainCamera.enabled = false;
            secondaryCamera.enabled = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            CloseShop();
            mainCamera.enabled = true;
            secondaryCamera.enabled = false;
        }
    }
}
