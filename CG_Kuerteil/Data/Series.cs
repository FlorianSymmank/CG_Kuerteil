using OpenTK.Mathematics;
using System.Collections;

namespace CG_Kuerteil.Data
{
    public class Series
    {
        protected List<DataPoint> dataPoints = new List<DataPoint>();
        public virtual Color4 Color { get; set; }
        public float MinValue => dataPoints.Min(x => x.Value);
        public float MaxValue => dataPoints.Max(x => x.Value);
        public float SummedValue => dataPoints.Sum(x => x.Value);
        public bool HasNegativeValues => dataPoints.Any(x => x.Value < 0);

        public int Count => dataPoints.Count;

        public void AddDataPoint(DataPoint data)
        {
            dataPoints.Add(data);
        }

        public void RemoveDataPoints()
        {
            dataPoints.Clear();
        }

        public DataPoint this[int index]
        {
            get => dataPoints[index];
        }
    }
}