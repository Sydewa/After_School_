using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoraStats : MonoBehaviour
{
    public int Health;
    public float Speed;
    public int Attack;
    public int Power;
    public float AttackSpeed;

    [Header ("Attack")]
    [Range(0,360)] public float attackAngle;
    public float attackRadius;
    public LayerMask enemyLayer;
    public float pushForce;

    [Header ("Ability")]
    public float pushForceAbility;


    [Header ("Abilities")]
    public Ability basicAbility;
    public Ability ultimateAbility;
}

