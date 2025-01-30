using System;
using UnityEngine;
using GenericStateMachine;
using static InputManager;

public class PlayerController : MonoBehaviour
{
    private StateMachine playerStateMachine;
    private void Awake()
    {
        IdleState idleState = new();
        WalkState walkState = new();
        SprintState sprintState = new();
        
        StateMachine groundMachine = new();

        Func<bool> toIdle   = () => DirectionalInput() == Vector2.zero;
        Func<bool> toWalk   = () => DirectionalInput() != Vector2.zero && (!SprintKey() || DirectionalInput().y < 1);
        Func<bool> toSprint = () => DirectionalInput().y > 0 && SprintKey();

        groundMachine.AddTransition(idleState, walkState, toWalk);
        groundMachine.AddTransition(idleState, sprintState, toSprint);
        groundMachine.AddTransition(walkState, idleState, toIdle);
        groundMachine.AddTransition(sprintState, idleState, toIdle);
        groundMachine.AddTransition(walkState, sprintState, toSprint);
        groundMachine.AddTransition(sprintState, walkState, toWalk);

        groundMachine.AddEntranceCondition(idleState, toIdle);
        groundMachine.AddEntranceCondition(walkState, toWalk);
        groundMachine.AddEntranceCondition(sprintState, toSprint);

        FallState fallState = new();
        GrappleState grappleState = new();

        StateMachine airMachine = new();

        Func<bool> toFall    = () => !GrappleKey();
        Func<bool> toGrapple = () => GrappleKey();

        airMachine.AddTransition(fallState, grappleState, toGrapple);
        airMachine.AddTransition(grappleState, fallState, toFall);

        airMachine.AddEntranceCondition(fallState, toFall);
        airMachine.AddEntranceCondition(grappleState, toGrapple);

        playerStateMachine = new();

        playerStateMachine.AddTransition(groundMachine, airMachine, () => !grounded);
        playerStateMachine.AddTransition(airMachine, groundMachine, () => grounded);

        playerStateMachine.AddEntranceCondition(groundMachine, () => true);
        playerStateMachine.OnEnter();
    }

    bool grounded = true;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) grounded = !grounded;

        playerStateMachine.Update();
    }
}
