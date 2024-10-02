using System.Runtime.InteropServices;

namespace ExodiumEngine.Extensions
{
    public static class StreamExtensions
    {
        public static T Read<T>(this Stream stream) where T : struct
        {
            byte[] buffer = new byte[Marshal.SizeOf<T>()];
            stream.ReadExactly(buffer);

            return MemoryMarshal.Read<T>(buffer);
        }

        public static T[] ReadArray<T>(this Stream stream) where T : struct
        {
            int typeLength = Marshal.SizeOf<T>();

            int length = stream.Read<int>();

            if (length * typeLength >= Array.MaxLength)
                throw new InvalidDataException("Invalid array length.");

            Span<byte> buffer = length * typeLength <= 1024
                ? stackalloc byte[length * typeLength]
                : new byte[length * typeLength];

            stream.ReadExactly(buffer);

            T[] values = new T[length];

            Span<T> structSpan = MemoryMarshal.Cast<byte, T>(buffer);
            structSpan.CopyTo(values);

            return values;
        }

        public static void Write<T>(this Stream stream, T value) where T : struct
        {
            Span<byte> bytes = stackalloc byte[Marshal.SizeOf<T>()];
            MemoryMarshal.Write<T>(bytes, value);

            stream.Write(bytes);
        }

        public static void WriteArray<T>(this Stream stream, T[] values) where T : struct
        {
            stream.Write<int>(values.Length);
            ReadOnlySpan<byte> buffer = MemoryMarshal.Cast<T, byte>(values);

            stream.Write(buffer);
        }
    }
}
