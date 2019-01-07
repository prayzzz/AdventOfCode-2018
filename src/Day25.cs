using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode2018
{
    public class Day25 : IDay
    {
        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)} (Expected: 327)");
        }

        public void Part2()
        {
        }

        private static int SolvePart1(string input)
        {
            var points = new List<GridPoint>();
            foreach (var line in input.Split("\n").Select(l => l.Trim()))
            {
                var s = line.Split(",");
                points.Add(new GridPoint(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]), int.Parse(s[3])));
            }

            var clusters = new List<List<GridPoint>>();
            var q = points.ToList();
            while (q.Count > 0)
            {
                var toRemove = new List<GridPoint>();

                foreach (var p1 in q)
                {
                    foreach (var cluster in clusters)
                    foreach (var clusterP in cluster)
                    {
                        if (3 >= Math.Abs(p1.X - clusterP.X) + Math.Abs(p1.Y - clusterP.Y) + Math.Abs(p1.Z - clusterP.Z) +
                            Math.Abs(p1.T - clusterP.T))
                        {
                            cluster.Add(p1);
                            toRemove.Add(p1);
                            goto nextPoint;
                        }
                    }

                    nextPoint: ;
                }

                // if nothing was added to a cluster, create a new cluster with the first remaining element
                if (toRemove.Count == 0)
                {
                    clusters.Add(new List<GridPoint> {q.First()});
                    toRemove.Add(q.First());
                }

                toRemove.ForEach(p => q.Remove(p));
            }

            return clusters.Count;
        }

        [DebuggerDisplay("{X},{Y},{Z},{T}")]
        private class GridPoint
        {
            public GridPoint(int x, int y, int z, int t)
            {
                X = x;
                Y = y;
                Z = z;
                T = t;
            }

            public int X { get; }
            public int Y { get; }
            public int Z { get; }
            public int T { get; }
        }
    }
}