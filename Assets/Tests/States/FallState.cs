using UnityEngine;
using GenericStateMachine;

public class FallState : State
{
    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override void Update()
    {
        Debug.Log("In Falling state");
    }
}
