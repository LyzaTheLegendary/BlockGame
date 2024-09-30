using OpenTK.Mathematics;

namespace BlockGame.Rendering
{
    public class Camera
    {
        private FovInfo fovInfo;
        private Matrix4 projection;
        public Vector3 Position;
        public Vector3 Front;
        public Vector3 Up;
        public Vector3 Right;

        // The up direction in world space (constant)
        private readonly Vector3 WorldUp;

        // Euler Angles
        public float Yaw;
        public float Pitch;

        // Constructor
        public Camera(float fovy, float height, float width, float depthNear = 0.1f, float depthFar = 100f, float yaw = 0f, float pitch = 0f)
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
            Position = new Vector3(0f, 0f, -3f);
            WorldUp = Vector3.UnitY;
            Yaw = yaw;
            Pitch = pitch;

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

            // Ensure division is done in float to get the correct aspect ratio
            //float aspectRatio = (float)fovInfo.screenWidth / (float)fovInfo.screenHeight;
            float aspectRatio = 1920f / 1080f;

            // Now create the perspective projection using the correct aspect ratio
            projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(fovInfo.fov),
                aspectRatio,
                fovInfo.depthNear,
                fovInfo.depthFar
            );
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