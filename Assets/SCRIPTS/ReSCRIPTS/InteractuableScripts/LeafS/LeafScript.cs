using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafScript : MonoBehaviour
{
    Rigidbody rb;
    public LeafBaseState currentState;

    //States
    public FallingLeafState FallingLeafState = new FallingLeafState();
    public LeafOnGround LeafOnGround = new LeafOnGround();
    public LeafPushedState LeafPushedState = new LeafPushedState();
    public LeafBurningState LeafBurningState = new LeafBurningState();

    public float maxTimeOnGround;
    float elapsedTimeOnGround = 0f;
    public AnimationCurve onGroundCurve;

    [Header ("Pushed variables")]
    public float pushForce;
    public Vector3 direction;
    public float maxTimePushed;

    [Header ("Burning variables")]
    bool isBurning = false;

    Vector3 boxSize = new Vector3(1f, 10f, 1f);
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentState = FallingLeafState;
    }

    void Start()
    {
        currentState.EnterState(this.gameObject);
    }

    void Update()
    {
        StateMachine();
        currentState.UpdateState(this.gameObject);
    }

    void StateMachine()
    {
        switch (currentState.GetType().Name)
        {
            case "FallingLeafState":
            break;

            case "LeafOnGround":
                elapsedTimeOnGround += Time.deltaTime;
                float leafSize = transform.localScale.x;
                float currentScale = onGroundCurve.Evaluate(elapsedTimeOnGround/maxTimeOnGround);
                transform.localScale = new Vector3(currentScale, leafSize, currentScale);
                if(elapsedTimeOnGround >= maxTimeOnGround)
                {
                    Debug.Log("LeafDestroyed");
                    Destroy(this.gameObject);
                }
            break;

            case "LeafPushedState":
            break;

            case "LeafBurningState":
            break;

            default:
            break;
        }
    }

    public void SwitchState(LeafBaseState state)
    {
        ExitState();
        currentState = state;
        state.EnterState(this.gameObject);
    }

    public void ExitState()
    {
        currentState.ExitState(this.gameObject);
    }

    public void OnLeafPushed(Vector3 directionForce)
    {
        if(isBurning)
        {
            return;
        }
        else
        {
            direction = directionForce;
            SwitchState(LeafPushedState);   
        }
    }

    public void OnLeafBurned()
    {
        isBurning = true;
        SwitchState(LeafBurningState);
    }

    private void OnDrawGizmos() 
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, boxSize);    
    }
}
