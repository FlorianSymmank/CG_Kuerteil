﻿using CG_Kuerteil.Graphics;
using CG_Kuerteil.Util;
using OpenTK.Mathematics;

namespace CG_Kuerteil.Data
{
    public class PieDiagram : Diagramm
    {
        private Random rnd = new Random();
        public PieDiagram(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public override void SetSeries(List<Series> series)
        {
            if (series.Count > 1)
                Logger.Log(Logger.LogLevel.Warn, "PieDiagramm supports just a single series (Later series will be ignored).");

            if (series[0].HasNegativeValues)
            {
                Logger.Log(Logger.LogLevel.Warn, "Negative numbers are not supported byPieDiagramm.");
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
                if(i == series[0].Count - 1 && angle + currRotation != 360)
                    angle += 360 - (angle + currRotation);

                Slice s = new(this, angle)
                {
                    Color = getRandomColor(),
                    RotationZ = currRotation,
                };

                currRotation += angle;
                Base3DObjects.Add(s);
            }
        }

        private Color4 getRandomColor()
        {
            return new Color4((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), 255);
        }
    }
}
