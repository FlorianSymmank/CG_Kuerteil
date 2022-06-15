using OpenTK.Mathematics;


namespace CG_Kuerteil
{
    public class BarDiagram : Container
    {
        private List<Bar> bars = new() { new Bar(Color4.Firebrick, 19), new Bar(Color4.Firebrick, 19), new Bar(Color4.Firebrick, 19), new Bar(Color4.Aqua, 10), new Bar(Color4.Aqua, 10), new Bar(Color4.Firebrick, 19), new Bar(Color4.Firebrick, 19), new Bar(Color4.Aqua, 10), new Bar(Color4.Aqua, 10), new Bar(Color4.Aqua, 10), new Bar(Color4.Beige, 4), new Bar(Color4.Firebrick, 19), new Bar(Color4.Green, -25) };

        public BarDiagram()
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

                offsetSpace += space;

                Base3DObject base3DObject = new Cube(this)
                {
                    Color = bar.Color,
                    Scale = scale,
                    Position = pos,
                };

                Base3DObjects.Add(base3DObject);
            }
        }
    }
}
