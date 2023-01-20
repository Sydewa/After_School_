using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum EricCharacterState { Idle, Running, Dying, AttackStart, OnAttack, AbilityStart, OnAbility }
public class Eric_Movement : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;
    [SerializeField]Stats ericStats;

    bool isOnAction;
    
    //Variables de movimiento
    Vector3 velocity = new Vector3(0f, -9.81f, 0f);
    Vector3 move;
    [SerializeField]float smoothTimeMove;
    [SerializeField]float smoothTimeLookAtMouse;
    [SerializeField]float accelerationTime;
    float timePassed;

    //Variables Smooth rotacion
    [SerializeField]float turnSmoothTime;
    float turnSmoothVelocity;

    //-----------------------------------------------------------
    private EricCharacterState _EricState;

    //Variables ATTACK
    float _nextAttack;
    float _attackCancel;

    //Variables ABILITY LUPA
    [SerializeField]AnimationCurve damageCurve;
    float damageInterval = 0.1f;
    private float startTime;
    private float timeSinceLastDamage;
    private float _nextAbility;
    Eric_LupaScript _lupa;
    

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        _lupa = GetComponentInChildren<Eric_LupaScript>();
    }

    void Start()
    {
        _EricState = EricCharacterState.Idle;
    }

    void Update()
    {
        EricStates();
        //Anyadimos la gravedad para que al subir y bajar pendientes no se quede flotando en el aire
        controller.Move(velocity * Time.deltaTime);
        move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
    }
    
    public void EricStates()
    {

        CheckInput();
        switch(_EricState)
        {
            case EricCharacterState.Idle:
                //Debug.Log("Idle");
                isOnAction = false;
                anim.SetBool("isRunning", false);
                //Si se mueve enviar a Running state
                if(move != Vector3.zero)
                {
                    _EricState = EricCharacterState.Running;
                }
            break;

            case EricCharacterState.Running: 
                //Debug.Log("Running");
                Running();
                if(move == Vector3.zero)
                {
                    _EricState = EricCharacterState.Idle;
                }

            break;

            case EricCharacterState.AttackStart:  
                //Debug.Log("AttackStart");
                anim.SetBool("isRunning", false);
                isOnAction = true;
                anim.SetTrigger("Attack 0");
                AttackStart();
                _EricState = EricCharacterState.OnAttack;
            break;

            case EricCharacterState.OnAttack:
                //Debug.Log("AttackAnim");
                //Si el tiempo actual es mayor que el tiempo a partir del cual se puede cancelar animacion se acaba la fase de ataque.
                //Si el tiempo actual es mayor que el tiempo de cancelacion + 1f (que es el tiempo que tarda la animacion entera) enviar a idle
                //Si cuando se ha acabado el ataque y te mueves enviar a Running state
                if(Time.time > _attackCancel)
                {
                    isOnAction = false;
                }
                else if(Time.time > (_attackCancel + 1f))
                {
                    _EricState = EricCharacterState.Idle;
                }
                
                if(move != Vector3.zero && Time.time > _attackCancel)
                {
                    _EricState = EricCharacterState.Running;
                }
            break;

            case EricCharacterState.AbilityStart:
                startTime = Time.time;
                isOnAction = true;
                anim.SetBool("OnAbility", true);
                StartCoroutine(_lupa.OnLupaStart());
                _EricState = EricCharacterState.OnAbility;
            break;

            case EricCharacterState.OnAbility:

                //Mientras tengas pulsado el click derecho haz esto, si no enviar a idle   
                anim.SetBool("isRunning", false);
                if(Input.GetButton("Fire2"))
                {
                    //Rotacion lupa -----------------------------------
                    RayCast_Rotation(smoothTimeLookAtMouse);
                    AbilityDMG();
                }
                else
                {
                    _nextAbility = Time.time + ericStats.abilityCD;
                    StartCoroutine(_lupa.OnLupaEnd());
                    anim.SetBool("OnAbility", false);
                    _EricState = EricCharacterState.Idle;
                }
            break;

            case EricCharacterState.Dying:
            break; 

            default:
                _EricState = EricCharacterState.Idle;
            break;
        }
    }

    void Running()
    {
        anim.SetBool("isRunning", true);

        //Rotacion del personaje
        float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0, angle, 0);
                
        //Movimiento
        float x = 0;
        timePassed += Time.deltaTime;
        float acceleration = timePassed / accelerationTime;

        x = Mathf.Lerp(0, 1, acceleration);

        float currentSpeed = Mathf.Lerp(0, ericStats.speed, x);
        controller.Move(move.normalized * currentSpeed * Time.deltaTime);
        if(move == Vector3.zero)
        {
            timePassed = 0;
        }
    }

    void AttackStart()
    {
        RayCast_Rotation(50000f);
        //Empieza el timer para el attackCancel y el nextAttack y apartir de ahora estara atacando. Enviar al state OnAttack
        _attackCancel = Time.time + 0.6f;
        _nextAttack = _attackCancel + ericStats.attackSpeed;
        isOnAction = true;
    }

    void RayCast_Rotation(float rotationTime)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 direction = hit.point - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, rotation.eulerAngles.y, 0f), Time.deltaTime * rotationTime);
        }
    }

    void AbilityDMG()
    {
        float elapsedTime = Time.time - startTime;
        //Para que las estadisticas extras de los objetos tengan efecto se tienen que anyadir los keyframes de la curva manualemnte
        //Primero se anyade el tiempo y luego la variable de (en este caso) danyo
        damageCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 4f, 0f, 0.5f), new Keyframe(3f, 220f + (float)ericStats.power, 0f, 0f, 0f, 0f));
        float damageFromCurve = damageCurve.Evaluate(elapsedTime);

        int currentDamage = (int)damageFromCurve;
        int dmg = (int) Math.Ceiling((double) currentDamage/10);
        //currentDamage = Mathf.Min(currentDamage, maxDamage);
        //Este if manda el dmg de la habilidad cada 0,1 secs a los enemigos
        timeSinceLastDamage += Time.deltaTime;
        if (timeSinceLastDamage >= damageInterval)
        {
            timeSinceLastDamage = 0;
            //Enviar currentDamage/10 para que asi se envie correctamente el danyo
        }
        Debug.Log(dmg);
    }
    
    void CheckInput()
    {
        if(PlayerManager.ericVida <= 0)
        {
            _EricState = EricCharacterState.Dying;
        }
        //Si aprietas click izquierdo y el tiempo es mayor que el next attack, que _nextAttack es el tiempo del sistema del ataque anterior + el CD del ataque. 
        if(Input.GetButtonDown("Fire1") && Time.time > _nextAttack && !isOnAction)
        {
            _EricState = EricCharacterState.AttackStart;
        }

        if(Input.GetButtonDown("Fire2") && Time.time > _nextAbility && !isOnAction)
        {
            _EricState = EricCharacterState.AbilityStart;
        }

        //Meter los inputs de menu y tal
    }
}
