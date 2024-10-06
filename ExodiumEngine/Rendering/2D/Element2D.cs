using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExodiumEngine.Rendering._2D
{
    internal class Element2D // create a shader for 2d
    {
        static Mesh? square = null;
        Renderable renderable;
        Rectangle bounds;
        public Element2D(Rectangle bounds, int texturePointer) // TODO: implement me.
        {
            //if(square == null)
            //{
            //    Application.GetInstance().Device.CreateMesh
            //} 
            //renderable = new Renderable(new Vector3(bounds.X, bounds.Y, 0), )
        }
    }
}
