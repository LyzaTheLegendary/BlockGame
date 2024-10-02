﻿using ExodiumEngine.Rendering.Shaders;
using OpenTK.Mathematics;

namespace ExodiumEngine.Rendering
{
    public class Renderable // should be a protected class.
    {
        private Matrix4 _transformations;
        private readonly Mesh _mesh;
        private readonly Texture2D m_texture;

        //save angles here
        public Renderable(Vector3 location, Mesh mesh, Texture2D texture) // make this scoped?
        {
            _transformations = Matrix4.CreateTranslation(location);
            _mesh = mesh;
            //SetLocation(location);
            m_texture = texture;
        }

        ~Renderable() => _mesh.Delete();

        public void RotateX(float angle)
        {
            _transformations = Matrix4.CreateRotationX(angle) * Matrix4.CreateTranslation(0, 0, 0);
        }

        public void RotateY(float angle)
        {
            _transformations = Matrix4.CreateRotationY(angle) * Matrix4.CreateTranslation(0, 0, 0);
        }

        public void RotateZ(float angle)
        {
            _transformations = Matrix4.CreateRotationZ(angle) * Matrix4.CreateTranslation(0, 0, 0);
        }

        public void SetRotation(float angleX, float angleY, float angleZ)
        {
            _transformations = Matrix4.CreateRotationZ(angleZ) * Matrix4.CreateRotationY(angleY) * Matrix4.CreateRotationX(angleX) * Matrix4.CreateTranslation(0, 0, -3f);
        }

        public void SetLocation(Vector3 location) // camera is broken maybe?
        {
            Matrix4 rotationMatrix = _transformations.ClearTranslation();
            Matrix4 translationMatrix = Matrix4.CreateTranslation(location);

            _transformations = rotationMatrix * translationMatrix;

            //Console.WriteLine(_transformations.ToString());
            return;
        }

        public void Render(ShaderProgram program, Camera3D camera)
        {
            program.SetUniform("model", _transformations);
            Console.WriteLine(_transformations.ToString());
            program.SetUniform("view", camera.GetViewMatrix());
            program.SetUniform("projection", camera.GetProjectionMatrix());
        }

        public Mesh GetMesh() => _mesh;
        public Texture2D GetTexture2D() => m_texture;
    }
}
