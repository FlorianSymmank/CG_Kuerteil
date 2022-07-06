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
            Diagramm d = new BarDiagram("", "");

            Series series = new Series();
            series.AddDataPoint(new(4, Color4.Firebrick));
            series.AddDataPoint(new(10, Color4.Aqua));
            series.AddDataPoint(new(-8, Color4.Green));

            Series series2 = new Series();
            series2.AddDataPoint(new(19, Color4.Yellow));
            series2.AddDataPoint(new(0, Color4.Wheat));
            series2.AddDataPoint(new(5, Color4.Navy));

            Series series3 = new Series();
            series3.AddDataPoint(new(19, Color4.Blue));
            series3.AddDataPoint(new(13, Color4.Red));
            series3.AddDataPoint(new(-5, Color4.Black));

            d.SetSeries(new List<Series>() { series, series2, series3 });

            window.SetDiagramm(d);
        }
    }
}