using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
    public class Day17 : IDay
    {
        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)} (Expected: 39162)");
        }

        public void Part2()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(input)} (Expected: 32047)");
        }

        private static (CellType[,] grid, int xMin, int yMin) SimulateFlow(string input)
        {
            var lines = input.Split("\n").Select(l => l.Trim());
            var pattern1 = new Regex("x=(\\d+), y=(\\d+)\\.\\.(\\d+)");
            var pattern2 = new Regex("y=(\\d+), x=(\\d+)\\.\\.(\\d+)");

            var points = new List<GridPoint>();
            foreach (var line in lines)
            {
                var match = pattern1.Match(line);
                if (match.Success)
                    points.Add(new GridPoint(
                        int.Parse(match.Groups[1].Value),
                        int.Parse(match.Groups[1].Value),
                        int.Parse(match.Groups[2].Value),
                        int.Parse(match.Groups[3].Value))
                    );

                var match2 = pattern2.Match(line);
                if (match2.Success)
                    points.Add(new GridPoint(
                        int.Parse(match2.Groups[2].Value),
                        int.Parse(match2.Groups[3].Value),
                        int.Parse(match2.Groups[1].Value),
                        int.Parse(match2.Groups[1].Value))
                    );
            }

            var xMin = points.Min(point => Math.Min(point.X1, point.X2));
            var xMax = points.Max(point => Math.Max(point.X1, point.X2)) + 10;
            var yMin = points.Min(point => Math.Min(point.Y1, point.Y2));
            var yMax = points.Max(point => Math.Max(point.Y1, point.Y2));

            var grid = new CellType[xMax + 1, yMax + 1];

            // Set clay
            foreach (var gridPoint in points)
                for (var x = gridPoint.X1; x <= gridPoint.X2; x++)
                for (var y = gridPoint.Y1; y <= gridPoint.Y2; y++)
                    grid[x, y] = CellType.Clay;

            var queue = new HashSet<(int X, int Y)> { (500, 0) };
            while (queue.Count > 0)
            {
                var (x, y) = queue.First();
                queue.Remove(queue.First());

                if (y >= yMax)
                {
                    // end reached
                }
                else if (grid[x, y] == CellType.Water)
                {
                    // continue if already water
                }
                else if (grid[x, y + 1] == CellType.Sand)
                {
                    // fall further down
                    for (var dY = y + 1; dY <= yMax; dY++)
                        if (grid[x, dY] == CellType.Sand)
                        {
                            grid[x, dY] = CellType.WaterFlow;
                        }
                        else if (grid[x, dY] == CellType.Clay || grid[x, dY] == CellType.Water)
                        {
                            queue.Add((x, dY - 1));
                            break;
                        }
                        else if (grid[x, dY] == CellType.WaterFlow)
                        {
                            break;
                        }
                }
                else if ((grid[x, y + 1] == CellType.Clay || grid[x, y + 1] == CellType.Water) && CanSettle(grid, x, y))
                {
                    // settle water

                    for (var dX = x; dX < xMax; dX++)
                    {
                        if (grid[dX, y] == CellType.Clay) break;

                        grid[dX, y] = CellType.Water;
                    }

                    for (var dX = x; dX > 0; dX--)
                    {
                        if (grid[dX, y] == CellType.Clay) break;

                        grid[dX, y] = CellType.Water;
                    }

                    queue.Add((x, y - 1));
                }
                else if ((grid[x, y + 1] == CellType.Clay || grid[x, y + 1] == CellType.Water) && !CanSettle(grid, x, y))
                {
                    // spread flow

                    for (var dX = x; dX < xMax + 1; dX++)
                    {
                        if (grid[dX, y] == CellType.Clay) break;

                        grid[dX, y] = CellType.WaterFlow;

                        // if there's sand under the set waterflow, stop and continue with this point
                        if (grid[dX, y + 1] == CellType.Sand)
                        {
                            queue.Add((dX, y));
                            break;
                        }
                    }

                    for (var dX = x; dX > 0; dX--)
                    {
                        if (grid[dX, y] == CellType.Clay) break;

                        grid[dX, y] = CellType.WaterFlow;

                        // if there's sand under the set waterflow, stop and continue with this point
                        if (grid[dX, y + 1] == CellType.Sand)
                        {
                            queue.Add((dX, y));
                            break;
                        }
                    }
                }

                // Print(grid, xMin);
            }

            return (grid, xMin, yMin);
        }

        private static int SolvePart1(string input)
        {
            var (grid, xMin, yMin) = SimulateFlow(input);

            var result = 0;
            for (var x = xMin; x < grid.GetUpperBound(0) + 1; x++)
            for (var y = yMin; y < grid.GetUpperBound(1) + 1; y++)
                if (grid[x, y] == CellType.WaterFlow || grid[x, y] == CellType.Water)
                    result++;

            return result;
        }

        private static int SolvePart2(string input)
        {
            var (grid, xMin, yMin) = SimulateFlow(input);

            var result = 0;
            for (var x = xMin; x < grid.GetUpperBound(0) + 1; x++)
            for (var y = yMin; y < grid.GetUpperBound(1) + 1; y++)
                if (grid[x, y] == CellType.Water)
                    result++;

            return result;
        }

        /// <summary>
        ///     The current point is in a box, if clay found on both sides.
        ///     If we encounter sand under the current point, it's not inside a box or the box is broken.
        /// </summary>
        private static bool CanSettle(CellType[,] grid, int x, int y)
        {
            for (var dX = x; dX < grid.GetUpperBound(0); dX++)
            {
                if (grid[dX, y] == CellType.Clay) break;

                if (grid[dX, y + 1] == CellType.Sand) return false;
            }

            for (var dX = x; dX > 0; dX--)
            {
                if (grid[dX, y] == CellType.Clay) break;

                if (grid[dX, y + 1] == CellType.Sand) return false;
            }

            return true;
        }

        private static void Print(CellType[,] grid, int xMin)
        {
            const string fileName = "day17_map.txt";
            if (File.Exists(fileName)) File.Delete(fileName);

            for (var y = 0; y < grid.GetUpperBound(1) + 1; y++)
            {
                var stringBuilder = new StringBuilder();
                for (var x = xMin; x < grid.GetUpperBound(0) + 1; x++)
                    switch (grid[x, y])
                    {
                        case CellType.Sand:
                            stringBuilder.Append(".");
                            break;
                        case CellType.Clay:
                            stringBuilder.Append("#");
                            break;
                        case CellType.Water:
                            stringBuilder.Append("~");
                            break;
                        case CellType.WaterFlow:
                            stringBuilder.Append("|");
                            break;
                    }

                stringBuilder.AppendLine();
                File.AppendAllText(fileName, stringBuilder.ToString());
            }
        }

        private enum CellType
        {
            Sand,
            Clay,
            Water,
            WaterFlow
        }

        private class GridPoint
        {
            public GridPoint(int x1, int x2, int y1, int y2)
            {
                X1 = x1;
                X2 = x2;
                Y1 = y1;
                Y2 = y2;
            }

            public int X1 { get; }
            public int X2 { get; }
            public int Y1 { get; }
            public int Y2 { get; }
        }
    }
}