using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Sirenix.OdinInspector;
using Smup.Util;
using UnityEditor;
using UnityEngine;
using CompressionLevel = System.IO.Compression.CompressionLevel;

namespace Smup.World.Waves.Data
{
    public class SpawnPattern : ScriptableObject, ISerializationCallbackReceiver
    {
        public List<SpawnPatternPart> parts = new();
        [ShowInInspector] public List<SpawnGroup> Groups = new();

        [SerializeField] private byte[] data;

        [MenuItem("TEST/Create/SpawnPattern Test")]
        public static void CreateTestFile()
        {
            var instance = CreateInstance<SpawnPattern>();
            instance.Groups = new List<SpawnGroup>
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

            AssetDatabase.CreateAsset(instance, "Assets/SpawnPattern-Test.asset");
        }

        public void OnAfterDeserialize()
        {
            Groups.Clear();

            if (data is not { Length: 0 }) return;

            var span = Decompress(data).AsSpan();
            var offset = 0;
            var groupCount = BytesHelper.ReadInt32(span, ref offset);
            for (var i = 0; i < groupCount; i++)
            {
                var length = BytesHelper.ReadInt32(span, ref offset);
                Groups.Add(new SpawnGroup(span[offset..]));
                offset += length;
            }
        }

        public void OnBeforeSerialize()
        {
            using var memoryStream = new MemoryStream();
            BytesHelper.WriteInt32(memoryStream, Groups.Count);
            for (var i = 0; i < Groups.Count; i++)
            {
                var bytes = Groups[i].ToBytes();
                BytesHelper.WriteInt32(memoryStream, bytes.Length);
                memoryStream.Write(bytes);
            }

            data = Compress(memoryStream.ToArray());
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
