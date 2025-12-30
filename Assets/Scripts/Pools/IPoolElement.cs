namespace ObjectPools
{
    public interface IPoolElement
    {
        public string PoolId { get; }
        void OnSpawn();
        void OnDespawn();
    }
}
