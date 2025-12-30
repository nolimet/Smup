namespace Util.StateMachine.Interfaces
{
    public interface IState
    {
        public IStateMachine StateMachine { get; set; }
        public void OnEnter();
        public void OnExit();
    }
}
