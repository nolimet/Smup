using System.IO;
using System.Linq;

namespace Smup.Helpers
{
    public class EnemyTypeHelper
    {
        public static string[] GetEnemyTypes() => Directory.GetFiles("Assets/Resources/Enemies", "*.prefab")
            .Select(x => x.Split('/', '\\').Last().Split('.').First()).ToArray();
    }
}
