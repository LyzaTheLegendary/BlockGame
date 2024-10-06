using ExodiumEngine.Rendering.Shaders;
using ExodiumEngine.Rendering;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace ExodiumEngine
{
    public class TestScene : AbstractScene
    {
        ShaderProgram blockProgram;

        Texture2D dirtTexture;
        float angle = 0f;

        public override ShaderProgram GetShaderProgram() => blockProgram;
        public override void Update(KeyboardState keyboardState, MouseState mouseState, double gameTime)
        {
            Camera3D.InputController(keyboardState, mouseState, (float)gameTime);
            float iterator = 0f;
            foreach (Renderable renderable in renderables)
            {
                renderable.SetRotation(angle, angle, angle);
                renderable.SetLocation(new Vector3(0, iterator++, 0));
            }

            angle += 0.0001f;
        }

        public override void OnLoad()
        {
            Shader vertexShader = new Shader(ShaderType.VertexShader);
            Shader fragmentShader = new Shader(ShaderType.FragmentShader);

            vertexShader.Compile(Encoding.UTF8.GetString(Resource.Fetch("entity.vert")));
            fragmentShader.Compile(Encoding.UTF8.GetString(Resource.Fetch("entity.frag")));

            blockProgram = new ShaderProgram();

            blockProgram.AttachShader(vertexShader);
            blockProgram.AttachShader(fragmentShader);

            blockProgram.Link();

            Mesh cube = ContentPipeLine.LoadMesh("cube.model");
            dirtTexture = ContentPipeLine.LoadTexture2D("dirt.png");

            for (int i = 0; i < 1; i++)
            {
                renderables.Add(new Renderable(new Vector3(0, i, 0), cube, dirtTexture));
            }
        }

        public override void OnActivation()
        {

        }

        public override void OnDeactivation()
        {

        }

    }
}
