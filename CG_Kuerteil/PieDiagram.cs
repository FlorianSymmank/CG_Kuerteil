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
            var (id, count) = Slice.createSlice(17, shader);
            Console.WriteLine(id);
            Base3DObjects.Add(new Slice(this, shader, id, count));
        }
    }
}
