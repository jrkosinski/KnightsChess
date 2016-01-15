#define USE_LOOKUP_TABLE

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace KnightsChess
{
    /// <summary>
    /// Knights's chessboard problem. The goal is to calculate the minimum number of moves from a given starting point on an unbounded 
    /// chessboard, to a given destination. Assume that the X and Y coordinate for each given point is an integer from -MAXINT to +MAXINT. 
    /// </summary>
    /// <author>John R. Kosinski</author>
    /// <created>17 Nov 2015</created>
    class Solution
    {
        private const int PointUpperBound = Int32.MaxValue;
        private const int PointLowerBound = Int32.MinValue;
#if USE_LOOKUP_TABLE
        private static Dictionary<string, Func<int, int>> _lookupTable = new Dictionary<string, Func<int, int>>();
#endif

        static Solution()
        {
            //configure lookup table 
#if USE_LOOKUP_TABLE
            _lookupTable.Add("0,1", (count) =>
            {
                if (count == 0)
                    return 3;
                else if (count == 1)
                    return 4;
                else
                    return count + 1;
            });

            _lookupTable.Add("1,1", (count) =>
            {
                return count + 2;
            });

            _lookupTable.Add("1,2", (count) =>
            {
                return count + 1;
            });

            _lookupTable.Add("0,2", (count) =>
            {
                return count + 2;
            });

            _lookupTable.Add("2,2", (count) =>
            {
                return count + 2;
            });

            _lookupTable.Add("0,3", (count) =>
            {
                return count + 3;
            });
#endif
        }

        /// <summary>
        /// Calculates the minimum number of moves a knight must make on a chessboard to get from the given start to the given end point.
        /// </summary>
        /// <param name="start">The starting point</param>
        /// <param name="end">The desired destination</param>
        /// <returns></returns>
        public int CalcMinNumberOfMoves(Point start, Point end)
        {
            //if the start equals the end, we are done 
            if (start == end)
                return 0;

            //zero the origin, to minimize the number of directions we need to figure
            end = ZeroOrigin(start, end);
            start = new Point(0, 0);
            int numberOfMoves = 0;

            //if out of bounds, we can't get there
            if (end.X > PointUpperBound ||
                end.Y > PointUpperBound ||
                end.X < PointLowerBound ||
                end.Y < PointLowerBound)
                return -1;

            //start 
            var currentPoint = start;
            List<Point> lastThreeMoves = new List<Point>();

            while (true)
            {
                //if in range, start calculating in a different way 
                if (IsInRange(currentPoint, end))
                {
                    return CalculateEndGame(currentPoint, end, lastThreeMoves, numberOfMoves);
                }
                else
                {
                    //otherwise, keep closing the distance 
                    var newPoint = GetNextMove(currentPoint, end);
                    if (newPoint == currentPoint)
                        return -1;
                    else
                        currentPoint = newPoint;

                    //save last 3 moves 
                    lastThreeMoves.Add(currentPoint);
                    numberOfMoves++;

                    while (lastThreeMoves.Count > 3)
                        lastThreeMoves.RemoveAt(0);
                }
            }
        }


        /// <summary>
        /// Ensures that the starting point is 0,0, and that x & y are positive 
        /// </summary>
        /// <returns>
        /// A new point for 'end', assuming 'start' to be 0,0.
        /// </returns>
        /// <param name="start">The starting point</param>
        /// <param name="end">The desired destination</param>
        private Point ZeroOrigin(Point start, Point end)
        {
            return new Point(Math.Abs(end.X - start.X), Math.Abs(end.Y - start.Y));
        }

        /// <summary>
        /// Returns a value indicating whether we're close enough for endgame calculation.
        /// </summary>
        /// <param name="point">The current position of the knight</param>
        /// <param name="destination">The desired destination point</param>
        /// <returns></returns>
        private bool IsInRange(Point point, Point destination)
        {
            return (CalcDistanceToTarget(point, destination) <= 3);
        }

        /// <summary>
        /// Calculates the absolute distance between any two points. 
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private long CalcDistanceToTarget(Point p1, Point p2)
        {
            return Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
        }

        /// <summary>
        /// Returns a list of all possible knight moves from the given point. 
        /// </summary>
        /// <param name="position">The given position.</param>
        /// <returns></returns>
        private List<Point> CalcPossibleMoves(Point position)
        {
            List<Point> output = new List<Point>();

            if (position.X <= PointUpperBound - 2)
            {
                if (position.Y <= PointUpperBound - 1)
                    output.Add(new Point(position.X + 2, position.Y + 1));

                if (position.Y >= PointLowerBound + 1)
                    output.Add(new Point(position.X + 2, position.Y - 1));
            }

            if (position.X <= PointUpperBound - 1)
            {
                if (position.Y <= PointUpperBound - 2)
                    output.Add(new Point(position.X + 1, position.Y + 2));

                if (position.Y >= PointLowerBound + 2)
                    output.Add(new Point(position.X + 1, position.Y - 2));
            }

            if (position.X >= PointLowerBound + 2)
            {
                if (position.Y <= PointUpperBound - 1)
                    output.Add(new Point(position.X - 2, position.Y + 1));

                if (position.Y >= PointLowerBound + 1)
                    output.Add(new Point(position.X - 2, position.Y - 1));
            }

            if (position.X >= PointLowerBound + 1)
            {
                if (position.Y <= PointUpperBound - 2)
                    output.Add(new Point(position.X - 1, position.Y + 2));

                if (position.Y >= PointLowerBound + 2)
                    output.Add(new Point(position.X - 1, position.Y - 2));
            }

            return output;
        }

        /// <summary>
        /// Returns the next move that brings the knight closer to the given destination from the given point, in a systematic way. 
        /// </summary>
        /// <param name="current">Knight's current position</param>
        /// <param name="destination">Desired end position</param>
        /// <returns>A point representing the next move</returns>
        private Point GetNextMove(Point current, Point destination)
        {
            var xDiff = destination.X - current.X;
            var yDiff = destination.Y - current.Y;

            Point newPoint = current;

            if (xDiff > yDiff)
            {
                if (current.X <= PointUpperBound - 2)
                {
                    if (yDiff < 0)
                    {
                        if (current.Y >= PointLowerBound + 1)
                            newPoint = Point.Origin;
                            newPoint = new Point(current.X + 2, current.Y - 1);
                    }
                    else
                    {
                        if (current.Y <= PointUpperBound - 1)
                            newPoint = Point.Origin;
                            newPoint = new Point(current.X + 2, current.Y + 1);
                    }
                }
            }
            else
            {
                if (current.Y <= PointUpperBound - 2)
                {
                    if (xDiff < 0)
                    {
                        if (current.X >= PointLowerBound + 1)
                            newPoint = Point.Origin;
                            newPoint = new Point(current.X - 1, current.Y + 2);
                    }
                    else
                    {
                        if (current.X <= PointUpperBound - 1)
                            newPoint = Point.Origin;
                            newPoint = new Point(current.X + 1, current.Y + 2);
                    }
                }
            }

            return newPoint;
        }

#if USE_LOOKUP_TABLE
        /// <summary>
        /// Calculates the endgame - how to reach the destination once the knight is within a certain range - using a lookup table. 
        /// </summary>
        /// <param name="current">Knight's current position</param>
        /// <param name="destination">Desired end position</param>
        /// <param name="lastNMoves">List of moves taken so far since leaving the starting point</param>
        /// <returns></returns>
        private int CalculateEndGame(Point current, Point destination, List<Point> lastNMoves, int numberOfMoves)
        {
            var xDiff = Math.Abs(destination.X - current.X);
            var yDiff = Math.Abs(destination.Y - current.Y);

            //convert x and y diffs into string keys, smaller number first 
            var xBigger = (xDiff > yDiff);
            string key = String.Format("{0},{1}", (xBigger ? yDiff : xDiff), (xBigger ? xDiff : yDiff));

            return _lookupTable[key](numberOfMoves);
        }

#else


        /// <summary>
        /// Calculates the endgame - how to reach the destination once the knight is within a certain range - using brute force and 
        /// a set of rules. 
        /// </summary>
        /// <param name="current">Knight's current position</param>
        /// <param name="destination">Desired end position</param>
        /// <param name="lastNMoves">List of moves taken so far since leaving the starting point</param>
        /// <returns></returns>
        private int CalculateEndGame(Point current, Point destination, List<Point> lastNMoves, int numberOfMoves)
        {
            //first make sure that we're not just one move away 
            if (GetNextMove(current, destination) == destination)
                return numberOfMoves + 1;


            //add the last 2 moves to the list of 'saved' points - we're now starting to make our map
            int currentlyCalculatingMove = numberOfMoves - 1;
            if (currentlyCalculatingMove <= 0)
                currentlyCalculatingMove = 1;

            //we will save points we've already looked at, here. They will be saved according to the move number they are 
            //associated with (i.e. here are all the possible moves for move 3, here are all possible moves for move 4...) 
            Dictionary<int, List<Point>> savedPoints = new Dictionary<int, List<Point>>();

            //get the point to start from 
            var startFrom = Point.Origin;
            if (lastNMoves.Count > 2)
                startFrom = lastNMoves[lastNMoves.Count - 3];

            //these will be the points we are examining (as starting points) for each iteration 
            List<Point> pointsToCheckFrom = new List<Point>();
            pointsToCheckFrom.Add(startFrom);

            //start branching out from earlier move 
            while (true)
            {
                List<Point> pointsToCheckNext = new List<Point>();

                for (int i = 0; i < pointsToCheckFrom.Count; i++)
                {
                    var allMoves = CalcPossibleMoves(pointsToCheckFrom[i]);

                    foreach (var move in allMoves)
                    {
                        //if we've reached the end, return
                        if (move == destination)
                            return currentlyCalculatingMove;

                        //otherwise, save this point 
                        if (!PointAlreadySaved(move, savedPoints))
                        {
                            if (!savedPoints.ContainsKey(currentlyCalculatingMove))
                                savedPoints.Add(currentlyCalculatingMove, new List<Point>()); 
                                
                            savedPoints[currentlyCalculatingMove].Add(move);

                            pointsToCheckNext.Add(move);
                        }
                    }
                }

                pointsToCheckFrom = pointsToCheckNext;
                currentlyCalculatingMove++;

                //unable to do it
                if (pointsToCheckNext.Count == 0)
                    return -1;
            }
        }

#endif
        /// <summary>
        /// Returns a value indicating whether or not the given point exists in the given dictionary of saved points. Used 
        /// in calculating the endgame. 
        /// </summary>
        /// <param name="point">The point to examine</param>
        /// <param name="savedPoints">Dictionary of lists of saved points</param>
        /// <returns></returns>
        private bool PointAlreadySaved(Point point, Dictionary<int, List<Point>> savedPoints)
        {
            //we never care to save the origin
            if (point == Point.Origin)
                return true;

            foreach (var list in savedPoints.Values)
            {
                if (list.Contains(point))
                    return true;
            }

            return false;
        }
    }

    /// <summary>
    /// Represents a point in 2D space. 
    /// </summary>
    struct Point
    {
        private static Point _origin = new Point(0, 0);

        public long X;
        public long Y;

        /// <summary>
        /// 0, 0
        /// </summary>
        public static Point Origin
        {
            get { return _origin; }
        }

        public Point(long x, long y)
        {
            X = x;
            Y = y;
        }

        public static bool operator ==(Point p1, Point p2)
        {
            return (p1.X == p2.X && p1.Y == p2.Y);
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return (p1.X != p2.X || p1.Y != p2.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                return ((Point)obj == this);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{0},{1}", this.X, this.Y);
        }
    }
}
