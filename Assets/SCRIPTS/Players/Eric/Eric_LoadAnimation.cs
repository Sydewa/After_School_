using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eric_LoadAnimation : MonoBehaviour
{
    Eric_Movement script;

    void Awake()
    {
        script = GetComponentInParent<Eric_Movement>();
    }

    void ReturnIdle()
    {
        script.StartIdle();
    }
}
