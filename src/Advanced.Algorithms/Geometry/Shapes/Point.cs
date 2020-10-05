namespace Advanced.Algorithms.Geometry
{
    public interface IPoint
    {
        double X { get; }
        double Y { get; }
    }

    /// <summary>
    /// Point object.
    /// </summary>
    public struct Point : IPoint
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }
        public double Y { get; }

        public override string ToString()
        {
            return X.ToString("F") + " " + Y.ToString("F");
        }
    }
}
