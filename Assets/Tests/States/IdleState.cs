using UnityEngine;
using GenericStateMachine;

public class IdleState : State
{
    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override void Update()
    {
        Debug.Log("In Idle state");
    }
}
