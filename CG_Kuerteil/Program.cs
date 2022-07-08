using CG_Kuerteil.Data;
using CG_Kuerteil.Util;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace CG_Kuerteil
{
    public static class Program
    {
        public static void Main()
        {
            Logger.Log(Logger.LogLevel.Info, "Application Started");

            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "LearnOpenTK - Creating a Window",
                // This is needed to run on macos
                Flags = ContextFlags.ForwardCompatible,
                
            };

            // To create a new window, create a class that extends GameWindow, then call Run() on it.
            using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.OnLoaded += AddDiagramm;
                window.Run();
            }
        }

        private static void AddDiagramm(Window window)
        {
            Diagramm d = new BarDiagram("Pie diagramm", "Description");

            Series series = new Series()
            {
                Description = "Description 1",
                Color = createColor(),
            };
            series.AddDataPoint(new(1, createColor()) { Title = "Series 1, Datapoint 1" });
            series.AddDataPoint(new(1, createColor()) { Title = "Series 1, Datapoint 2" });
            series.AddDataPoint(new(19, createColor()) { Title = "Series 1, Datapoint 3" });

            Series series2 = new Series()
            {
                Description = "Description 2",
                Color = createColor(),

            };
            series2.AddDataPoint(new(19, createColor()) { Title = "Series 2, Datapoint 1" });
            series2.AddDataPoint(new(0, createColor()) { Title = "Series 2, Datapoint 2" });
            series2.AddDataPoint(new(5, createColor()) { Title = "Series 2, Datapoint 3" });

            Series series3 = new Series()
            {
                Description = "Description 3",
                Color = createColor(),
            };

            series3.AddDataPoint(new(19, createColor()) { Title = "Series 3, Datapoint 1" });
            series3.AddDataPoint(new(13, createColor()) { Title = "Series 3, Datapoint 2" });
            series3.AddDataPoint(new(-5, createColor()) { Title = "Series 3, Datapoint 3" });

            d.SetSeries(new List<Series>() { series, series2, series3 });
            window.SetDiagramm(d);
        }

        private static Random rnd = new Random();

        private static Color4 createColor()
        {
            return new Color4((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), 255);
        }
    }
}