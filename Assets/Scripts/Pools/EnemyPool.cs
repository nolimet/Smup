using Entities.Enemies;

namespace Pools
{
    public class EnemyPool : BasePool<Enemy, EnemyPool>
    {
        protected override string BasePath => "Enemies";
    }
}
