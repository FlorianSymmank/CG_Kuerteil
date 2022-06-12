using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_Kuerteil
{
    public class PieDiagram : Container
    {
        public PieDiagram(Shader shader) : base(shader)
        {
            Slice s = new Slice(this, shader, 17)
            {
                Color = Color4.BlueViolet,
            };

            Base3DObjects.Add(s);
        }
    }
}
