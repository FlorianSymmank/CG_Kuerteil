using CG_Kuerteil.Graphics;
using OpenTK.Mathematics;


namespace CG_Kuerteil.Data
{
    public class BarDiagram : Diagramm
    {
        public BarDiagram(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public override void SetSeries(List<Series> series)
        {
            Base3DObjects.Clear();

            float spacer = 0.5f;
            float width = 1f / series.Count / series[0].Count;
            float start = -(spacer * series.Count + width * series[0].Count) / 2;
            float offsetSpace = start;

            float min = series.Min(x => x.MinValue);
            float max = series.Max(x => x.MaxValue);

            float maxmax = Math.Max(Math.Abs(max), Math.Abs(min));

            min -= min * 0.1f;
            max += max * 0.1f;

            float range = max - min;

            for (int i = 0; i < series[0].Count; i++)
            {
                foreach (Series s in series)
                {
                    DataPoint dp = s[i];

                    Vector3 scale = new(width, dp.Value / range, width);
                    Vector3 pos = new(offsetSpace, scale.Y / 2, 0);
                    offsetSpace += width;

                    Base3DObject base3dobject = new Cube(this)
                    {
                        Color = dp.Color,
                        Scale = scale,
                        Position = pos,
                    };

                    Base3DObjects.Add(base3dobject);
                }

                offsetSpace += spacer;
            }

            // Percentage bars
            float percentageIndicator25 = maxmax / range / 4;
            for (int i = 1; i < 5; i++)
            {
                float y = i * percentageIndicator25;
                Base3DObjects.Add(new Cube(this)
                {
                    Color = Color4.Gray,
                    Scale = new(offsetSpace + offsetSpace * 0.4f, 0.001f, 0.001f),
                    Position = new(0, y, 0),
                });
            }

            if (min < 0)
                for (int i = -1; i > -5; i--)
                {
                    float y = i * percentageIndicator25;
                    Base3DObjects.Add(new Cube(this)
                    {
                        Color = Color4.Gray,
                        Scale = new(offsetSpace + offsetSpace * 0.4f, 0.001f, 0.001f),
                        Position = new(0, y, 0),
                    });
                }

            Base3DObjects.Add(new Cube(this)
            {
                Color = Color4.Black,
                Scale = new(offsetSpace + offsetSpace * 0.4f, 0.001f, 0.001f),
                Position = new(0, 0, 0),
            });

            DataSeries = series;
        }
    }
}
