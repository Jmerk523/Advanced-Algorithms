using System;
using System.Collections.Generic;

namespace Advanced.Algorithms.Geometry
{
    /// <summary>
    /// Line object.
    /// </summary>
    public struct Line : IEquatable<Line>
    {
        public Point Left { get; }
        public Point Right { get; }

        public bool IsVertical => Left.X == Right.X;
        public bool IsHorizontal => Left.Y == Right.Y;

        public double Slope { get; }

        internal Line(Point start, Point end, double tolerance)
        {
            if (start.X < end.X)
            {
                Left = start;
                Right = end;
            }
            else if (start.X > end.X)
            {
                Left = end;
                Right = start;
            }
            else
            {
                //use Y
                if (start.Y < end.Y)
                {
                    Left = start;
                    Right = end;
                }
                else
                {
                    Left = end;
                    Right = start;
                }
            }

            Slope = default;
            Slope = calcSlope();
        }

        public Line(Point start, Point end, int precision = 5)
        : this(start, end, Math.Round(Math.Pow(0.1, precision), precision)) { }

        private double calcSlope()
        {
            Point left = Left, right = Right;

            //vertical line has infinite slope
            if (left.Y == right.Y)
            {
                return double.MaxValue;
            }

            return ((right.Y - left.Y) / (right.X - left.X));
        }

        public override bool Equals(object obj)
        {
            return obj is Line line && Equals(line);
        }

        public bool Equals(Line other)
        {
            return EqualityComparer<Point>.Default.Equals(Left, other.Left) &&
                   EqualityComparer<Point>.Default.Equals(Right, other.Right) &&
                   Slope == other.Slope;
        }

        public override int GetHashCode()
        {
            int hashCode = 450345861;
            hashCode = hashCode * -1521134295 + Left.GetHashCode();
            hashCode = hashCode * -1521134295 + Right.GetHashCode();
            hashCode = hashCode * -1521134295 + Slope.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Line left, Line right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Line left, Line right)
        {
            return !(left == right);
        }
    }
}
