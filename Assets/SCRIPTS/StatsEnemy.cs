using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats_Enemy", menuName = "Enemy Stats")]
public class StatsEnemy : ScriptableObject
{
    public int vida;
    public float speed;

    [Header("Variables attack")]
    public float attackSpeed;
    public int attackDMG;

    

}
