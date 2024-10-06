using ExodiumEngine.Content;
using ExodiumEngine.Extensions;
using ExodiumEngine.OpenGL;
using ExodiumEngine.Rendering;
using ExodiumEngine.Rendering.Shaders;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ExodiumEngine
{
    public abstract class AbstractScene
    {
        private Application Application => Application.GetInstance();
        protected GraphicsDevice Device => Application.Device;
        protected ContentPipeLine ContentPipeLine => Application.Content;
        protected Camera3D Camera3D => Application.Camera3D;
        protected IResource Resource => Application.Resource;

        protected List<Renderable> renderables = new List<Renderable>();
        public List<Renderable> GetRenderables() => renderables;
        public abstract ShaderProgram GetShaderProgram();
        public abstract void Update(KeyboardState keyboardState, MouseState mouseState, double gameTime);
        public abstract void OnLoad();
        public abstract void OnActivation();
        public abstract void OnDeactivation();
    }
}
