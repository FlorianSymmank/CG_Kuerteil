using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_Kuerteil
{
    public static class MathHelper
    {
        public static float CircleX(double angle, float radius = 1)
        {
            return radius * (float)Math.Cos(OpenTK.Mathematics.MathHelper.DegreesToRadians(angle));
        }

        public static float CircleY(double angle, float radius = 1)
        {
            return radius * (float)Math.Sin(OpenTK.Mathematics.MathHelper.DegreesToRadians(angle));
        }
    }
}
