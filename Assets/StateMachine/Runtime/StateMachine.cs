using System;
using UnityEngine;
using System.Collections.Generic;

namespace GenericStateMachine
{
    public sealed class StateMachine : State
    {
        /// <summary>
        /// The current state.
        /// </summary>
        private State currentState = default;

        /// <summary>
        /// A list of transitions used to determine which state is the current state upon entering the state machine.
        /// </summary>
        private List<Transition> entranceConditions = new();

        /// <summary>
        /// A mapping of state to all transitions leading away from it.
        /// </summary>
        private Dictionary<State, List<Transition>> stateTransitions = new();

        /// <summary>
        /// Returns the current state.
        /// </summary>
        /// <returns>The current state.</returns>
        public State CurrentState() => currentState;

        /// <summary>
        /// Adds a transition to govern changing states.
        /// </summary>
        /// <param name="from">The origin state.</param>
        /// <param name="to">The destination state.</param>
        /// <param name="when">The condition that causes the transition.</param>
        public void AddTransition(State from, State to, Func<bool> when)
        {
            Transition transition = new Transition(when, to);
            
            if (!stateTransitions.ContainsKey(from))
            {
                stateTransitions.Add(from, new List<Transition>());
            }

            stateTransitions[from].Add(transition);
        }

        /// <summary>
        /// Adds a condition to specify which state is the initial state when this state machine is entered.
        /// </summary>
        /// <param name="to">The destination state.</param>
        /// <param name="when">The condition that causes the transition.</param>
        public void AddEntranceCondition(State to, Func<bool> when)
        {
            Transition transition = new Transition(when, to);
            entranceConditions.Add(transition);
        }

        /// <summary>
        /// Use this to initialize the statemachine.
        /// </summary>
        public sealed override void OnEnter() 
        {
            currentState = null;
            foreach (var transition in entranceConditions)
            {
                if (transition.Evaluate())
                {
                    currentState = transition.NextState();
                    break;
                }
            }

            if (currentState != null)
            {
                currentState.OnEnter();
            }
            else
            {
                Debug.LogError("No entrance condition in statemachine evaluated to true resulting in a null initial state!");
            }
        }

        /// <summary>
        /// Updates the state of the state machine.
        /// </summary>
        public sealed override void Update()
        {
            currentState.Update();

            IEnumerable<Transition> transitions = stateTransitions[currentState];

            foreach (var transition in transitions)
            {
                if (transition.Evaluate())
                {
                    currentState.OnExit();
                    currentState = transition.NextState();
                    currentState.OnEnter();
                    break;
                }
            }
        }

        /// <summary>
        /// Use this to leave the statemachine.
        /// </summary>
        public sealed override void OnExit()
        {
            currentState.OnExit();
        }
    }
}