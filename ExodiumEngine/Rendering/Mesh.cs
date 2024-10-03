using ExodiumEngine.Rendering.Shaders;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ExodiumEngine.Rendering
{
    public readonly struct Mesh // idea make one of these which represents a normal block, and keep copying this one so we have the same gpu data.
    {
        readonly int m_indicesSize = 0;
        readonly int m_vboGraphicsPointer = 0;
        readonly int m_vboTexelGraphicsPointer = 0;
        readonly int m_iboGraphicsPointer = 0;
        readonly int m_vaoGraphicsPointer = 0;
        readonly int m_shaderProgramPointer = 0;
        readonly Material m_material;

        public readonly int VboPointer => m_vboGraphicsPointer;
        public readonly int VboTextPointer => m_vboTexelGraphicsPointer;
        public readonly int IboPointer => m_iboGraphicsPointer;
        public readonly int VaoPointer => m_vaoGraphicsPointer;
        public readonly int ShaderPointer => throw new NotImplementedException("Materials are not implemented yet!"); 
        public readonly int Indices => m_indicesSize;
        public Material Material => throw new NotImplementedException("Materials are not implemented yet!");

        public Mesh(int vboPointer, int vboTexelPointer, int iboPointer, int vaoPointer, int indicesSize)
        {
            m_indicesSize = indicesSize;
            m_vboGraphicsPointer = vboPointer;
            m_vboTexelGraphicsPointer = vboTexelPointer;
            m_iboGraphicsPointer = iboPointer;
            m_vaoGraphicsPointer = vaoPointer;
        }
        public Mesh Clone() => (Mesh)MemberwiseClone();

        public void Delete()
        {
            GL.DeleteBuffers(4, new int[] {VboPointer, VboTextPointer, IboPointer, VaoPointer});
        }
    }
}
