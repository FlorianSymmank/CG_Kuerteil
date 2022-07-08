using CG_Kuerteil.Graphics;
using CG_Kuerteil.Util;
using OpenTK.Mathematics;

namespace CG_Kuerteil.Data
{
    public class PieDiagram : Diagramm
    {
        public PieDiagram(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public override void SetSeries(List<Series> series)
        {
            Base3DObjects.Clear();

            if (series.Count > 1)
                Logger.Log(Logger.LogLevel.Warn, "PieDiagramm supports just a single series (Later series will be ignored).");

            series = new List<Series> { series[0] };

            if (series[0].HasNegativeValues)
            {
                Logger.Log(Logger.LogLevel.Warn, "Negative values are not supported by PieDiagramm.");
                return;
            }

            int currRotation = 0;
            float summedValue = series[0].SummedValue;
            float v = 360 / summedValue;

            for (int i = 0; i < series[0].Count; i++)
            {
                DataPoint dp = series[0][i];
                int angle = (int)(dp.Value * v);

                // if last, check if full circle? no => adjust to full circle
                if (i == series[0].Count - 1 && angle + currRotation != 360)
                    angle += 360 - (angle + currRotation);

                Slice s = new(this, angle)
                {
                    Color = dp.Color,
                    RotationZ = currRotation,
                };

                currRotation += angle;
                Base3DObjects.Add(s);
            }

            DataSeries = series;
            AddRotation(Direction.Y, (float)Math.PI);
        }
    }
}
