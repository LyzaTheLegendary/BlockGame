using OpenTK.Mathematics;

namespace ExodiumEngine.Rendering
{
    public class Camera2D()
    {
        public Matrix4 View = Matrix4.Identity;
        public Matrix4 Projection { get; private set; }
        float m_height;
        float m_width;
        public float AspectRatio { get => m_width / m_height; }
        public void UpdateFov(float height, float width)
        {
            m_height = height;
            m_width = width;

            Projection = Matrix4.CreateOrthographic(width, height, 0.1f, 100f);
        }
        
    }
}
