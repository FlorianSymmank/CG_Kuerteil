using OpenTK.Mathematics;

namespace CG_Kuerteil
{
    public class DataPoint
    {
        public float Value { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public Color4 Color { get; set; }

        public DataPoint(float value, Color4 color4)
        {
            Value = value;
            Color = color4;
        }
    }
}