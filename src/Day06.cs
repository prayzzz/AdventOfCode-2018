using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    public class Day06 : IDay
    {
        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)} (Expected: 3238)");
        }

        public void Part2()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(input)} (Expected: 45046)");
        }

        private static int SolvePart1(string input)
        {
            var lines = input.Split("\n");

            var coordinates = new List<(int x, int y)>();
            foreach (var line in lines)
            {
                var coord = line.Split(",");
                coordinates.Add((int.Parse(coord[0].Trim()), int.Parse(coord[1].Trim())));
            }

            var maxX = coordinates.Max(x => x.x) + 1;
            var maxY = coordinates.Max(x => x.y) + 1;

            var max = Math.Max(maxX, maxY);

            var area = new (int index, int distance)[max, max];
            var areaCount = new int[coordinates.Count];
            for (var x = 0; x <= area.GetUpperBound(0); x++)
            for (var y = 0; y <= area.GetUpperBound(1); y++)
            {
                var doubled = false;
                var minDistance = int.MaxValue;
                var index = -1;
                for (var i = 0; i < coordinates.Count; i++)
                {
                    var (cX, cY) = coordinates[i];

                    var distX = Math.Abs(cX - x);
                    var distY = Math.Abs(cY - y);
                    var distance = distX + distY;

                    if (distance < minDistance)
                    {
                        index = i;
                        doubled = false;
                        minDistance = distance;
                    }
                    else if (distance == minDistance)
                    {
                        doubled = true;
                    }
                }

                if (doubled)
                {
                    area[x, y] = (-1, -1);
                }
                else
                {
                    area[x, y] = (index, minDistance);
                }
            }

            for (var x = 0; x <= area.GetUpperBound(0); x++)
            for (var y = 0; y <= area.GetUpperBound(1); y++)
            {
                var entry = area[x, y];

                if (area[x, 0].index == entry.index ||
                    area[0, y].index == entry.index ||
                    area[x, area.GetUpperBound(1)].index == entry.index ||
                    area[area.GetUpperBound(0), y].index == entry.index)
                {
                    continue;
                }

                if (entry.index != -1)
                {
                    areaCount[entry.index]++;
                }
            }

            return areaCount.Max();
        }

        private static int SolvePart2(string input)
        {
            var lines = input.Split("\n");

            var coordinates = new List<(int x, int y)>();
            foreach (var line in lines)
            {
                var coord = line.Split(",");
                coordinates.Add((int.Parse(coord[0].Trim()), int.Parse(coord[1].Trim())));
            }

            var maxX = coordinates.Max(x => x.x) + 1;
            var maxY = coordinates.Max(x => x.y) + 1;
            var areaSize = Math.Max(maxX, maxY);

            var areaCount = 0;
            for (var x = 0; x < areaSize; x++)
            for (var y = 0; y < areaSize; y++)
            {
                var distanceSum = coordinates.Sum(c => Math.Abs(c.x - x) + Math.Abs(c.y - y));
                if (distanceSum < 10000)
                {
                    areaCount++;
                }
            }

            return areaCount;
        }
    }
}