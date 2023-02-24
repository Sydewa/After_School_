using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MossiStats : MonoBehaviour
{
    public int Health;
    public float Speed;
    public int Attack;
    public int Power;
    public float AttackSpeed;

    [Header ("Attack")]
    public Vector2 DashTime;
    public Vector2 DashSpeed;
    public float PushForce;
    public Transform AttackHitBox;
    public float AttackRadius;
    public LayerMask EnemyLayer;

    [Header ("Ability")]
    public float StunnedTime;


    public Ability basicAbility;
    public Ability ultimateAbility;
}
