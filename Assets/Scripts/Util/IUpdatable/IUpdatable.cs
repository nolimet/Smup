namespace Util.IUpdatable
{
    public interface IUpdatable
    {
        //Update called once a frame
        void Update();
    }

    public interface IFixedUpdateble
    {
        //Update called once every physics update
        void FixedUpdate();
    }

    public interface IContinuesUpdateAble
    {
        //game state independent update. Does not react to pause
        void ContinuesUpdate();
    }
}
