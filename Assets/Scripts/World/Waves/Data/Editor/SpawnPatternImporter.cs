using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Smup.World.Waves.Data.Editor
{
    [ScriptedImporter(1, "w", AllowCaching = true)]
    public class SpawnPatternImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var bytes = File.ReadAllBytes(ctx.assetPath);

            var instance = ScriptableObject.CreateInstance<SpawnPattern>();
            instance.name = Path.GetFileNameWithoutExtension(ctx.assetPath.Trim('.'));

            instance.ImportBytes(bytes);

            ctx.AddObjectToAsset("data", instance);
            ctx.SetMainObject(instance);
        }
    }
}
