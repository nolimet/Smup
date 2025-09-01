using Generic_Objects;

namespace ObjectPools
{
    public class BulletPool : BasePool<BulletGeneric, BulletPool>
    {
        protected override string BasePath => "Weapons";

        public BulletGeneric GetObject(BulletGeneric.Type poolId)
        {
            return GetObject(poolId.ToString());
        }
    }
}
