using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_Kuerteil.Graphics
{
    public class CharUnit
    {
        public char Char { get; set; }
        public int Handle { get; set; }
        public TextureUnit Unit { get; set; } = TextureUnit.Texture0;
        public int Count { get; set; }
        public int vertexArrayObject { get; set; }

        public void Use()
        {
            GL.ActiveTexture(Unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }
    }
}
