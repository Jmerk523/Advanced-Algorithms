using Advanced.Algorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advanced.Algorithms.Geometry
{
    /// <summary>
    ///     Bentley-Ottmann sweep line algorithm to find line intersections.
    /// </summary>
    public class BentleyOttmann
    {
        private readonly PointComparer pointComparer;

        private HashSet<Event> verticalAndHorizontalLines;
        private HashSet<Event> otherLines;

        private BHeap<Event> eventQueue;
        private HashSet<Event> eventQueueLookUp;

        private Dictionary<Event, Event> rightLeftEventLookUp;
        private RedBlackTree<Event> currentlyTrackedLines;

        private Dictionary<Point, HashSet<Tuple<Event, Event>>> intersectionEvents;

        internal readonly double Tolerance;

        internal Line SweepLine;

        public BentleyOttmann(int precision = 5)
        {
            pointComparer = new PointComparer();

            Tolerance = Math.Round(Math.Pow(0.1, precision), precision);
        }

        private void initialize(IEnumerable<Line> lineSegments)
        {
            SweepLine = new Line(new Point(0, 0), new Point(0, int.MaxValue), Tolerance);

            currentlyTrackedLines = new RedBlackTree<Event>(true, pointComparer);
            intersectionEvents = new Dictionary<Point, HashSet<Tuple<Event, Event>>>(pointComparer);

            verticalAndHorizontalLines = new HashSet<Event>();
            otherLines = new HashSet<Event>();

            rightLeftEventLookUp = lineSegments
                                   .Select(x =>
                                   {
                                       if (x.Left.X < 0 || x.Left.Y < 0 || x.Right.X < 0 || x.Right.Y < 0)
                                       {
                                           throw new Exception("Negative coordinates are not supported.");
                                       }

                                       return new KeyValuePair<Event, Event>(
                                          new Event(x.Left, pointComparer, EventType.Start, x, this),
                                          new Event(x.Right, pointComparer, EventType.End, x, this)
                                       );

                                   }).ToDictionary(x => x.Value, x => x.Key);

            eventQueueLookUp = new HashSet<Event>(rightLeftEventLookUp.SelectMany(x => new[] {
                                    x.Key,
                                    x.Value
                                }));

            eventQueue = new BHeap<Event>(SortDirection.Ascending, eventQueueLookUp, new EventQueueComparer());
        }

        public Dictionary<Point, List<Line>> FindIntersections(IEnumerable<Line> lineSegments)
        {
            initialize(lineSegments);

            while (eventQueue.Count > 0)
            {
                var currentEvent = eventQueue.Extract();
                eventQueueLookUp.Remove(currentEvent);
                sweepTo(currentEvent);

                switch (currentEvent.Type)
                {
                    case EventType.Start:

                        //special case
                        if (verticalAndHorizontalLines.Count > 0)
                        {
                            foreach (var line in verticalAndHorizontalLines)
                            {
                                var intersection = findIntersection(currentEvent, line);
                                recordIntersection(currentEvent, line, intersection);
                            }
                        }

                        //special case
                        if (currentEvent.Segment.Value.IsVertical || currentEvent.Segment.Value.IsHorizontal)
                        {
                            verticalAndHorizontalLines.Add(currentEvent);

                            foreach (var line in otherLines)
                            {
                                var intersection = findIntersection(currentEvent, line);
                                recordIntersection(currentEvent, line, intersection);
                            }

                            break;
                        }

                        otherLines.Add(currentEvent);

                        currentlyTrackedLines.Insert(currentEvent);

                        var lower = currentlyTrackedLines.NextLower(currentEvent);
                        var upper = currentlyTrackedLines.NextHigher(currentEvent);

                        var lowerIntersection = findIntersection(currentEvent, lower);
                        recordIntersection(currentEvent, lower, lowerIntersection);
                        enqueueIntersectionEvent(currentEvent, lowerIntersection);

                        var upperIntersection = findIntersection(currentEvent, upper);
                        recordIntersection(currentEvent, upper, upperIntersection);
                        enqueueIntersectionEvent(currentEvent, upperIntersection);

                        break;

                    case EventType.End:

                        currentEvent = rightLeftEventLookUp[currentEvent];

                        //special case
                        if (currentEvent.Segment.Value.IsVertical || currentEvent.Segment.Value.IsHorizontal)
                        {
                            verticalAndHorizontalLines.Remove(currentEvent);
                            break;
                        }

                        otherLines.Remove(currentEvent);

                        lower = currentlyTrackedLines.NextLower(currentEvent);
                        upper = currentlyTrackedLines.NextHigher(currentEvent);

                        currentlyTrackedLines.Delete(currentEvent);

                        var upperLowerIntersection = findIntersection(lower, upper);
                        recordIntersection(lower, upper, upperLowerIntersection);
                        enqueueIntersectionEvent(currentEvent, upperLowerIntersection);

                        break;

                    case EventType.Intersection:

                        var intersectionLines = intersectionEvents[currentEvent.Point];

                        foreach (var lines in intersectionLines)
                        {
                            //special case
                            if (lines.Item1.Segment.Value.IsHorizontal || lines.Item1.Segment.Value.IsVertical
                                || lines.Item2.Segment.Value.IsHorizontal || lines.Item2.Segment.Value.IsVertical)
                            {
                                continue;
                            }

                            swapBstNodes(currentlyTrackedLines, lines.Item1, lines.Item2);

                            var upperLine = lines.Item1;
                            var upperUpper = currentlyTrackedLines.NextHigher(upperLine);

                            var newUpperIntersection = findIntersection(upperLine, upperUpper);
                            recordIntersection(upperLine, upperUpper, newUpperIntersection);
                            enqueueIntersectionEvent(currentEvent, newUpperIntersection);

                            var lowerLine = lines.Item2;
                            var lowerLower = currentlyTrackedLines.NextLower(lowerLine);

                            var newLowerIntersection = findIntersection(lowerLine, lowerLower);
                            recordIntersection(lowerLine, lowerLower, newLowerIntersection);
                            enqueueIntersectionEvent(currentEvent, newLowerIntersection);
                        }

                        break;
                }

            }

            return intersectionEvents.ToDictionary(x => x.Key,
                                                   x => x.Value.SelectMany(y => new[] { y.Item1.Segment.Value, y.Item2.Segment.Value })
                                                       .Distinct().ToList());
        }

        private void sweepTo(Event currentEvent)
        {
            SweepLine = new Line(new Point(currentEvent.X, 0), new Point(currentEvent.X, int.MaxValue), Tolerance);
        }

        internal void swapBstNodes(RedBlackTree<Event> currentlyTrackedLines, Event value1, Event value2)
        {
            var node1 = currentlyTrackedLines.Find(value1).Item1;
            var node2 = currentlyTrackedLines.Find(value2).Item1;

            if (node1 == null || node2 == null)
            {
                throw new Exception("Value1, Value2 or both was not found in this BST.");
            }

            var tmp = node1.Value;
            node1.Value = node2.Value;
            node2.Value = tmp;

            currentlyTrackedLines.NodeLookUp[node1.Value] = node1;
            currentlyTrackedLines.NodeLookUp[node2.Value] = node2;
        }

        private void enqueueIntersectionEvent(Event currentEvent, Point? intersection)
        {
            if (intersection == null)
            {
                return;
            }

            var intersectionEvent = new Event(intersection.Value, pointComparer, EventType.Intersection, null, this);

            if (intersectionEvent.X > SweepLine.Left.X
                || (intersectionEvent.X == SweepLine.Left.X
                   && intersectionEvent.Y > currentEvent.Y))
            {
                if (!eventQueueLookUp.Contains(intersectionEvent))
                {
                    eventQueue.Insert(intersectionEvent);
                    eventQueueLookUp.Add(intersectionEvent);
                }
            }

        }

        private Point? findIntersection(Event a, Event b)
        {
            if (a == null || b == null
                || a.Type == EventType.Intersection
                || b.Type == EventType.Intersection)
            {
                return null;
            }

            if (b.Segment == null)
            {
                return null;
            }
            return a.Segment?.Intersection(b.Segment.Value, Tolerance);
        }

        private void recordIntersection(Event line1, Event line2, Point? intersection)
        {
            if (intersection == null)
            {
                return;
            }

            var existing = intersectionEvents.ContainsKey(intersection.Value) ?
                intersectionEvents[intersection.Value] : new HashSet<Tuple<Event, Event>>();

            if (line1.Segment.Value.Slope.CompareTo(line2.Segment.Value.Slope) > 0)
            {
                existing.Add(new Tuple<Event, Event>(line1, line2));
            }
            else
            {
                existing.Add(new Tuple<Event, Event>(line2, line1));
            }

            intersectionEvents[intersection.Value] = existing;
        }
    }

    //point type
    internal enum EventType
    {
        Start = 0,
        Intersection = 1,
        End = 2
    }

    /// <summary>
    ///     A custom object representing start/end/intersection point.
    /// </summary>
    internal class Event : IPoint, IComparable<Event>
    {
        private readonly double tolerance;
        private readonly PointComparer pointComparer;

        internal EventType Type;

        //The full line only if not an intersection event
        internal Line? Segment;

        internal BentleyOttmann Algorithm;

        internal Line LastSweepLine;
        internal Point LastIntersection;
        internal Point Point;

        internal Event(Point eventPoint, PointComparer pointComparer, EventType eventType,
            Line? lineSegment, BentleyOttmann algorithm)
        {
            tolerance = algorithm.Tolerance;
            this.pointComparer = pointComparer;

            Point = eventPoint;
            Type = eventType;
            Segment = lineSegment;
            Algorithm = algorithm;
        }

        public double X => Point.X;
        public double Y => Point.Y;

        public int CompareTo(Event thatEvent)
        {
            if (Equals(thatEvent))
            {
                return 0;
            }

            if (!Segment.HasValue || !thatEvent.Segment.HasValue)
            {
                throw new InvalidOperationException();
            }

            var line1 = Segment.Value;
            var line2 = thatEvent.Segment.Value;

            Point intersectionA;
            if (Type == EventType.Intersection)
            {
                intersectionA = Point;
            }
            else
            {
                if (LastSweepLine == Algorithm.SweepLine)
                {
                    intersectionA = LastIntersection;
                }
                else
                {
                    intersectionA = LineIntersection.FindIntersection(line1, Algorithm.SweepLine, tolerance) ?? default;
                    LastSweepLine = Algorithm.SweepLine;
                    LastIntersection = intersectionA;
                }
            }

            Point intersectionB;
            if (Type == EventType.Intersection)
            {
                intersectionB = thatEvent.Point;
            }
            else
            {
                if (thatEvent.LastSweepLine == thatEvent.Algorithm.SweepLine)
                {
                    intersectionB = thatEvent.LastIntersection;
                }
                else
                {
                    intersectionB = LineIntersection.FindIntersection(line2, thatEvent.Algorithm.SweepLine, tolerance) ?? default;
                    thatEvent.LastSweepLine = thatEvent.Algorithm.SweepLine;
                    thatEvent.LastIntersection = intersectionB;
                }
            }

            var result = intersectionA.Y.CompareTo(intersectionB.Y);
            if (result != 0)
            {
                return result;
            }

            //if Y is same use slope as comparison
            double slope1 = line1.Slope;

            //if Y is same use slope as comparison
            double slope2 = line2.Slope;

            result = slope1.CompareTo(slope2);
            if (result != 0)
            {
                return result;
            }

            //if slope is the same use diff of X co-ordinate
            result = line1.Left.X.CompareTo(line2.Left.X);
            if (result != 0)
            {
                return result;
            }

            //if diff of X co-ordinate is same use diff of Y co-ordinate
            result = line1.Left.Y.CompareTo(line2.Left.Y);

            //at this point this is guaranteed to be not same.
            //since we don't let duplicate lines with input HashSet of lines.
            //see line equals override in Line class.
            return result;
        }

        public int CompareTo(object that)
        {
            return CompareTo((Event)that);
        }

        public override bool Equals(object that)
        {
            if (that == this)
            {
                return true;
            }

            var thatEvent = that as Event;

            if ((Type != EventType.Intersection && thatEvent.Type == EventType.Intersection)
                || (Type == EventType.Intersection && thatEvent.Type != EventType.Intersection))
            {
                return false;
            }

            if (Type == EventType.Intersection && thatEvent.Type == EventType.Intersection)
            {
                return pointComparer.Equals(this, thatEvent);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    //Used to override event comparison when using BMinHeap for Event queue.
    internal class EventQueueComparer : Comparer<Event>
    {
        public override int Compare(Event a, Event b)
        {
            //same object
            if (a == b)
            {
                return 0;
            }

            //compare X
            var result = a.X.CompareTo(b.X);

            if (result != 0)
            {
                return result;
            }

            //Left event first, then intersection and finally right.
            result = a.Type.CompareTo(b.Type);

            if (result != 0)
            {
                return result;
            }

            return a.Y.CompareTo(b.Y);
        }
    }

}
