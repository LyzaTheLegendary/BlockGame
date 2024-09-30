using BlockGame.Content;
using BlockGame.Extensions;
using BlockGame.OpenGL;
using BlockGame.Rendering;
using BlockGame.Rendering.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Text;

namespace BlockGame
{
    public class Game : GameWindow
    {
        int vertexArray3D;
        int vertexArray2D;
        GraphicsDevice device;
        IResource resource = new RawResource();
        Camera camera;
        Renderable block;
        Texture dirtTexture;
        
        ShaderProgram blockProgram;

        float xAxis = 0f;
        float yAxis = 0f;
        float angle = 0f;
        public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default) {
            Title = "BlockGame";
            camera = new Camera(45f, 1920,1080);
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.Enable(EnableCap.DepthTest | EnableCap.CullFace);

            device = new GraphicsDevice();
            Shader vertexShader = new Shader(ShaderType.VertexShader);
            Shader fragmentShader = new Shader(ShaderType.FragmentShader);

            vertexShader.Compile(Encoding.UTF8.GetString(resource.Fetch("block.vert")));
            fragmentShader.Compile(Encoding.UTF8.GetString(resource.Fetch("block.frag")));

            blockProgram = new ShaderProgram();

            blockProgram.AttachShader(vertexShader);
            blockProgram.AttachShader(fragmentShader);

            blockProgram.Link();

            Mesh cube = resource.LoadMesh(device, "cube.model");

            block = new(new Vector3(0, 0, 0), cube);
            dirtTexture = resource.LoadTexture(device, "dirt.png");
            
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            camera.UpdateFov(e.Width, e.Height);
        }
        protected override void OnUpdateFrame(FrameEventArgs frameArgs)
        {
            //cube.transformation = Matrix4.CreateRotationY(angle += 0.0001f) * Matrix4.CreateRotationX(angle += 0.0001f) * Matrix4.CreateTranslation(0, 0, -3f);
            block.SetRotate(angle, angle, angle);
            block.SetLocation(new Vector3(xAxis, yAxis, 0));
            //block.RotateY(angle);
            //block.RotateX(angle);
            //block.RotateZ(angle);
            xAxis += 0.0002f;
            yAxis += 0.0001f;
            angle += 0.0001f;
            camera.UpdateCameraVectors();
            base.OnUpdateFrame(frameArgs);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            
            GL.ClearColor(device._background);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            device.Render(block, dirtTexture, blockProgram, camera);
            //Console.WriteLine(GL.GetError());
            Context.SwapBuffers();


            base.OnRenderFrame(args);
        }
    }
}
    