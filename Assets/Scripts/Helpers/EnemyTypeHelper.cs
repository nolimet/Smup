using System.IO;
using System.Linq;

namespace Helpers
{
    public class EnemyTypeHelper
    {
        public static string[] GetEnemyTypes() => Directory.GetFiles("Assets/Resources/Enemies", "*.prefab")
            .Select(x => x.Split('/', '\\').Last().Split('.').First()).ToArray();
    }
}
