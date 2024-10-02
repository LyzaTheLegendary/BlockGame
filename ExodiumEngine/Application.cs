using ExodiumEngine.Content;
using ExodiumEngine.Extensions;
using ExodiumEngine.OpenGL;
using ExodiumEngine.Rendering;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ExodiumEngine // rename project to ExodiumEngine
{
    public class Application : GameWindow // TODO create a texture manager
    {
        static private Application s_Instance;
        public static Application GetInstance() => s_Instance;
        GraphicsDevice m_device;
        Camera3D m_camera3D;
        IResource m_resource;
        ContentPipeLine m_content;
        int m_maxTextureUnits = 0;
        AbstractScene m_scene;

        public IResource Resource => m_resource;
        public ContentPipeLine Content => m_content;
        public Camera3D Camera3D => m_camera3D;
        public GraphicsDevice Device => m_device;
        public AbstractScene Scene => m_scene;
        public int MaxTextureUnits => m_maxTextureUnits;
 
        public Application(string name = "unknown") : base(GameWindowSettings.Default, NativeWindowSettings.Default) {
            Title = name;
            m_camera3D = new Camera3D(45f, 1920f, 1080f); // clean up aspect ratio code does not work still
            m_resource = new RawResource();
            m_device = new GraphicsDevice();
            m_content = new ContentPipeLine(m_device, m_resource);
            s_Instance = this;
        }

        protected override void OnLoad() // TODO implement culling
        {
            base.OnLoad();
            GL.Enable(EnableCap.DepthTest);

            m_scene = new TestScene();
            m_maxTextureUnits = GL.GetInteger(GetPName.MaxTextureImageUnits);
            CursorState = CursorState.Grabbed;
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            m_camera3D.UpdateFov(e.Width, e.Height);

        }
        protected override void OnUpdateFrame(FrameEventArgs frameArgs)
        {
            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
                Environment.Exit(0);

            m_scene.Update(KeyboardState,MouseState, frameArgs.Time);
            m_camera3D.UpdateCameraVectors();
            base.OnUpdateFrame(frameArgs);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            
            GL.ClearColor(m_device._background);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //GL.MultiDrawElements() singular draw call?
            foreach(Renderable gameObject in m_scene.GetRenderables())
                m_device.Render(gameObject, gameObject.GetTexture2D(), m_scene.GetShaderProgram(), Camera3D);
            
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}
    