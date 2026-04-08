using System;
using System.Buffers.Binary;
using Sirenix.OdinInspector;
using Smup.Helpers;
using Smup.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Smup.World.Waves.Data
{
    [Serializable]
    public class SpawnPatternPart
    {
        [FormerlySerializedAs("enemyId")] [ValueDropdown(nameof(Enemies))] public string typeName;
        public Vector2[] positions;

        private string[] Enemies => EnemyTypeHelper.GetEnemyTypes();
    }

    public readonly struct SpawnGroup
    {
        public readonly Vector2Int GridSize;
        public readonly float GridStepSize;

        public readonly SpawnSection[] Sections;

        public SpawnGroup(Vector2Int gridSize, float gridStepSize, SpawnSection[] sections)
        {
            GridSize = gridSize;
            GridStepSize = gridStepSize;
            Sections = sections;
        }

        public SpawnGroup(ReadOnlySpan<byte> buffer)
        {
            GridSize = new Vector2Int(x: BinaryPrimitives.ReadInt32LittleEndian(buffer), y: BinaryPrimitives.ReadInt32LittleEndian(buffer[4..]));
            GridStepSize = BytesHelper.ReadLittleEndianSingle(buffer[8..]);

            var sectionsCount = (buffer.Length - 12) / SpawnSection.ByteSize;
            Sections = new SpawnSection[sectionsCount];
            for (var i = 0; i < sectionsCount; i++)
                Sections[i] = new SpawnSection(buffer[(12 + i * SpawnSection.ByteSize)..]);
        }

        public byte[] ToBytes()
        {
            var bytes = new byte[GetByteLength()];

            BinaryPrimitives.WriteInt32LittleEndian(bytes.AsSpan(0, 4), GridSize.x);
            BinaryPrimitives.WriteInt32LittleEndian(bytes.AsSpan(4, 4), GridSize.y);
            BytesHelper.WriteLittleEndianSingle(bytes.AsSpan(8, 4), GridStepSize);

            var sectionsOffset = 12;
            foreach (var section in Sections)
            {
                section.ToBytes(bytes.AsSpan(sectionsOffset, SpawnSection.ByteSize));
                sectionsOffset += SpawnSection.ByteSize;
            }

            return bytes;
        }

        public int GetByteLength() => Sections.Length * SpawnSection.ByteSize + 3 * 4;
    }

    public readonly struct SpawnSection
    {
        public const int ByteSize = 3;

        public readonly byte Id;
        public readonly short Length;

        public SpawnSection(byte id, short length)
        {
            Id = id;
            Length = length;
        }

        public SpawnSection(ReadOnlySpan<byte> buffer)
        {
            if (buffer.Length < ByteSize)
                throw new ArgumentException("Not enough bytes.", nameof(buffer));

            Id = buffer[0];
            Length = BinaryPrimitives.ReadInt16LittleEndian(buffer[1..]);
        }

        public void ToBytes(Span<byte> buffer)
        {
            if (buffer.Length < ByteSize)
                throw new ArgumentException("Not enough bytes.", nameof(buffer));

            buffer[0] = Id;
            BinaryPrimitives.WriteInt16LittleEndian(buffer[1..], Length);
        }
    }
}
