using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CG_Kuerteil
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
