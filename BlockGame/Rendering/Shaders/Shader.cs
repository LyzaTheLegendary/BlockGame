using OpenTK.Graphics.OpenGL4;
using System;

namespace BlockGame.Rendering.Shaders
{
    public ref struct Shader
    {
        public readonly ShaderType _type;
        public readonly int _graphicsPointer;

        public Shader(ShaderType type)
        {
            _type = type;
            _graphicsPointer = GL.CreateShader(type);
        }
        public string? Compile(string shaderCode)
        {
            GL.ShaderSource(_graphicsPointer, shaderCode);
            GL.CompileShader(_graphicsPointer);

            GL.GetShader(_graphicsPointer, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(_graphicsPointer);
                return infoLog;
            }

            return null;
        }

        public void Delete() => GL.DeleteShader(_graphicsPointer);

        
    }
}
