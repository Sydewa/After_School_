using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDamaged : MonoBehaviour
{
    [SerializeField]StatsEnemy _stats;
    int enemyHealth;
    Rigidbody rb;
    NavMeshAgent agent;
    [SerializeField]float timeToReenableKinematic;
    bool isKinematicDisabled = false;
    float elapsedTime;

    void Awake()
    {
        enemyHealth = _stats.Health;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
    }
    
    public void OnEnemyDamaged(int dmgTaken)
    {
        enemyHealth -= dmgTaken;
        //Debug.Log("VidaKnight: "+ enemyHealth);
        if(enemyHealth < 0)
        {
            //Hacer cosas
            Destroy(this.gameObject);
        }
    }

    public void OnEnemyPushed(float pushForce, Vector3 direction)
    {
        if (!isKinematicDisabled)
        {
            rb.isKinematic = false;
            isKinematicDisabled = true;
            rb.AddForce((direction * pushForce)/_stats.SlowResistance, ForceMode.Impulse);
            StartCoroutine(EnableKinematicAfterTime(timeToReenableKinematic));
        }
    }

    public void OnEnemyStunned(float stunnedTime)
    {
        Debug.Log("Stunned");
        gameObject.SendMessage("GoStunned");
        StartCoroutine(DisableMovement(stunnedTime));
    }

    private IEnumerator DisableMovement(float stunnedTime)
    {
        yield return new WaitForSeconds(stunnedTime);
        gameObject.SendMessage("GoChase");
    }

    private IEnumerator EnableKinematicAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        rb.isKinematic = true;
        isKinematicDisabled = false;
    }

    public void OnEnemySlow()
    {
        StartCoroutine(OnEnemySlowed());
    }

    public IEnumerator OnEnemySlowed()
    {
        float currentSpeed = agent.speed;
        while(elapsedTime < 0.5f)
        {
            elapsedTime += Time.deltaTime;
            currentSpeed = currentSpeed/3f;
            yield return null;
        }
        elapsedTime = 0f;
    }
}
