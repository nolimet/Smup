using Enemies;

namespace ObjectPools
{
    public class EnemyPool : BasePool<EnemyBase, EnemyPool>
    {
        protected override string BasePath => "Enemies";
    }
}
