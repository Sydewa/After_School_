using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AntiaCharacterState { Idle, Running, Dying, Attack, AbilityStart }
public class Antia_Movement : MonoBehaviour
{
    private CharacterController controller;
    //private Animator anim;
    [SerializeField]Stats antiaStats;

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
    private AntiaCharacterState _AntiaState;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        //anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        AntiaStates();
        controller.Move(velocity * Time.deltaTime);
        move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
    }

    public void AntiaStates()
    {
        CheckInput();
        switch(_AntiaState)
        {
            case AntiaCharacterState.Idle:
                //anim.SetBool("Run", false);
                if(move != Vector3.zero)
                {
                    _AntiaState = AntiaCharacterState.Running;
                }
            break;

            case AntiaCharacterState.Running: 
                
                //anim.SetBool("Run", true);
                Running();
                if(move ==  Vector3.zero)
                {
                    _AntiaState = AntiaCharacterState.Idle;
                }
            break;

            case AntiaCharacterState.Attack:
                //Hacer animacion, en esa animacion crear un evento que cree un trigger que detecte si donde ha attackado Eric hay enemigos y danyarles.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 direction = hit.point - transform.position;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, rotation.eulerAngles.y, 0f), Time.deltaTime * smoothTimeLookAtMouse);
                }
                controller.Move(move.normalized * (antiaStats.speed/4.5f) * Time.deltaTime);
                _AntiaState = AntiaCharacterState.Idle;
            break;

            case AntiaCharacterState.Dying:
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

        float currentSpeed = Mathf.Lerp(0, antiaStats.speed, x);
        controller.Move(move.normalized * currentSpeed * Time.deltaTime);
        if(move == Vector3.zero)
        {
            timePassed = 0;
            _AntiaState = AntiaCharacterState.Idle;
        }
    }

    void CheckInput()
    {
        
        if(PlayerManager.ericVida <= 0)
        {
            _AntiaState = AntiaCharacterState.Dying;
        }
        //Si aprietas click izquierdo y el tiempo es mayor que el next attack, que _nextAttack es el tiempo del sistema del ataque anterior + el CD del ataque. 
        if(Input.GetButtonDown("Fire1") && !isOnAction)
        {
            isOnAction = true;
            _AntiaState = AntiaCharacterState.Attack;
        }

        if(Input.GetButtonDown("Fire2") && !isOnAction)
        {
            isOnAction = true;
            _AntiaState = AntiaCharacterState.AbilityStart;
        }
        if(move != Vector3.zero && !isOnAction)
        {
            _AntiaState = AntiaCharacterState.Running;
        }

        //Meter los inputs de menu y tal en otro script
        
    }
}
