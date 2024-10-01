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
        GraphicsDevice device;
        Camera3D camera3D;
        IResource resource = new RawResource();
        ContentPipeLine content;
        ShaderProgram blockProgram;


        Renderable mesh;
        Texture2D dirtTexture;

        float xAxis = 0f;
        float yAxis = 0f;
        float angle = 0f;
        public Game() : base(GameWindowSettings.Default, NativeWindowSettings.Default) {
            Title = "BlockGame";
            camera3D = new Camera3D(45f, 1920f,1080f);
        }

        protected override void OnLoad() // TODO implement culling
        {
            base.OnLoad();
            GL.Enable(EnableCap.DepthTest);

            device = new GraphicsDevice();
            content = new ContentPipeLine(device, resource);
            Shader vertexShader = new Shader(ShaderType.VertexShader);
            Shader fragmentShader = new Shader(ShaderType.FragmentShader);

            vertexShader.Compile(Encoding.UTF8.GetString(resource.Fetch("block.vert")));
            fragmentShader.Compile(Encoding.UTF8.GetString(resource.Fetch("block.frag")));

            blockProgram = new ShaderProgram();

            blockProgram.AttachShader(vertexShader);
            blockProgram.AttachShader(fragmentShader);

            blockProgram.Link();

            Mesh cube = content.LoadMesh("cube.model");

            mesh = new(new Vector3(0, 0, 0), cube);
            dirtTexture = content.LoadTexture2D("dirt.png");
            Console.WriteLine(GL.GetInteger(GetPName.MaxTextureImageUnits));
            CursorState = CursorState.Grabbed;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            camera3D.UpdateFov(e.Width, e.Height);

        }
        protected override void OnUpdateFrame(FrameEventArgs frameArgs)
        {
            //cube.transformation = Matrix4.CreateRotationY(angle += 0.0001f) * Matrix4.CreateRotationX(angle += 0.0001f) * Matrix4.CreateTranslation(0, 0, -3f);
            mesh.SetRotation(angle, angle, angle);
            //mesh.SetLocation(new Vector3(angle, 0, 0));
            //camera.Pitch += angle;
            //block.RotateY(angle);
            //block.RotateX(angle);
            //block.RotateZ(angle);
            xAxis += 0.0002f;
            yAxis += 0.0001f;
            angle += 0.0001f;
            camera3D.InputController(KeyboardState, MouseState, frameArgs);
            base.OnUpdateFrame(frameArgs);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            
            GL.ClearColor(device._background);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            camera3D.UpdateCameraVectors();
            device.Render(mesh, dirtTexture, blockProgram, camera3D);
            //Console.WriteLine(GL.GetError());
            Context.SwapBuffers();


            base.OnRenderFrame(args);
        }
    }
}
    