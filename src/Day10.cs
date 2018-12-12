using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2018
{
    [TestClass]
    public class Day10Part1
    {
        [TestMethod]
        public void TheStarsAlign()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day10.txt");
            Console.WriteLine(GetType().Name + ": " + Solve(input));
        }

        private static int Solve(string input)
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

            var maxX = points.Max(g => g.X);
            var maxY = points.Max(g => g.Y);
            var max = Math.Max(maxX, maxY) + 2;

            var second = 0;
            while (true)
            {
                second++;

                // Move points
                foreach (var gridPoint in points)
                {
                    gridPoint.X += gridPoint.VX;
                    gridPoint.Y += gridPoint.VY;
                }

                // Calculate points near to each other
                var count = 0;
                foreach (var gridPoint1 in points)
                foreach (var gridPoint2 in points)
                {
                    if (gridPoint1.X != gridPoint2.X && gridPoint1.Y != gridPoint2.Y)
                    {
                        var distX = Math.Abs(gridPoint1.X - gridPoint2.X);
                        var distY = Math.Abs(gridPoint1.Y - gridPoint2.Y);

                        if (distX + distY <= 2)
                        {
                            count++;
                        }
                    }
                }

                // Print result
                if (count > 550)
                {
                    Console.WriteLine("Points near to each other: " + count);

                    Print(points, max);
                    break;
                }
            }

            return second;
        }

        private static void Print(List<GridPoint> points, int max)
        {
            var maxX = points.Where(g => g.X < max).Max(g => g.X) + 1;
            var minX = points.Where(g => g.X > 0).Min(g => g.X);
            var maxY = points.Where(g => g.Y < max).Max(g => g.Y) + 1;
            var minY = points.Where(g => g.Y > 0).Min(g => g.Y);

            var sky = new bool[maxX - minX, maxY - minY];
            foreach (var gridPoint in points)
            {
                if (gridPoint.X > -1 && gridPoint.X < max && gridPoint.Y > -1 && gridPoint.Y < max)
                {
                    sky[gridPoint.X - minX, gridPoint.Y - minY] = true;
                }
            }

            for (var i = 0; i < sky.GetUpperBound(1) - 1; i++)
            {
                for (var j = 0; j < sky.GetUpperBound(0) - 1; j++)
                {
                    Console.Write(sky[j, i] ? "#" : ".");
                }

                Console.Write(Environment.NewLine);
            }
        }

        class GridPoint
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int VX { get; set; }
            public int VY { get; set; }

            public GridPoint(int x, int y, int vx, int vy)
            {
                X = x;
                Y = y;
                VX = vx;
                VY = vy;
            }
        }
    }

    [TestClass]
    public class Day10Part2
    {
        [TestMethod]
        public void TheStarsAlign()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day10.txt");
            Console.WriteLine(GetType().Name + ": " + Solve(input));
        }

        private static int Solve(string input)
        {
            return -1;
        }
    }
}