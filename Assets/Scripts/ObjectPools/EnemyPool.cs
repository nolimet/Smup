using Enemies;

namespace ObjectPools
{
    public class EnemyPool : BasePool<Enemy, EnemyPool>
    {
        protected override string BasePath => "Enemies";
    }
}
