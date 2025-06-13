namespace Class2
{
    public abstract class FsmState<T>
    {
        protected T owner;

        public void Initialize (T owner)
        {
            this.owner = owner;
            OnInitialize();
        }

        protected abstract void OnInitialize ();
        
        public abstract void Enter ();
        public abstract void Exit ();
        public abstract void Update ();
    }
}