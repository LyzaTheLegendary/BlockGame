using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using static System.Net.Mime.MediaTypeNames;

namespace ExodiumEngine.Rendering
{
    public class Camera3D
    {
        private FovInfo fovInfo;
        private Matrix4 projection;
        

        public Vector3 Front;
        public Vector3 Up;
        public Vector3 Right;
        // The up direction in world space (constant)
        private readonly Vector3 WorldUp;

        public Vector3 Position;
        private Vector2? lastPos;


        // Euler Angles
        public float Yaw;
        public float Pitch;

        // camera settings
        private float SPEED = 8f;
        private float SENSITIVITY = 180f;

        // Constructor
        public Camera3D(float fovy, float height, float width, float depthNear = 0.1f, float depthFar = 100f, float yaw = 0f, float pitch = 0f)
        {
            fovInfo = new FovInfo
            {
                fov = fovy,
                depthFar = depthFar,
                depthNear = depthNear,
                screenHeight = height,
                screenWidth = width
            };

            UpdateFov(height, width);
            Position = new Vector3(0f, 0f, 0f);
            WorldUp = Vector3.UnitY;
            Yaw = yaw;
            Pitch = pitch;
            lastPos = null;
            Front = new Vector3(0.0f, 0.0f, -1.0f);
            UpdateCameraVectors();
        }

        public Matrix4 GetProjectionMatrix() => projection;

        public Matrix4 GetViewMatrix()
            => Matrix4.LookAt(Position, Position + Front, Up);

        public void UpdateFov(float height, float width)
        {
            fovInfo.screenHeight = height;
            fovInfo.screenWidth = width;

            float aspectRatio = 1920f / 1080f; // TODO figure out why it kills itself when it's done using the dynamic height and width.

            // Now create the perspective projection using the correct aspect ratio
            projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(fovInfo.fov),
                aspectRatio,
                fovInfo.depthNear,
                fovInfo.depthFar
            );
        }
        public void InputController(KeyboardState input, MouseState mouse, double time) // bug it should not be able to look higher than 180 degrees for obvious reasons.
        {

            if (input.IsKeyDown(Keys.W))
            {
                Position += Front * SPEED * (float)time;
            }
            if (input.IsKeyDown(Keys.A))
            {
                Position -= Right * SPEED * (float)time;
            }
            if (input.IsKeyDown(Keys.S))
            {
                Position -= Front * SPEED * (float)time;
            }
            if (input.IsKeyDown(Keys.D))
            {
                Position += Right * SPEED * (float)time;
            }

            if (input.IsKeyDown(Keys.Space))
            {
                Position.Y += SPEED * (float)time;
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                Position.Y -= SPEED * (float)time;
            }

            if (lastPos == null)
            {
                lastPos = new Vector2(mouse.X, mouse.Y);
            }
            else
            {
                float deltaX = mouse.X - lastPos.Value.X;
                float deltaY = mouse.Y - lastPos.Value.Y;
                lastPos = new Vector2(mouse.X, mouse.Y);

                Yaw += deltaX * SENSITIVITY * (float)time;
                Pitch -= deltaY * SENSITIVITY * (float)time;
            }
            UpdateCameraVectors();
        }
        public void UpdateCameraVectors()
        {
            float yawRad = MathHelper.DegreesToRadians(Yaw);
            float pitchRad = MathHelper.DegreesToRadians(Pitch);

            // Calculate the new Front vector
            Front.X = MathF.Cos(yawRad) * MathF.Cos(pitchRad);
            Front.Y = MathF.Sin(pitchRad);
            Front.Z = MathF.Sin(yawRad) * MathF.Cos(pitchRad);
            Front = Vector3.Normalize(Front);

            // Recalculate the Right and Up vectors
            Right = Vector3.Normalize(Vector3.Cross(Front, WorldUp));
            Up = Vector3.Normalize(Vector3.Cross(Right, Front));
        }
    }

    public record struct FovInfo
    {
        public float fov;
        public float screenHeight;
        public float screenWidth;
        public float depthNear;
        public float depthFar;
    }
}