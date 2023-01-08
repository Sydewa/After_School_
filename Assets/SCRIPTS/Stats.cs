using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats", menuName = "Basic Stats")]
public class Stats : ScriptableObject
{
    public int vida;
    public float speed;

    [Header("Variables attack")]
    public float attackSpeed;
    public int attack;

    [Header("Variables habilidad")]
    public float abilityCD;
    public int power;

}
