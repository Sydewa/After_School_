using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MossiCharacterState { Idle, Running, Dying, Attack }
public class Mossi_Movement : MonoBehaviour
{
    private CharacterController controller;
    //private Animator anim;
    
    //Variables de movimiento
    [SerializeField]float speed;
    Vector3 velocity = new Vector3(0f,-9.81f,0f);
    Vector3 move;
    [SerializeField]float smoothTimeMove;
    [SerializeField]float smoothTimeLookAtMouse;
    [SerializeField]float accelerationTime;
    float timePassed;

    //Variables Smooth rotacion
    [SerializeField]float turnSmoothTime;
    float turnSmoothVelocity;

    //-----------------------------------------------------------
    private MossiCharacterState _MossiState;

    void Awake()
    {
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

        if(PlayerManager.mossiVida <= 0)
        {
            _MossiState = MossiCharacterState.Dying;
        }
        if(Input.GetButton("Fire1"))
        {
            _MossiState = MossiCharacterState.Attack;
        }
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
                    _MossiState = MossiCharacterState.Attack;
                }
            break;

            case MossiCharacterState.Running: 
                
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
                    _MossiState = MossiCharacterState.Idle;
                }
                if(Input.GetButton("Fire1"))
                {
                    //anim.SetBool("Run", false);
                    _MossiState = MossiCharacterState.Attack;
                }
            break;

            case MossiCharacterState.Attack:
                //Hacer animacion, en esa animacion crear un evento que cree un trigger que detecte si donde ha attackado Eric hay enemigos y danyarles.
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Vector3 direction = hit.point - transform.position;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, rotation.eulerAngles.y, 0f), Time.deltaTime * smoothTimeLookAtMouse);
                }
                _MossiState = MossiCharacterState.Idle;
            break;

            case MossiCharacterState.Dying:
            break; 

            default:
            break;
        }
    }
}
