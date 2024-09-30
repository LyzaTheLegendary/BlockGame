using BlockGame.Rendering.Shaders;
using OpenTK.Mathematics;

namespace BlockGame.Rendering
{
    public readonly struct Mesh // idea make one ofthese which represents a normal block, and keep copying this one so we have the same gpu data.
    {
        readonly int _indicesSize = 0;
        readonly int _vboGraphicsPointer = 0;
        readonly int _vboTexelGraphicsPointer = 0;
        readonly int _iboGraphicsPointer = 0;
        readonly int _vaoGraphicsPointer = 0;

        //public Matrix4 transformation = Matrix4.CreateTranslation(0, 0, -3f); shouldn't be in here, as this is not the renderable object, it is simply the mesh
        public readonly int VboPointer => _vboGraphicsPointer;
        public readonly int VboTextPointer => _vboTexelGraphicsPointer;
        public readonly int IboPointer => _iboGraphicsPointer;
        public readonly int VaoPointer => _vaoGraphicsPointer;
        public readonly int Indices => _indicesSize;

        public Mesh(int vboPointer, int vboTexelPointer, int iboPointer, int vaoPointer, int indicesSize)
        {
            _indicesSize = indicesSize;
            _vboGraphicsPointer = vboPointer;
            _vboTexelGraphicsPointer = vboTexelPointer;
            _iboGraphicsPointer = iboPointer;
            _vaoGraphicsPointer = vaoPointer;
        }
        public Mesh Clone() => (Mesh)MemberwiseClone();
    }
}
