using System;
using System.Buffers.Binary;
using System.IO;

namespace Smup.Util
{
    public static class BytesHelper
    {
        public static void WriteLittleEndianSingle(Span<byte> destination, float value)
        {
            if (destination.Length < 4)
                throw new ArgumentException("Destination must be at least 4 bytes.", nameof(destination));

            var bits = BitConverter.SingleToInt32Bits(value);
            BinaryPrimitives.WriteInt32LittleEndian(destination, bits);
        }

        public static float ReadLittleEndianSingle(ReadOnlySpan<byte> source)
        {
            if (source.Length < 4)
                throw new ArgumentException("Source must be at least 4 bytes.", nameof(source));

            var bits = BinaryPrimitives.ReadInt32LittleEndian(source);
            return BitConverter.Int32BitsToSingle(bits);
        }

        public static void WriteInt32(Stream stream, int value)
        {
            Span<byte> buffer = stackalloc byte[4];
            buffer[0] = (byte)(value & 0xFF);
            buffer[1] = (byte)((value >> 8) & 0xFF);
            buffer[2] = (byte)((value >> 16) & 0xFF);
            buffer[3] = (byte)((value >> 24) & 0xFF);
            stream.Write(buffer);
        }

        public static int ReadInt32(ReadOnlySpan<byte> span, ref int offset)
        {
            var value =
                span[offset] |
                (span[offset + 1] << 8) |
                (span[offset + 2] << 16) |
                (span[offset + 3] << 24);

            offset += 4;
            return value;
        }
    }
}
