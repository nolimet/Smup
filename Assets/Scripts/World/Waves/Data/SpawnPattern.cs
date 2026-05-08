using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Smup.Util;
using UnityEditor;
using UnityEngine;
using CompressionLevel = System.IO.Compression.CompressionLevel;

namespace Smup.World.Waves.Data
{
    [CreateAssetMenu(fileName = "SpawnPattern", menuName = "Smup/Spawn Pattern")]
    public class SpawnPattern : SerializedScriptableObject
    {
        public IReadOnlyList<SpawnGroup> Groups => _groups;
        public string PatternID => _patternID;

        [SerializeField] private string _patternID;
        [OdinSerialize] private List<SpawnGroup> _groups = new();

        [MenuItem("TEST/Create/SpawnPattern Test")]
        public static void CreateTestFile()
        {
            var groups = new List<SpawnGroup>
            {
                new(new Vector2Int(10, 10), 1f, new SpawnSection[]
                {
                    new(1, 10),
                    new(1, 1), new(0, 8), new(1, 1),
                    new(1, 1), new(0, 8), new(1, 1),
                    new(1, 1), new(0, 8), new(1, 1),
                    new(1, 1), new(0, 8), new(1, 1),
                    new(1, 1), new(0, 8), new(1, 1),
                    new(1, 1), new(0, 8), new(1, 1),
                    new(1, 1), new(0, 8), new(1, 1),
                    new(1, 1), new(0, 8), new(1, 1),
                    new(1, 10)
                }),
                new(new Vector2Int(10, 10), 1f, new SpawnSection[]
                {
                    new(1, 100)
                })
            };

            var path = Path.Combine(Application.dataPath, "SpawnPattern-Test.w");
            File.WriteAllBytes(path, ExportBytes(groups));
        }

        public void ImportBytes(byte[] bytes)
        {
            _groups.Clear();

            if (bytes is { Length: 0 }) return;

            var span = Decompress(bytes).AsSpan();
            var offset = 0;
            var groupCount = BytesHelper.ReadInt32(span, ref offset);
            for (var i = 0; i < groupCount; i++)
            {
                var length = BytesHelper.ReadInt32(span, ref offset);
                _groups.Add(new SpawnGroup(span[offset..]));
                offset += length;
            }
        }

        public static byte[] ExportBytes(IReadOnlyList<SpawnGroup> groups)
        {
            using var memoryStream = new MemoryStream();
            BytesHelper.WriteInt32(memoryStream, groups.Count);
            for (var i = 0; i < groups.Count; i++)
            {
                var bytes = groups[i].ToBytes();
                BytesHelper.WriteInt32(memoryStream, bytes.Length);
                memoryStream.Write(bytes);
            }

            return Compress(memoryStream.ToArray());
        }

        private static byte[] Compress(byte[] input)
        {
            using var output = new MemoryStream();
            using (var gzip = new GZipStream(output, CompressionLevel.Optimal, leaveOpen: true))
            {
                gzip.Write(input, 0, input.Length);
            }

            return output.ToArray();
        }

        private static byte[] Decompress(byte[] input)
        {
            using var inputStream = new MemoryStream(input);
            using var gzip = new GZipStream(inputStream, CompressionMode.Decompress);
            using var output = new MemoryStream();
            gzip.CopyTo(output);
            return output.ToArray();
        }
    }
}
