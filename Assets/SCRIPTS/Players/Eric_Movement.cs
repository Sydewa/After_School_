using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EricCharacterState { Idle, Running, Dying, Attack }
public class Eric_Movement : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;
    [SerializeField]Stats ericStats;
    
    //Variables de movimiento
    //[SerializeField]float speed;
    Vector3 velocity;
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

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        _EricState = EricCharacterState.Idle;
    }

    void Update()
    {
        EricStates();
    }
    
    public void EricStates()
    {
        //Movimiento
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        //Comprobar vida del personaje, si es 0 o menos de 0 state de Eric a Dying
        if(PlayerManager.ericVida <= 0)
        {
            _EricState = EricCharacterState.Dying;
        }
        
        //Si aprietas click izquierdo y el tiempo es mayor que el next attack, que _nextAttack es el tiempo del sistema del ataque anterior + el CD del ataque.
        if(Input.GetButtonDown("Fire1") && Time.time > _nextAttack)
        {
            _EricState = EricCharacterState.Attack;
        }
        switch(_EricState)
        {
            case EricCharacterState.Idle:
                anim.SetBool("Run", false);
                if(move != Vector3.zero)
                {
                    _EricState = EricCharacterState.Running;
                }
            break;

            case EricCharacterState.Running: 
                anim.SetBool("Run", true);
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
                    _EricState = EricCharacterState.Idle;
                }
            break;

            case EricCharacterState.Attack:
                anim.SetBool("Run", false);
                //anim.Play("Run_FullCycle 0");
                //Hacer animacion, en esa animacion crear un evento que cree un trigger que detecte si donde ha attackado Eric hay enemigos y danyarles.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 direction = hit.point - transform.position;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
                }
                _nextAttack = Time.time + ericStats.attackSpeed;
                _EricState = EricCharacterState.Idle;
                
            break;

            case EricCharacterState.Dying:
            break; 

            default:
                _EricState = EricCharacterState.Idle;
            break;
        }
    }

    //iEnumerator CooldownTimer(float cooldownTime)


}
