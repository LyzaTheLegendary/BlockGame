using BlockGame.Content;
using BlockGame.OpenGL;
using BlockGame.Rendering;
using System.Numerics;
using System.Runtime.InteropServices;

namespace BlockGame.Extensions
{
    public class ContentPipeLine
    {
        private readonly GraphicsDevice _device;
        private readonly IResource _resource;
        public ContentPipeLine(GraphicsDevice device, IResource resource)
        {
            _device = device;
            _resource = resource;
        }
        public Mesh LoadMesh(string filename) // big resources like this should be pooled, Otherwise we get a lot of 3rd level garbage collection...
        {
            using (MemoryStream stream = new(_resource.Fetch(filename)))
            {
                Vector3[] vertices = stream.ReadArray<Vector3>();
                Vector2[] texels = stream.ReadArray<Vector2>();
                uint[] indices = stream.ReadArray<uint>();

                ReadOnlyMemory<byte> verticesBytes = MemoryMarshal.Cast<Vector3, byte>(vertices).ToArray();
                ReadOnlyMemory<byte> texelsBytes = MemoryMarshal.Cast<Vector2, byte>(texels).ToArray();
                ReadOnlyMemory<byte> indicesBytes = MemoryMarshal.Cast<uint, byte>(indices).ToArray();

                return _device.CreateMesh(verticesBytes, texelsBytes, indicesBytes);
            }
        }

        public Texture2D LoadTexture2D(string filename)
        {
            return new Texture2D(_resource.Fetch(filename), _device);
        }
    }
}
