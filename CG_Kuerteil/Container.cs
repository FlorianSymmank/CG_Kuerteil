using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_Kuerteil
{
    public class Container
    {
        private List<Bar> bars = new List<Bar>() { new Bar(Color4.Firebrick, 19),new Bar(Color4.Firebrick, 19),new Bar(Color4.Firebrick, 19),new Bar(Color4.Aqua, 10), new Bar(Color4.Aqua, 10), new Bar(Color4.Firebrick, 19), new Bar(Color4.Firebrick, 19), new Bar(Color4.Aqua, 10), new Bar(Color4.Aqua, 10), new Bar(Color4.Aqua, 10), new Bar(Color4.Beige, 4), new Bar(Color4.Firebrick, 19), new Bar(Color4.Green, -25) };
        public List<DrawBar> DrawBars = new List<DrawBar>();
        public Container()
        {
            float space = 2f / bars.Count;
            float start = space / 2 - 1;
            float width = 1f / bars.Count;
            float offsetSpace = start;

            float min = bars.Min(x => x.Value);
            float max = bars.Max(x => x.Value);

            float range = max - min;

            foreach (Bar bar in bars)
            {
                Vector3 scale = new(width, bar.Value / range, width);
                Vector3 pos = new(offsetSpace, scale.Y / 2, 0);

                DrawBars.Add(new DrawBar(bar, pos, scale));
                offsetSpace += space;
            }
        }

        public Matrix4 model = Matrix4.Identity;
        private float normalize(float input, float min, float max)
        {
            return (input - min) / (max - min);
        }
    }
}
