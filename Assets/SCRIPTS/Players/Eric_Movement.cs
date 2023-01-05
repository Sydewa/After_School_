using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EricCharacterState { Idle, Running, Dying, Attack }
public class Eric_Movement : MonoBehaviour
{
    private CharacterController controller;
    private Animator anim;
    
    //Variables de movimiento
    [SerializeField]float speed;
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

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        EricStates();
    }
    
    public void EricStates()
    {
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        if(PlayerManager.ericVida <= 0)
        {
            _EricState = EricCharacterState.Dying;
        }
        if(Input.GetButton("Fire1"))
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
                if(Input.GetButton("Fire1"))
                {
                    _EricState = EricCharacterState.Attack;
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

                float currentSpeed = Mathf.Lerp(0, speed, x);
                controller.Move(move.normalized * currentSpeed * Time.deltaTime);
                if(move == Vector3.zero)
                {
                    timePassed = 0;
                    _EricState = EricCharacterState.Idle;
                }
                if(Input.GetButton("Fire1"))
                {
                    anim.SetBool("Run", false);
                    _EricState = EricCharacterState.Attack;
                }
            break;

            case EricCharacterState.Attack:
                //Hacer animacion, en esa animacion crear un evento que cree un trigger que detecte si donde ha attackado Eric hay enemigos y danyarles.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 direction = hit.point - transform.position;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, rotation.eulerAngles.y, 0f), Time.deltaTime * smoothTimeLookAtMouse);
                }
                _EricState = EricCharacterState.Idle;
            break;

            case EricCharacterState.Dying:
            break; 

            default:
            break;
        }
    }

    void Movement()
    {
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        if(move != Vector3.zero && Input.GetButton("Fire1") == false)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        
        else if(Input.GetButton("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 direction = hit.point - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, rotation.eulerAngles.y, 0f), Time.deltaTime * smoothTimeLookAtMouse);
            }
        } 
        if(move != Vector3.zero)
        {
            float x = 0;
            timePassed += Time.deltaTime;
            float acceleration = timePassed / accelerationTime; // Calculate the change in x over each unit of time

            x = Mathf.Lerp(0, 1, acceleration);

            float currentSpeed = Mathf.Lerp(0, speed, x); // Gradually increase the speed
            controller.Move(move.normalized * currentSpeed * Time.deltaTime);
        }
        else
        {
            timePassed = 0;
        }
    }
}
