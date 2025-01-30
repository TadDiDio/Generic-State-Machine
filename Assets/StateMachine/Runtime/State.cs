namespace GenericStateMachine
{
    public abstract class State
    {
        /// <summary>
        /// Called every frame while in this state.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Called when this state is entered.
        /// </summary>
        public abstract void OnEnter();

        /// <summary>
        /// Called when this state is exited.
        /// </summary>
        public abstract void OnExit();
    }
}