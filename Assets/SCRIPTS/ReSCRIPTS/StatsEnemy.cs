using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stats_Enemy", menuName = "Enemy Stats")]
public class StatsEnemy : ScriptableObject
{
    [Header ("Nuevas variables")]
    public int Health;
    public int Attack;
    public float AttackSpeed;

    [Header ("Speeds")]
    public float SlowResistance;

    [Header ("Patrolling variables")]
    public float PatrollingSpeed;
    public float PatrollingRadius;
    public Vector2 PatrolWaitingInterval;
    public float VisionRange;

    [Header ("Chasing variables")]
    public float ChasingSpeed;

    [Header ("Attack variables")]
    public float AttackRadius;
    

}
