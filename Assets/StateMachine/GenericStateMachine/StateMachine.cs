using System;
using UnityEngine;
using System.Collections.Generic;

namespace GenericStateMachine
{
    public sealed class StateMachine : State
    {
        /// <summary>
        /// The name of this statemachine.
        /// </summary>
        private string name;

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
        /// Makes a new statemachine.
        /// </summary>
        /// <param name="name">The name of this state machine.</param>
        public StateMachine(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// Returns the name of this state machine.
        /// </summary>
        /// <returns>The name of this state machine.</returns>
        public override string ToString()
        {
            return name;
        }

        /// <summary>
        /// Returns the current state.
        /// </summary>
        /// <returns>The current state.</returns>
        public State CurrentState() => currentState;

        /// <summary>
        /// Gets a formatted string of the current base state.
        /// </summary>
        /// <returns>A formatted string of the base most state of this state machine.</returns>
        public string CurrentStateLog()
        {
            if (currentState is StateMachine)
            {
                return $"[{ToString()}]->{((StateMachine)currentState).CurrentStateLog()}";
            }
            return currentState.ToString();
        }

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
            if (currentState == null)
            {
                Debug.LogError($"The current state of {ToString()} is null. Did you call OnEnter on the top level statemachine before calling Update?");
            }
            else
            {
                currentState.Update();
            }

            if (!stateTransitions.TryGetValue(currentState, out List<Transition> transitions))
            {
                Debug.LogWarning($"There are no transitions leading away from {currentState.ToString()}. This is probably unintentional as this state will never be left within this level of the state machine.");
            }

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