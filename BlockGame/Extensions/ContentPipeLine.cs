using BlockGame.Content;
using BlockGame.OpenGL;
using BlockGame.Rendering;
using System.Numerics;
using System.Runtime.InteropServices;

namespace BlockGame.Extensions
{
    public static class ContentPipeLine
    {
        public static Mesh LoadMesh(this IResource resource, GraphicsDevice device, string filename) // big resources like this should be pooled, Otherwise we get a lot of 3rd level garbage collection...
        {
            using (MemoryStream stream = new(resource.Fetch(filename)))
            {
                Vector3[] vertices = stream.ReadArray<Vector3>();
                Vector2[] texels = stream.ReadArray<Vector2>();
                uint[] indices = stream.ReadArray<uint>();

                ReadOnlyMemory<byte> verticesBytes = MemoryMarshal.Cast<Vector3, byte>(vertices).ToArray();
                ReadOnlyMemory<byte> texelsBytes = MemoryMarshal.Cast<Vector2, byte>(texels).ToArray();
                ReadOnlyMemory<byte> indicesBytes = MemoryMarshal.Cast<uint, byte>(indices).ToArray();

                return device.CreateMesh(verticesBytes, texelsBytes, indicesBytes);
            }
        }

        public static Texture LoadTexture(this IResource resource, GraphicsDevice device, string filename)
        {
            return new Texture(resource.Fetch(filename), device);
        }
    }
}
