using System;

namespace GenericStateMachine
{
    public sealed class Transition
    {
        /// <summary>
        /// The next state to transition to.
        /// </summary>
        private State targetState = null;

        /// <summary>
        /// A list of conditions ANDed together to tell if the transition should be taken.
        /// </summary>
        private Func<bool> predicate;

        /// <summary>
        /// Constructs a transition based on a predicate to another state.
        /// </summary>
        /// <param name="predicate">The predicate needed to trigger this transition</param>
        /// <param name="targetState">The state to target.</param>
        public Transition(Func<bool> predicate, State targetState)
        {
            this.predicate = predicate;
            this.targetState = targetState;
        }

        /// <summary>
        /// Tells if this transition should be used.
        /// </summary>
        /// <returns>True if the transition should be used, false otherwise.</returns>
        public bool Evaluate() => predicate();
        
        /// <summary>
        /// Gets the next state
        /// </summary>
        /// <returns></returns>
        public State NextState() => targetState;
    }
}
