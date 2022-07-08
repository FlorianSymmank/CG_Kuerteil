using CG_Kuerteil.Graphics;
using CG_Kuerteil.Util;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathHelper = CG_Kuerteil.Util.MathHelper;

namespace CG_Kuerteil.Data
{
    public class StackBarDiagramm : Diagramm
    {
        public StackBarDiagramm(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public override void SetSeries(List<Series> series)
        {
            Base3DObjects.Clear();

            if (series.Any(s => s.HasNegativeValues))
            {
                Logger.Log(Logger.LogLevel.Warn, "Negative values are not supported by StackBarDiagramm.");
                return;
            }

            float spacer = 1f / series[0].Count;
            float width = 2f / series[0].Count;
            float start = (width / 2 + spacer / 2) - (width * series[0].Count / 2f) - (spacer * series[0].Count / 2f);
            float offsetSpace = start;

            float[] summedVals = new float[series[0].Count];

            for (int i = 0; i < series.Count; i++)
                for (int j = 0; j < series[i].Count; j++)
                    summedVals[j] += series[i][j].Value;

            float min = 0;
            float max = MathHelper.Max(summedVals);

            float maxmax = max;

            min -= min * 0.1f;
            max += max * 0.1f;

            float range = max - min;

            for (int i = 0; i < series[0].Count; i++)
            {
                float yOff = 0;
                foreach (Series s in series)
                {
                    DataPoint dp = s[i];
                    Vector3 scale = new(width, dp.Value / range, width);
                    Vector3 pos = new(offsetSpace, scale.Y / 2 + yOff, 0);

                    Base3DObject base3dobject = new Cube(this)
                    {
                        Color = dp.Color,
                        Scale = scale,
                        Position = pos,
                    };

                    Base3DObjects.Add(base3dobject);
                    yOff += dp.Value / range;
                }


                offsetSpace += width + spacer;
            }

            // Percentage bars
            float percentageIndicator25 = maxmax / range / 4;
            for (int i = 1; i < 5; i++)
            {
                float y = i * percentageIndicator25;
                Base3DObjects.Add(new Cube(this)
                {
                    Color = Color4.Gray,
                    Scale = new(offsetSpace + offsetSpace * 0.55f, 0.001f, 0.001f),
                    Position = new(0, y, 0),
                });
            }

            Base3DObjects.Add(new Cube(this)
            {
                Color = Color4.Black,
                Scale = new(offsetSpace + offsetSpace * 0.55f, 0.001f, 0.001f),
                Position = new(0, 0, 0),
            });

            model *= Matrix4.CreateTranslation(0, -0.5f, 0);
            DataSeries = series;
        }
    }
}
