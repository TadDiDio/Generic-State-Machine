using UnityEngine;
using GenericStateMachine;

public class SprintState : State
{
    public override void OnEnter()
    {
    }

    public override void OnExit()
    {
    }

    public override void Update()
    {
        Debug.Log("In Sprinting state");
    }
}
