using CG_Kuerteil.Data;
using CG_Kuerteil.Util;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace CG_Kuerteil
{
    public static class Program
    {
        /* Tutorial: https://opentk.net/learn/index.html
           Shader.cs, Camera.cs, shader.frag und shader.vert mit geringen Änderungen übernommen */

        public static void Main()
        {
            Logger.Log(Logger.LogLevel.Info, "Application Started");
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(1920, 1080),
                Title = "CG-Kürteil 2022 3D-Diagramme Florian Symmank 578767",
                Flags = ContextFlags.ForwardCompatible,

            };
            using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.OnLoaded += AddDiagramm;
                window.IsVisible = false;
                window.Run();
            }
        }
        private static void AddDiagramm(Window window)
        {
            Console.WriteLine("1: PieDiagramm, 2:BarDiagramm, 3:StackBarDiagramm");
            Diagramm d = Console.ReadLine() switch
            {
                "2" => new BarDiagram("BarDiagram", "Description"),
                "3" => new StackBarDiagramm("StackBarDiagramm", "Description"),
                _ => new PieDiagram("PieDiagramm", "Description"),
            };

            Series series = new()
            {
                Description = "Description 1",
                Color = createColor(),
            };
            series.AddDataPoint(new(1, createColor()) { Title = "Series 1, Datapoint 1" });
            series.AddDataPoint(new(1, createColor()) { Title = "Series 1, Datapoint 2" });
            series.AddDataPoint(new(5, createColor()) { Title = "Series 1, Datapoint 3" });
            series.AddDataPoint(new(9, createColor()) { Title = "Series 1, Datapoint 4" });
            series.AddDataPoint(new(14, createColor()) { Title = "Series 1, Datapoint 5" });


            Series series2 = new()
            {
                Description = "Description 2",
                Color = createColor(),

            };
            series2.AddDataPoint(new(19, createColor()) { Title = "Series 2, Datapoint 1" });
            series2.AddDataPoint(new(0, createColor()) { Title = "Series 2, Datapoint 2" });
            series2.AddDataPoint(new(5, createColor()) { Title = "Series 2, Datapoint 3" });
            series2.AddDataPoint(new(4, createColor()) { Title = "Series 2, Datapoint 4" });
            series2.AddDataPoint(new(9, createColor()) { Title = "Series 2, Datapoint 5" });

            Series series3 = new()
            {
                Description = "Description 3",
                Color = createColor(),
            };

            series3.AddDataPoint(new(4, createColor()) { Title = "Series 3, Datapoint 1" });
            series3.AddDataPoint(new(13, createColor()) { Title = "Series 3, Datapoint 2" });
            series3.AddDataPoint(new(5, createColor()) { Title = "Series 3, Datapoint 3" });
            series3.AddDataPoint(new(3, createColor()) { Title = "Series 3, Datapoint 4" });
            series3.AddDataPoint(new(20, createColor()) { Title = "Series 3, Datapoint 5" });

            Series series4 = new()
            {
                Description = "Description 4",
                Color = createColor(),
            };

            series4.AddDataPoint(new(20, createColor()) { Title = "Series 4, Datapoint 1" });
            series4.AddDataPoint(new(13, createColor()) { Title = "Series 4, Datapoint 2" });
            series4.AddDataPoint(new(5, createColor()) { Title = "Series 4, Datapoint 3" });
            series4.AddDataPoint(new(17, createColor()) { Title = "Series 4, Datapoint 4" });
            series4.AddDataPoint(new(6, createColor()) { Title = "Series 4, Datapoint 5" });

            d.SetSeries(new List<Series>() { series, series2, series3/*, series4 */});
            window.SetDiagramm(d);
            window.IsVisible = true;
        }
        private static Random rnd = new Random();
        private static Color4 createColor() => new Color4((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble(), 255);
    }
}