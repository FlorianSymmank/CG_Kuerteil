using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_Kuerteil
{
    public class DrawBar
    {
        public DrawBar(Bar bar, Vector3 pos,Vector3 scale)
        {
            Bar = bar;
            Pos = pos;
            Scale = scale;

            Console.WriteLine("Scale: " + scale);
        }

        public Bar Bar { get; }
        public Vector3 Pos { get; }

        public Vector3 Scale { get; }
    }
}
