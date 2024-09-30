using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace BlockGame.Rendering.Shaders
{
    public struct ShaderProgram
    {
        readonly int _graphicsPointer;
        bool _linked;
        readonly UNIFORM[]? _uniforms;

        public readonly int GlPointer => _graphicsPointer;

        public ShaderProgram()
        {
            _graphicsPointer = GL.CreateProgram();
            _linked = false;
            _uniforms = null;
        }

        public void AttachShader(scoped Shader shader)
        {
            if (_linked)
                throw new Exception("Cannot attach a new shader to a linked program.");

            GL.AttachShader(_graphicsPointer, shader._graphicsPointer);

            shader.Delete();
            
        }

        public void Link()
        {
            GL.LinkProgram(_graphicsPointer);

            GL.GetProgram(_graphicsPointer, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(_graphicsPointer);
                throw new Exception($"Error linking program: {infoLog}");
            }
            _linked = true;
        }

        public void SetUniform(string name, Matrix4 matrix)
        {
            int location = GL.GetUniformLocation(GlPointer, name);

            GL.UniformMatrix4(location, true, ref matrix);
        }

        private UNIFORM[] GetUniforms()
        {
            if (_uniforms != null)
                return _uniforms;

            GL.GetProgram(_graphicsPointer, GetProgramParameterName.ActiveUniforms, out int count);
            UNIFORM[] uniforms = new UNIFORM[count];

            for (int i = 0; i < count; i++)
            {
                //NOTE: variable names in shaders are NOT allowed to be bigger than 512 in ANY circumstance.
                GL.GetActiveAttrib(_graphicsPointer, count, 512, out int length, out int size, out ActiveAttribType type, out string name);
                uniforms[i] = new UNIFORM() with { name = name, location = count, type = type };
            }

            return uniforms;
        }
    }

    public record struct UNIFORM
    {
        public string name;
        public ActiveAttribType type;
        public int location;
    }
}
