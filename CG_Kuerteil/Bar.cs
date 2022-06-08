using OpenTK.Mathematics;
using System.Drawing;


namespace CG_Kuerteil
{
    public class Bar
    {
        public Color4 Color;
        public float Value;
        public Bar(Color4 color, float value)
        {
            Color = color;
            Value = value;
        }
    }
}
