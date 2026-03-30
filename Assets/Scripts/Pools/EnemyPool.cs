using Smup.Entities.Enemies;

namespace Smup.Pools
{
    public class EnemyPool : BasePool<Enemy, EnemyPool>
    {
        protected override string BasePath => "Enemies";
    }
}
