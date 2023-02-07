using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoraCharacterState { Idle, Running, Dying, Attack, Ability, Ultimate }
public class Sora_Movement : MonoBehaviour
{
    private CharacterController controller;
    [SerializeField]Stats soraStats;
    //private Animator anim;
    
    //Variables de movimiento
    Vector3 velocity = new Vector3(0f, -9.81f, 0f);
    Vector3 move;
    [SerializeField]float smoothTimeMove;
    [SerializeField]float smoothTimeLookAtMouse;
    [SerializeField]float accelerationTime;
    float timePassed;

    bool isOnAction;

    [Header ("Attack")]
    [SerializeField][Range(0,360)]float attackAngle;
    [SerializeField]float attackRadius;
    [SerializeField]LayerMask enemyLayer;
    float elapsedTimeAttack;

    [Header ("Ability")]
    [SerializeField]float abilityRadius;
    float _nextAbility;

    [Header ("Ultimate")]
    [SerializeField]float ultimateDuration;
    Vector3 ultimatePosition;
    float elapsedTimeUltimate;
    float elapsedTimeUltimatePush;

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
        controller.Move(velocity * Time.deltaTime);
        move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
    }

    public void AntiaStates()
    {
        CheckInput();

        switch(_SoraState)
        {
            case SoraCharacterState.Idle:
                //anim.SetBool("Run", false);
                if(move != Vector3.zero)
                {
                    _SoraState = SoraCharacterState.Running;
                }
                
            break;

            case SoraCharacterState.Running: 
                
                //anim.SetBool("Run", true);
                Running();
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
                Attack();
            break;

            case SoraCharacterState.Ability:
                Ability();
            break;

            case SoraCharacterState.Ultimate:
                ultimatePosition = transform.position;
                StartCoroutine(Ultimate());
                _SoraState = SoraCharacterState.Idle;
            break;

            case SoraCharacterState.Dying:
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

        float currentSpeed = Mathf.Lerp(0, soraStats.speed, x);
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
            controller.Move(move.normalized * (soraStats.speed/4.5f) * Time.deltaTime);

            //-------------------------------------------
            ShootingLogic();
            //VisualizeAttack();

        }
        else
        {
            isOnAction = false;
            _SoraState = SoraCharacterState.Idle;
        }
        
    }

    void ShootingLogic()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRadius, enemyLayer);
        //Debug.Log("Shoot");
        elapsedTimeAttack += Time.deltaTime;
        if(elapsedTimeAttack >= soraStats.attackSpeed)
        {
            foreach (Collider collider in colliders)
            {
                //get the direction from the character to the enemy
                Vector3 direction = (collider.transform.position - ultimatePosition).normalized;

                //if the dot product is within the angle, the enemy is within the cone of attack
                if (Physics.CheckSphere(ultimatePosition, attackRadius))
                {
                    //do something to the enemy, such as damaging it
                    //Debug.Log("Enemy in range: " + collider.gameObject.name);
                    EnemyDamaged _enemyDamaged = collider.GetComponent<EnemyDamaged>();
                    float distance = Vector3.Distance(transform.position, collider.transform.position) + 1f;
                    if(_enemyDamaged != null)
                    {
                        _enemyDamaged.OnEnemyDamaged(Mathf.CeilToInt(soraStats.attack/distance));
                        _enemyDamaged.OnEnemyPushed(soraStats.pushForce/distance, direction);
                        Debug.Log(Mathf.CeilToInt(soraStats.attack/distance));
                    }
                }
            }
            elapsedTimeAttack = 0f;
        }
        
    }

    void Ability()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, abilityRadius, enemyLayer);
        foreach (Collider collider in colliders)
        {
            //get the direction from the character to the enemy
            Vector3 direction = (collider.transform.position - transform.position).normalized;
            //do something to the enemy, such as damaging it
            EnemyDamaged _enemyDamaged = collider.GetComponent<EnemyDamaged>();
            float distance = Vector3.Distance(transform.position, collider.transform.position) + 1f;
            if(_enemyDamaged != null)
            {
                _enemyDamaged.OnEnemyDamaged(Mathf.CeilToInt((soraStats.power + soraStats.attack)/distance));
                _enemyDamaged.OnEnemyPushed(soraStats.pushForce2 * abilityRadius/distance, direction);
                Debug.Log(Mathf.CeilToInt((soraStats.power + soraStats.attack)/distance));
            }
            
        }
        _nextAbility = Time.time + soraStats.abilityCD;
        _SoraState = SoraCharacterState.Idle;
        isOnAction = false;
    }

    IEnumerator Ultimate()
    {
        Collider[] colliders = Physics.OverlapSphere(ultimatePosition, abilityRadius, enemyLayer);
        
        while(elapsedTimeUltimate < ultimateDuration)
        {
            elapsedTimeUltimate += Time.deltaTime;
            elapsedTimeUltimatePush += Time.deltaTime;
            foreach (Collider collider in colliders)
            {
                //get the direction from the character to the enemy
                Vector3 direction = (transform.position-collider.transform.position).normalized;
                //do something to the enemy, such as damaging it
                EnemyDamaged _enemyDamaged = collider.GetComponent<EnemyDamaged>();
                float distance = Vector3.Distance(transform.position, collider.transform.position) + 1f;
                if(_enemyDamaged != null && elapsedTimeUltimatePush > soraStats.attackSpeed)
                {
                    elapsedTimeUltimatePush = 0f;
                    //_enemyDamaged.OnEnemyDamaged(Mathf.CeilToInt((soraStats.power + soraStats.attack)/distance));
                    _enemyDamaged.OnEnemyPushed(soraStats.pushForce2 * abilityRadius, direction);
                    //Debug.Log(Mathf.CeilToInt((soraStats.power + soraStats.attack)/distance));
                }
                
            }
            yield return null;
        }
        
        //_nextAbility = Time.time + soraStats.abilityCD;
        //_SoraState = SoraCharacterState.Idle;
        //isOnAction = false;
    }

    void CheckInput()
    {
        bool isDead = false;
        if(PlayerManager.soraVida <= 0)
        {
            isDead = true;
            _SoraState = SoraCharacterState.Dying;
            
        }
        //Si aprietas click izquierdo y el tiempo es mayor que el next attack, que _nextAttack es el tiempo del sistema del ataque anterior + el CD del ataque. 
        if(Input.GetButton("Fire1") && !isOnAction && !isDead)
        {
            isOnAction = true;
            _SoraState = SoraCharacterState.Attack;
        }

        if(Input.GetButtonDown("Fire2") && !isDead && Time.time > _nextAbility)
        {
            isOnAction = true;
            _SoraState = SoraCharacterState.Ability;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            elapsedTimeUltimate = 0f;
            elapsedTimeUltimatePush = 0f;
            
            _SoraState = SoraCharacterState.Ultimate;
        }

        //Meter los inputs de menu y tal en otro script
    }

    void VisualizeAttack()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        Vector3 left = transform.TransformDirection(Vector3.left);

        Vector3 leftAttackLimit = Quaternion.AngleAxis(-attackAngle, Vector3.up) * (-forward);
        Vector3 rightAttackLimit = Quaternion.AngleAxis(attackAngle, Vector3.up) * (-forward);

        Vector3 leftAttackEnd = transform.position + leftAttackLimit * attackRadius;
        Vector3 rightAttackEnd = transform.position + rightAttackLimit * attackRadius;

        Debug.DrawLine(transform.position, leftAttackEnd, Color.red);
        Debug.DrawLine(transform.position, rightAttackEnd, Color.red);
    }

    void OnDrawGizmosSelected()
    {
        VisualizeAttack();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,abilityRadius);
    }
}
