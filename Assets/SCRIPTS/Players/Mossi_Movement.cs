using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MossiCharacterState { Idle, Running, Dying, AttackStart, OnAttack, Ability }
public class Mossi_Movement : MonoBehaviour
{
    private CharacterController controller;
    //private Animator anim;
    [SerializeField]Stats mossiStats;

    bool isOnAction;
    
    //Variables de movimiento
    [SerializeField]float speed;
    [SerializeField]Vector3 velocity = new Vector3(0f,-9.81f,0f);
    Vector3 move;
    [SerializeField]float smoothTimeMove;
    [SerializeField]float smoothTimeLookAtMouse;
    [SerializeField]float accelerationTime;
    float timePassed;

    //Variables Smooth rotacion
    [SerializeField]float turnSmoothTime;
    float turnSmoothVelocity;


    [Header ("Attack")]

    float elapsedTimeAttack;
    float elapsedTimeAttack2;
    float clampedElapsedTime;
    [SerializeField]AnimationCurve attackDuration; 
    [SerializeField]Vector2 dashSpeed;
    [SerializeField]Vector2 dashTime;
    [SerializeField]Transform attackHitBox;
    [SerializeField]float attackRadius;
    [SerializeField]LayerMask enemyLayer;
    List<int> hitEnemies = new List<int>();

    //-----------------------------------------------------------
    private MossiCharacterState _MossiState;

    void Awake()
    {
        elapsedTimeAttack = 1.5f;
        controller = GetComponent<CharacterController>();
        //anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        MossiStates();
        controller.Move(velocity * Time.deltaTime);
        move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
    }
    
    public void MossiStates()
    {
        CheckInput();
        switch(_MossiState)
        {
            case MossiCharacterState.Idle:
                //anim.SetBool("Run", false);
                if(move != Vector3.zero)
                {
                    _MossiState = MossiCharacterState.Running;
                }
                if(Input.GetButton("Fire1"))
                {
                    _MossiState = MossiCharacterState.AttackStart;
                }
            break;

            case MossiCharacterState.Running: 
                Running();
            break;

            case MossiCharacterState.AttackStart:
                //Hacer animacion, en esa animacion crear un evento que cree un trigger que detecte si donde ha attackado Eric hay enemigos y danyarles.
                AttackStart();
            break;

            case MossiCharacterState.OnAttack:
                OnAttack();
            break;

            case MossiCharacterState.Dying:
            break; 

            default:
            break;
        }
    }

    void Running()
    {
        float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0, angle, 0);
        
        //Movimiento
        float x = 0;
        timePassed += Time.deltaTime;
        float acceleration = timePassed / accelerationTime;

        x = Mathf.Lerp(0, 1, acceleration);

        float currentSpeed = Mathf.Lerp(0, mossiStats.speed, x);
        controller.Move(move.normalized * currentSpeed * Time.deltaTime);
        if(move == Vector3.zero)
        {
            timePassed = 0;
            _MossiState = MossiCharacterState.Idle;
        }
        if(Input.GetButton("Fire1"))
        {
            //anim.SetBool("Run", false);
            _MossiState = MossiCharacterState.AttackStart;
        }
    }

    void AttackStart()
    {
        if(Input.GetButton("Fire1"))
        {
            elapsedTimeAttack += Time.deltaTime * 5f;
            elapsedTimeAttack2 += Time.deltaTime;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 direction = hit.point - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, rotation.eulerAngles.y, 0f), Time.deltaTime * smoothTimeLookAtMouse);
            }
            controller.Move(move.normalized * (mossiStats.speed/elapsedTimeAttack) * Time.deltaTime);
        }
        else
        {
            clampedElapsedTime = Mathf.Lerp(dashTime.x, dashTime.y, elapsedTimeAttack2);
            _MossiState = MossiCharacterState.OnAttack;
        }
    }

    void OnAttack()
    {
        //float time = attackDuration.Evaluate(Mathf.Clamp(elapsedTimeAttack, 0f, 1f));
        clampedElapsedTime -= Time.deltaTime;
        float dashForce = Mathf.Lerp(dashSpeed.x, dashSpeed.y, clampedElapsedTime);
        controller.Move(transform.forward * (mossiStats.speed * dashForce * clampedElapsedTime) * Time.deltaTime);
        Collider[] colliders = Physics.OverlapSphere(attackHitBox.position, attackRadius, enemyLayer);
        foreach (Collider collider in colliders)
        {
            int enemyID = collider.GetInstanceID();

            if(!hitEnemies.Contains(enemyID))
            {
                Vector3 direction = (collider.transform.position - transform.position).normalized;
                EnemyDamaged _enemyDamaged = collider.GetComponent<EnemyDamaged>();
                float dashDMG = Mathf.Lerp(25f, (mossiStats.attack) + 80f, elapsedTimeAttack2/2f);
                Debug.Log(dashDMG);
                if(_enemyDamaged != null)
                {
                    _enemyDamaged.OnEnemyDamaged(Mathf.CeilToInt((dashDMG)));
                    _enemyDamaged.OnEnemyPushed(mossiStats.pushForce * dashDMG, direction);
                }
                hitEnemies.Add(enemyID);
            }
            
        }
        
        if(clampedElapsedTime <= 0f)
        {
            elapsedTimeAttack = 1.5f;
            elapsedTimeAttack2 = 0f;
            isOnAction = false;
            hitEnemies.Clear();
            _MossiState = MossiCharacterState.Idle;
        }
    }

    void CheckInput()
    {
        /*
        bool isDead = false;
        if(PlayerManager.mossiVida <= 0)
        {
            isDead = true;
            _MossiState = MossiCharacterState.Dying;
        }
        
        if(Input.GetButton("Fire1") && !isOnAction && !isDead)
        {
            isOnAction = true;
            _MossiState = MossiCharacterState.AttackStart;
        }

        if(Input.GetButtonDown("Fire2") && !isDead && !isOnAction /*&& Time.time > _nextAbility)
        {
            isOnAction = true;
            _MossiState = MossiCharacterState.Ability;
        }
        */

        //Meter los inputs de menu y tal en otro script
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackHitBox.position, attackRadius);
    }
}
