using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafPushedState : LeafBaseState
{
    Rigidbody rb;
    float pushForce;
    Vector3 direction;
    float maxTimePushed;
    float maxTime = 0.6f;
    float elapsedTime;
    Collider collider;
    public AnimationCurve forceCurve = new AnimationCurve(
        new Keyframe(0f, 1f),
        new Keyframe(0.75f, -0.6f),
        new Keyframe(1f, 0f)
        );
    List<GameObject> damagedEnemies = new List<GameObject>();

    public override void EnterState(GameObject leaf)
    {
        elapsedTime = 0f;

        rb = leaf.GetComponent<Rigidbody>();
        collider = leaf.GetComponent<Collider>();
        
        pushForce = leaf.GetComponent<LeafScript>().pushForce;
        direction = leaf.GetComponent<LeafScript>().direction;
        maxTimePushed = leaf.GetComponent<LeafScript>().maxTimePushed;

        rb.AddForce(new Vector3(0f, 1f, 0f) * 10f, ForceMode.Impulse);
    }

    public override void UpdateState(GameObject leaf)
    {
        elapsedTime += Time.deltaTime;
        float t = forceCurve.Evaluate(elapsedTime / maxTimePushed);
        
        rb.AddForce(pushForce * direction * Time.deltaTime * t, ForceMode.Impulse);
        rb.AddForce(new Vector3(0f, -9.81f, 0f) * Time.deltaTime, ForceMode.Force);

        //Detecta los enemigos arriba y abajo de la leaf da igual la altura y la rotacion del objeto
        Vector3 boxSize = new Vector3(collider.bounds.size.x + 0.5f, 20f, collider.bounds.size.z + 0.5f);
        Vector3 boxCenter = collider.bounds.center - new Vector3(0f, collider.bounds.extents.y + 0.05f, 0f);
        Collider[] enemyColliders = Physics.OverlapBox(boxCenter, boxSize, Quaternion.identity, LayerMask.GetMask("Enemy"));
        foreach (Collider enemyCollider in enemyColliders)
        {
            if (!damagedEnemies.Contains(enemyCollider.gameObject))
            {
                damagedEnemies.Add(enemyCollider.gameObject);
                Physics.IgnoreCollision(collider, enemyCollider, true);
                EnemyDamaged _enemyDamaged = enemyCollider.GetComponent<EnemyDamaged>();
                if(_enemyDamaged != null)
                {
                    _enemyDamaged.OnEnemyDamaged(Mathf.CeilToInt(SoraStateManager.Instance.Attack + SoraStateManager.Instance.Power));
                    Debug.Log(Mathf.CeilToInt((SoraStateManager.Instance.Attack + SoraStateManager.Instance.Power)));
                }
            }
        }

        //Cambia de estado al acabar el dash
        if(elapsedTime >= maxTime)
        {
            leaf.GetComponent<LeafScript>().SwitchState(leaf.GetComponent<LeafScript>().FallingLeafState);
        }
    }

    public override void ExitState(GameObject leaf)
    {
        damagedEnemies.Clear();
        rb.AddForce(-direction * 8f, ForceMode.Impulse);
    }
}
