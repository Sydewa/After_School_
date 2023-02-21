using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafScript : MonoBehaviour
{
    Rigidbody rb;
    public LeafBaseState currentState;

    //States
    public FallingLeafState FallingLeafState = new FallingLeafState();
    
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
        currentState.UpdateState(this.gameObject);
    }

    void StateMachine()
    {
        switch (currentState.GetType().Name)
        {
            case "FallingLeafState":
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

}
