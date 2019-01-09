using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
    public class Day10 : IDay
    {
        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 1: ... (Expected: HRPHBRKG)");
        }

        public void Part2()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart1(input)}  (Expected: 10355)");
        }

        private static int SolvePart1(string input)
        {
            var linePattern = new Regex("position=<\\s*(?<x>-?\\d+),\\s*(?<y>-?\\d+)> velocity=<\\s*(?<vx>-?\\d+),\\s*(?<vy>-?\\d+)>");

            var points = new List<GridPoint>();
            foreach (var s in input.Split("\n").Select(l => l.Trim()))
            {
                var match = linePattern.Match(s);
                var x = int.Parse(match.Groups["x"].Value);
                var y = int.Parse(match.Groups["y"].Value);
                var vx = int.Parse(match.Groups["vx"].Value);
                var vy = int.Parse(match.Groups["vy"].Value);

                points.Add(new GridPoint(x, y, vx, vy));
            }

            var smallestGridSize = int.MaxValue;
            var previousPoints = points.ToList();

            var second = 0;
            while (true)
            {
                // Move points
                var currentPoints = new List<GridPoint>();
                foreach (var gridPoint in previousPoints)
                {
                    currentPoints.Add(new GridPoint(gridPoint.X + gridPoint.VX, gridPoint.Y + gridPoint.VY, gridPoint.VX, gridPoint.VY));
                }

                var gridSize = currentPoints.Max(g => g.X) + currentPoints.Max(g => g.Y);

                // previous iteration was best
                if (gridSize > smallestGridSize)
                {
                    Print(previousPoints);
                    return second;
                }

                smallestGridSize = gridSize;

                previousPoints = currentPoints;
                second++;
            }
        }

        private static void Print(IReadOnlyCollection<GridPoint> points)
        {
            var maxX = points.Max(g => g.X) + 1;
            var minX = points.Min(g => g.X);
            var maxY = points.Max(g => g.Y) + 1;
            var minY = points.Min(g => g.Y);

            var sky = new bool[maxX - minX, maxY - minY];
            foreach (var gridPoint in points)
            {
                sky[gridPoint.X - minX, gridPoint.Y - minY] = true;
            }

            for (var i = 0; i < sky.GetUpperBound(1) + 1; i++)
            {
                for (var j = 0; j < sky.GetUpperBound(0) + 1; j++)
                {
                    Console.Write(sky[j, i] ? "#" : " ");
                }

                Console.Write(Environment.NewLine);
            }
        }

        private struct GridPoint
        {
            public GridPoint(int x, int y, int vx, int vy)
            {
                X = x;
                Y = y;
                VX = vx;
                VY = vy;
            }

            public int X { get; }
            public int Y { get; }
            public int VX { get; }
            public int VY { get; }
        }
    }
}