using ExodiumEngine.Content;
using ExodiumEngine.OpenGL;
using ExodiumEngine.Rendering;
using System.Buffers;
using System.Numerics;
using System.Runtime.InteropServices;

namespace ExodiumEngine.Extensions
{
    public class ContentPipeLine // create array pooling here.
    {
        private readonly ArrayPool<byte> bytePool = ArrayPool<byte>.Shared;
        private readonly GraphicsDevice _device;
        private readonly IResource _resource;
        public ContentPipeLine(GraphicsDevice device, IResource resource)
        {
            _device = device;
            _resource = resource;
        }
        public Mesh LoadMesh(string filename) // big resources like this should be pooled, Otherwise we get a lot of 3rd level garbage collection... maybe also load textures like this in the mesh itself?
        {
            using (Stream stream = _resource.GetReadStreamFrom(filename))
            {
                int size = stream.Read<int>() * Marshal.SizeOf<Vector3>();

                byte[] vertices = bytePool.Rent(size);
                stream.ReadExactly(vertices, 0, size);

                size = stream.Read<int>() * Marshal.SizeOf<Vector2>();
                byte[] texels = bytePool.Rent(size);
                stream.ReadExactly(texels, 0, size);

                size = stream.Read<int>() * sizeof(uint);
                byte[] indices = bytePool.Rent(size);
                stream.ReadExactly(indices, 0, size);

                Mesh mesh = _device.CreateMesh(vertices, texels, indices);

                bytePool.Return(vertices);
                bytePool.Return(texels);
                bytePool.Return(indices);

                return mesh;
            }
            //using (MemoryStream stream = new(_resource.Fetch(filename)))
            //{
            //    Vector3[] vertices = stream.ReadArray<Vector3>();
            //    Vector2[] texels = stream.ReadArray<Vector2>();
            //    uint[] indices = stream.ReadArray<uint>();

            //    ReadOnlyMemory<byte> verticesBytes = MemoryMarshal.Cast<Vector3, byte>(vertices).ToArray();
            //    ReadOnlyMemory<byte> texelsBytes = MemoryMarshal.Cast<Vector2, byte>(texels).ToArray();
            //    ReadOnlyMemory<byte> indicesBytes = MemoryMarshal.Cast<uint, byte>(indices).ToArray();

            //    return _device.CreateMesh(verticesBytes, texelsBytes, indicesBytes);
            //}
        }

        public Texture2D LoadTexture2D(string filename)
        {
            // should make something better, this works but is poop.
            return new Texture2D(_resource.Fetch(filename), _device);
        }
    }
}
