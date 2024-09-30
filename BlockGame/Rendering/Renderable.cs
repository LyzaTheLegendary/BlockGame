using BlockGame.Rendering.Shaders;
using OpenTK.Mathematics;

namespace BlockGame.Rendering
{
    public class Renderable // should be a protected class.
    {
        private Matrix4 _transformations;
        private readonly Mesh _mesh;

        //save angles here
        public Renderable(Vector3 location, Mesh mesh) // make this scoped?
        {
            _transformations = Matrix4.CreateTranslation(location) * Matrix4.CreateTranslation(0, 0, -3f);
            _mesh = mesh;
            SetLocation(location);
        }

        public void RotateX(float angle)
        {
            _transformations = Matrix4.CreateRotationX(angle) * Matrix4.CreateTranslation(0, 0, -3f);
        }

        public void RotateY(float angle)
        {
            _transformations = Matrix4.CreateRotationY(angle) * Matrix4.CreateTranslation(0, 0, -3f);
        }

        public void RotateZ(float angle)
        {
            _transformations = Matrix4.CreateRotationZ(angle) * Matrix4.CreateTranslation(0, 0, -3f);
        }

        public void SetRotate(float angleX, float angleY, float angleZ)
        {
            _transformations = Matrix4.CreateRotationZ(angleZ) * Matrix4.CreateRotationY(angleY) * Matrix4.CreateRotationX(angleX) * Matrix4.CreateTranslation(0, 0, -3f);
        }

        public void SetLocation(Vector3 location)
        {    // Extract the rotation part of the matrix (the upper-left 3x3 part)
            Matrix4 rotationMatrix = _transformations.ClearTranslation();

            // Create a new translation matrix
            Matrix4 translationMatrix = Matrix4.CreateTranslation(location);

            // Combine the translation and rotation to form the new transformation matrix
            _transformations = rotationMatrix * translationMatrix;

            Console.WriteLine(_transformations.ToString());
            return;
            //Matrix4 rotationMatrix = _transformations.ClearTranslation();

            //// Apply the new translation (position) while keeping the rotation intact
            //_transformations = Matrix4.CreateTranslation(location) * rotationMatrix;
            //return;
            //Matrix4 translationMatrix = Matrix4.CreateTranslation(location);

            //// Apply the translation to the existing transformations (if any)
            //_transformations = translationMatrix * _transformations;
            //Console.WriteLine(_transformations.ToString());
        }

        public void Render(ShaderProgram program, Camera camera)
        {
            program.SetUniform("model", _transformations);
            //program.SetUniform("view", Matrix4.Identity); //TODO: fix view
            program.SetUniform("view", camera.GetViewMatrix()); //TODO: FIX ME I AM BROKEN CYKA
            program.SetUniform("projection", camera.GetProjectionMatrix());
        }

        public Mesh GetMesh() => _mesh;
    }
}
