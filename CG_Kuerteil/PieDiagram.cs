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

        private Random rnd;
        public PieDiagram()
        {
            rnd = new Random();

            Slice s = new Slice(this, 45)
            {
                Color = getRandomColor(),
                RotationZ = 0
            };
            Base3DObjects.Add(s);

            s = new Slice(this, 45)
            {
                Color = getRandomColor(),
                RotationZ = 90
            };
            Base3DObjects.Add(s);

            s = new Slice(this, 45)
            {
                Color = getRandomColor(),
                RotationZ = 190
            };
            Base3DObjects.Add(s);

            s = new Slice(this, 45)
            {
                Color = getRandomColor(),
                RotationZ = 270
            };
            Base3DObjects.Add(s);
        }

        private Color4 getRandomColor()
        {
            return new Color4((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), 255);
        }
    }
}
