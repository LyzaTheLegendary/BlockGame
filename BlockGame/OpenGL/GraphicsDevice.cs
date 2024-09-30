using BlockGame.Rendering;
using BlockGame.Rendering.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using StbImageSharp;

namespace BlockGame.OpenGL
{
    public class GraphicsDevice
    {
        private int _currProgram = 0;
        private int _currVao = 0;
        private int _currVbo = 0;
        private int _currIbo = 0;

        public Color4 _background = Color4.AliceBlue;
        public GraphicsDevice()
        {
            GL.Enable(EnableCap.DepthTest);
        }
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


        // Create one that can do things based on the vbo and text vbo that already exist.
        public Mesh CreateMesh(ReadOnlyMemory<byte> vertices, ReadOnlyMemory<byte> texelData, ReadOnlyMemory<byte> indicesData, BufferUsageHint bufferHint = BufferUsageHint.StaticDraw) {
            int glVertexArrayPointer = GL.GenVertexArray();
            GL.BindVertexArray(glVertexArrayPointer);

            int glVboPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, glVboPointer);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length, vertices.ToArray(), bufferHint);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);


            int glVboTexPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, glVboTexPointer);
            GL.BufferData(BufferTarget.ArrayBuffer, texelData.Length, texelData.ToArray(), BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);

            int glElementArrayPointer = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, glElementArrayPointer);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indicesData.Length, indicesData.ToArray(), BufferUsageHint.StaticDraw);

            return new Mesh(glVboPointer, glVboTexPointer, glElementArrayPointer, glVertexArrayPointer, indicesData.Length / sizeof(uint));
        }

        public int CreateVertexArray3D()
        {
            int glVertexArrayPointer = GL.GenVertexArray();
            GL.BindVertexArray(glVertexArrayPointer);

            // Position attribute
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            // Texture coordinate attribute
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0,0);
            GL.EnableVertexAttribArray(1);
            return glVertexArrayPointer;
        }

        public void DeleteTexture(int texturePointer) => GL.DeleteTexture(texturePointer);

        public void Render(Renderable renderableObject, Texture texture, ShaderProgram program, Camera camera) // 3D camera should be here aswell to render
        {
            UseProgram(program);
            renderableObject.Render(program, camera);
            UseTexture2D(texture);
            Mesh mesh = renderableObject.GetMesh();
            UseMesh(mesh);            
            GL.DrawElements(PrimitiveType.Triangles, mesh.Indices, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        private void UseTexture2D(Texture texture)
        {
            GL.BindTextureUnit(0, texture.GlPointer);
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
