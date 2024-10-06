using ExodiumEngine.Rendering;
using ExodiumEngine.Rendering.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ExodiumEngine.OpenGL
{
    public class GraphicsDevice
    {
        private int _currProgram = 0;
        private int _currVao = 0;
        private int _currVbo = 0;
        private int _currIbo = 0;

        public Color4 _background = Color4.AliceBlue;
        public int CreateTexture2D(int width, int height, ColorComponents colorComponents, byte[] bitmap)
        {
            int texturePointer = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texturePointer);


            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, bitmap);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

            return texturePointer;
        }

        // Always inline, As we're simply moving memory to the gpu
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Mesh CreateMesh(byte[] vertices, byte[] texelData, byte[] indicesData, BufferUsageHint bufferHint = BufferUsageHint.StaticDraw) {
            int glVertexArrayPointer = GL.GenVertexArray();
            GL.BindVertexArray(glVertexArrayPointer);

            int glVboPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, glVboPointer);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length, vertices, bufferHint);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);

            int glVboTexPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, glVboTexPointer);
            GL.BufferData(BufferTarget.ArrayBuffer, texelData.Length, texelData, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            int glElementArrayPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, glElementArrayPointer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicesData.Length, indicesData, BufferUsageHint.StaticDraw);

            return new Mesh(glVboPointer, glVboTexPointer, glElementArrayPointer, glVertexArrayPointer, indicesData.Length / sizeof(uint));
        }
        public int Create2DPlane(Vector2i position, Vector2i size) // TODO: implement me.
        {


            Span<float> vertices = stackalloc float[]
            {
                                // Positions         // Texture Coords
                -1.0f, -1.0f, 0.0f,  0.0f, 0.0f,  // Bottom-left
                 1.0f, -1.0f, 0.0f,  1.0f, 0.0f,  // Bottom-right
                 1.0f,  1.0f, 0.0f,  1.0f, 1.0f,  // Top-right
                -1.0f,  1.0f, 0.0f,  0.0f, 1.0f   // Top-left
            };

            Span<uint> indices = stackalloc uint[]
            {
                0,1,2,3,0
            };

            Span<float> texels = stackalloc float[]
            {
                0.0f, 0.0f,  // Bottom-left
                1.0f, 0.0f,  // Bottom-right
                1.0f, 1.0f,  // Top-right
                0.0f, 1.0f   // Top-left
            };


            Span<byte> vertexBuffer = stackalloc byte[vertices.Length + Marshal.SizeOf<Vector4i>()];

            void WriteToBuffer(int offset, int length, scoped Span<byte> buffer, Vector2i position)
            {
                unsafe
                {
                    byte* positionPtr = (byte*)&position;
                    for (int i = offset; i < offset + length; i++)
                        buffer[i] = positionPtr[i];
                }
            }




            WriteToBuffer(0, Marshal.SizeOf<Vector2i>(), vertexBuffer, position);
            WriteToBuffer(Marshal.SizeOf<Vector2i>(), Marshal.SizeOf<Vector2i>(), vertexBuffer, size);

            int glVaoPointer = GL.GenVertexArray();
            GL.BindVertexArray(glVaoPointer);

            // layout position -> vertex data -> texel data : for shader

            return 0;
        }
        public void DeleteTexture(int texturePointer) => GL.DeleteTexture(texturePointer);

        public void Render(Renderable renderableObject, Texture2D texture, ShaderProgram program, Camera3D camera)
        {
            UseProgram(program);
            renderableObject.Render(program, camera);
            UseTexture2D(texture);
            Mesh mesh = renderableObject.GetMesh();
            UseMesh(mesh);            
            GL.DrawElements(PrimitiveType.Triangles, mesh.Indices, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        private void UseTexture2D(Texture2D texture, int unit = 0)
        {
            GL.BindTextureUnit(unit, texture.GlPointer);
            GL.BindTexture(TextureTarget.Texture2D, texture.GlPointer);
        }

        private void UseMesh(Mesh mesh)
        {
            if (mesh.VaoPointer != _currVao)
            {
                GL.BindVertexArray(mesh.VaoPointer);
                _currVao = mesh.VaoPointer;
            }

            if (mesh.VboPointer != _currVbo)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, mesh.VboPointer);
                _currVbo = mesh.VboPointer;
            }

            if(mesh.IboPointer != _currIbo)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.IboPointer);
                _currIbo = mesh.IboPointer;
            }

        }

        private void UseProgram(ShaderProgram program)
        {
            if (_currProgram == program.GlPointer)
                return;

            GL.UseProgram(program.GlPointer);
            _currProgram = program.GlPointer;
        }
    }
}
