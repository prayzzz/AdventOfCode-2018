using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
    public class Day22 : IDay
    {
        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)} (Expected: 5400)");
        }

        public void Part2()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(input)} (Expected: 1048)");
        }

        private static int SolvePart1(string input)
        {
            var lines = input.Split("\n");

            var depth = int.Parse(Regex.Match(lines[0], "\\d+").Value);
            var coord = Regex.Match(lines[1], "\\d+,\\d+").Value.Split(",");
            var targetX = int.Parse(coord[0]);
            var targetY = int.Parse(coord[1]);

            var max = Math.Max(targetX, targetY) + 10;
            var map = new int[max, max];

            for (var x = 0; x < max; x++)
            for (var y = 0; y < max; y++)
            {
                int geoIndex;
                if (x == 0 && y == 0 || x == targetX && y == targetY)
                {
                    geoIndex = 0;
                }
                else if (y == 0)
                {
                    geoIndex = x * 16807;
                }
                else if (x == 0)
                {
                    geoIndex = y * 48271;
                }
                else
                {
                    geoIndex = map[x - 1, y] * map[x, y - 1];
                }

                map[x, y] = (geoIndex + depth) % 20183;
            }

            var result = 0;
            for (var x = 0; x <= targetX; x++)
            for (var y = 0; y <= targetY; y++)
            {
                result += map[x, y] % 3;
            }

            return result;
        }

        private static int SolvePart2(string input)
        {
            var lines = input.Split("\n");

            var depth = int.Parse(Regex.Match(lines[0], "\\d+").Value);
            var coord = Regex.Match(lines[1], "\\d+,\\d+").Value.Split(",");
            var targetX = int.Parse(coord[0]);
            var targetY = int.Parse(coord[1]);

            var max = Math.Max(targetX, targetY) + 10;
            var map = new int[max, max];

            for (var x = 0; x < max; x++)
            for (var y = 0; y < max; y++)
            {
                int geoIndex;
                if (x == 0 && y == 0 || x == targetX && y == targetY)
                {
                    geoIndex = 0;
                }
                else if (y == 0)
                {
                    geoIndex = x * 16807;
                }
                else if (x == 0)
                {
                    geoIndex = y * 48271;
                }
                else
                {
                    geoIndex = map[x - 1, y] * map[x, y - 1];
                }

                map[x, y] = (geoIndex + depth) % 20183;
            }

            var neighbours = new List<(int X, int Y)> {(1, 0), (-1, 0), (0, 1), (0, -1)};

            // dijkstra?
            // take position with lowest distance from queue
            // add valid and unvisited neighbours with their distance to queue
            // repeat until target reached
            var seen = new HashSet<(int X, int Y, Tool Tool)>(1_000_000);
            var queue = new SortedList<int, (int X, int Y, int Dist, Tool Tool)>(10_000, new DuplicateKeyComparer());
            queue.Add(0, (0, 0, 0, Tool.Torch));

            while (queue.Count > 0)
            {
                var (x, y, dist, tool) = queue.Values[0];
                queue.RemoveAt(0);

                // Target reached?
                if (x == targetX && y == targetY && tool == Tool.Torch)
                {
                    return dist;
                }

                // Already visited?
                if (!seen.Add((x, y, tool)))
                {
                    continue;
                }

                foreach (Tool t in Enum.GetValues(typeof(Tool)))
                {
                    if (IsToolValid(map[x, y] % 3, t) && !seen.Contains((x, y, t)))
                    {
                        var nDist = dist + 7;
                        queue.Add(nDist, (x, y, nDist, t));
                    }
                }

                foreach (var (dX, dY) in neighbours)
                {
                    var nX = x + dX;
                    var nY = y + dY;

                    if (nX < 0 || nY < 0 || nX > max || nY > max)
                    {
                        continue;
                    }

                    if (IsToolValid(map[nX, nY] % 3, tool) && !seen.Contains((nX, nY, tool)))
                    {
                        var nDist = dist + 1;
                        queue.Add(nDist, (nX, nY, nDist, tool));
                    }
                }
            }

            bool IsToolValid(int type, Tool tool)
            {
                if (type == 0 && (tool == Tool.Torch || tool == Tool.Gear))
                {
                    return true;
                }

                if (type == 1 && (tool == Tool.Gear || tool == Tool.Neither))
                {
                    return true;
                }

                if (type == 2 && (tool == Tool.Torch || tool == Tool.Neither))
                {
                    return true;
                }

                return false;
            }

            return -1;
        }

        private enum Tool
        {
            Neither,
            Gear,
            Torch
        }
    }

    public class DuplicateKeyComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return x > y ? 1 : -1; // handle equal as lower is faster for this problem
        }
    }
}