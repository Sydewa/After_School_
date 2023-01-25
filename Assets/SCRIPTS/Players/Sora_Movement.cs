using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoraCharacterState { Idle, Running, Dying, Attack }
public class Sora_Movement : MonoBehaviour
{
    private CharacterController controller;
    //private Animator anim;
    
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
    private SoraCharacterState _SoraState;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        //anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        AntiaStates();
    }

    public void AntiaStates()
    {
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        if(PlayerManager.soraVida <= 0)
        {
            _SoraState = SoraCharacterState.Dying;
        }
        if(Input.GetButton("Fire1"))
        {
            _SoraState = SoraCharacterState.Attack;
        }
        switch(_SoraState)
        {
            case SoraCharacterState.Idle:
                //anim.SetBool("Run", false);
                if(move != Vector3.zero)
                {
                    _SoraState = SoraCharacterState.Running;
                }
                if(Input.GetButton("Fire1"))
                {
                    _SoraState = SoraCharacterState.Attack;
                }
            break;

            case SoraCharacterState.Running: 
                
                //anim.SetBool("Run", true);
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
                    _SoraState = SoraCharacterState.Idle;
                }
                if(Input.GetButton("Fire1"))
                {
                    //anim.SetBool("Run", false);
                    _SoraState = SoraCharacterState.Attack;
                }
            break;

            case SoraCharacterState.Attack:
                //Hacer animacion, en esa animacion crear un evento que cree un trigger que detecte si donde ha attackado Eric hay enemigos y danyarles.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 direction = hit.point - transform.position;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, rotation.eulerAngles.y, 0f), Time.deltaTime * smoothTimeLookAtMouse);
                }
                controller.Move(move.normalized * (speed/4.5f) * Time.deltaTime);
                _SoraState = SoraCharacterState.Idle;
            break;

            case SoraCharacterState.Dying:
            break; 

            default:
            break;
        }
    }
}
