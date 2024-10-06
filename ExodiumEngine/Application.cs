using ExodiumEngine.Content;
using ExodiumEngine.Extensions;
using ExodiumEngine.OpenGL;
using ExodiumEngine.Rendering;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ExodiumEngine
{
    public class Application : GameWindow // TODO create a texture manager
    {
        static private Application s_Instance;
        public static Application GetInstance() => s_Instance;
        
        Camera3D m_camera3D;
        Camera2D m_camera2D;
        GraphicsDevice m_device;

        IResource m_resource;
        ContentPipeLine m_content;
        int m_maxTextureUnits = 0;

        readonly Dictionary<string, AbstractScene> m_scenes;
        AbstractScene m_currScene;

        private string? m_nextScene;

        private FpsInfo m_fpsInfo;

        public IResource Resource => m_resource;
        public ContentPipeLine Content => m_content;
        public Camera3D Camera3D => m_camera3D;
        public Camera2D Camera2D => m_camera2D;
        public GraphicsDevice Device => m_device;
        public AbstractScene Scene => m_currScene;
        public int MaxTextureUnits => m_maxTextureUnits;
 
        public Application(Dictionary<string, AbstractScene> scenes, string startScene, string name = "unknown") : base(GameWindowSettings.Default, NativeWindowSettings.Default) {
            
            Title = name;
            m_camera3D = new Camera3D(45f, 1920f, 1080f); // clean up aspect ratio code does not work still
            m_resource = new RawResource();
            m_device = new GraphicsDevice();
            m_content = new ContentPipeLine(m_device, m_resource);
            m_scenes = scenes;
            s_Instance = this;

            m_currScene = m_scenes[startScene];
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front); // idk back culling does not work lol
            
            m_maxTextureUnits = GL.GetInteger(GetPName.MaxTextureImageUnits);
            CursorState = CursorState.Grabbed;

            //VSync = VSyncMode.On;
            Scene.OnLoad();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            m_camera3D.UpdateFov(e.Width, e.Height);

        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
#if DEBUG
            m_fpsInfo.frameCount++;
            m_fpsInfo.frameCurrentTime += e.Time;

            if (m_fpsInfo.frameCurrentTime >= 1.0)
            {
                Title = $"FPS: {m_fpsInfo.frameCount}";
                m_fpsInfo.frameCount = 0;
                m_fpsInfo.frameCurrentTime = 0.0;
            }
#endif
            if (m_nextScene != null)
            {
                //Signal old scene that it's about to be switched.
                m_currScene.OnDeactivation();

                m_currScene = m_scenes[m_nextScene];
                //Signal new scene that it is about to be up.
                m_currScene.OnActivation();
                m_nextScene = null;
            }

            if (KeyboardState.IsKeyDown(Keys.Escape))
                Environment.Exit(0);

            m_currScene.Update(KeyboardState,MouseState, e.Time);
            
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.ClearColor(m_device._background);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            //GL.MultiDrawElements() singular draw call?
            foreach(Renderable gameObject in m_currScene.GetRenderables()) // create a render manager, As we should manage the order we render things as many things should just use the same shader.
                m_device.Render(gameObject, gameObject.GetTexture2D(), m_currScene.GetShaderProgram(), Camera3D);
            
            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }

    public struct FpsInfo()
    {
        public int frameCount = 0;
        public double frameCurrentTime = 0f;
    }
}
    