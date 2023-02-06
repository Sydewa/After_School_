using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AntiaCharacterState { Idle, Running, Dying, Attack, AbilityStart, Reload }
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

    [Header ("Attack")]

    [SerializeField]float reloadTime;
    [SerializeField]float maxShootingTime;
    float waterTankAmount;
    [SerializeField]float maxWaterTankAmount;
    [SerializeField]AnimationCurve waterLength;
    [SerializeField]AnimationCurve waterIconCurve;
    [SerializeField]MunicionAntia _municionAntia;
    bool isReloaded;
    float shootingTime;
    float elapsedTimeReload;
    public BoxCollider cube;
    public Transform endPoint;


    [Header ("Dash")]

    [SerializeField]float dashForce;
    [SerializeField]float dashDuration;
    [SerializeField]float dashLength;

    //-----------------------------------------------------------
    private AntiaCharacterState _AntiaState;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        isReloaded = true;
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
                isOnAction = false;
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
                    timePassed = 0f;
                }
            break;

            case AntiaCharacterState.Attack:
                Attack();
            break;

            case AntiaCharacterState.Reload:
                StartCoroutine(TriggerReload());
                _AntiaState = AntiaCharacterState.Idle;
            break;

            case AntiaCharacterState.AbilityStart:
                StartCoroutine(Dash());
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
    }

    void Attack()
    {

        if(Input.GetButton("Fire1"))
        {
            //Miramos al puntero
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 direction = hit.point - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, rotation.eulerAngles.y, 0f), Time.deltaTime * smoothTimeLookAtMouse);
            }
            //Nos movemos lentamente
            controller.Move(move.normalized * (antiaStats.speed/4.5f) * Time.deltaTime);

            //-------------------------------------------
            ShootingLogic();

        }
        else
        {
            cube.transform.localScale = new Vector3(0f, 0f, 0f);
            _AntiaState = AntiaCharacterState.Idle;
        }
    }

    void ShootingLogic()
    {
        shootingTime += Time.deltaTime;
        float rayLength = waterLength.Evaluate(shootingTime);
        float currentTankAmount = waterIconCurve.Evaluate((maxShootingTime - shootingTime)/2);
        waterTankAmount = currentTankAmount;
        _municionAntia.MunicionDisplay(waterTankAmount);

        if(shootingTime >= maxShootingTime)
        {
            _AntiaState = AntiaCharacterState.Reload;
        }
        cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, rayLength);
        
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, rayLength))
        {
            //Mirar si toca enemigo, calcular danyo y enviarselo al enemigo
            endPoint.position = hit.point;
            float distance = Vector3.Distance(transform.position, hit.point);
            cube.center = new Vector3(0f, 0f, distance);            
        }
    }

    IEnumerator TriggerReload()
    {
        isReloaded = false;
        shootingTime = 0f;
        while(elapsedTimeReload < reloadTime)
        {
            elapsedTimeReload += Time.deltaTime;
            float currentTankAmount = waterIconCurve.Evaluate(elapsedTimeReload);
            waterTankAmount = currentTankAmount;
            _municionAntia.MunicionDisplay(waterTankAmount);
            yield return null;
        }
        elapsedTimeReload = 0f;
        isReloaded = true;
    }

    IEnumerator Dash()
    {
        float elapsedTime = 0f;
        while (elapsedTime <= dashDuration)
        {
            elapsedTime += Time.deltaTime;
            
            if(move == Vector3.zero)
            {
                Vector3 moveDerection= transform.forward*dashLength;
                controller.Move(moveDerection * dashForce * Time.deltaTime);
            }
            else
            {
                Vector3 moveDerection= move*dashLength;
                controller.Move(moveDerection * dashForce * Time.deltaTime);
            }
            yield return _AntiaState = AntiaCharacterState.Idle;
        }
        
    }
    
    void CheckInput()
    {
        bool isDead = false;
        if(PlayerManager.ericVida <= 0)
        {
            isDead = true;
            _AntiaState = AntiaCharacterState.Dying;
        }
        //Si aprietas click izquierdo y el tiempo es mayor que el next attack, que _nextAttack es el tiempo del sistema del ataque anterior + el CD del ataque. 
        if(Input.GetButton("Fire1") && !isOnAction && isReloaded && !isDead)
        {
            isOnAction = true;
            _AntiaState = AntiaCharacterState.Attack;
        }

        if(Input.GetButtonDown("Fire2") && !isDead)
        {
            isOnAction = true;
            _AntiaState = AntiaCharacterState.AbilityStart;
        }

        //Meter los inputs de menu y tal en otro script
        
    }
}
