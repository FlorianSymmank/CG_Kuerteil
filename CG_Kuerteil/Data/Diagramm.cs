using CG_Kuerteil.Graphics;

namespace CG_Kuerteil.Data
{
    public abstract class Diagramm : Container
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";

        public abstract void SetSeries(List<Series> series);

        public void RemoveAllSeries()
        {
            Base3DObjects.Clear();
        }
    }
}
