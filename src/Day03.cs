using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
    public class Day03 : IDay
    {
        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)} (Expected: 121163)");
        }

        public void Part2()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(input)} (Expected: 943)");
        }

        private static int SolvePart1(string input)
        {
            var linePattern =
                new Regex("#(?<id>[0-9]+) @ (?<y>[0-9]+),(?<x>[0-9]+): (?<width>[0-9]+)x(?<height>[0-9]+)");

            var fabric = new int[1000, 1000];

            var overlap = 0;

            var lines = input.Trim().Split("\n");
            foreach (var line in lines.Select(l => l.Trim()))
            {
                var match = linePattern.Match(line);

                var id = int.Parse(match.Groups["id"].Value);
                var startY = int.Parse(match.Groups["y"].Value);
                var startX = int.Parse(match.Groups["x"].Value);
                var width = int.Parse(match.Groups["width"].Value);
                var height = int.Parse(match.Groups["height"].Value);

                for (var y = 0; y < width; y++)
                for (var x = 0; x < height; x++)
                {
                    var alreadyUsed = fabric[startX + x, startY + y] > 0;
                    var alreadyOverlapping = fabric[startX + x, startY + y] == -1;

                    if (alreadyUsed)
                    {
                        overlap++;
                        fabric[startX + x, startY + y] = -1;
                    }
                    else if (alreadyOverlapping)
                    {
                        // Do not count twice
                    }
                    else
                    {
                        fabric[startX + x, startY + y] = id;
                    }
                }
            }

            return overlap;
        }

        private static int SolvePart2(string input)
        {
            var linePattern =
                new Regex("#(?<id>[0-9]+) @ (?<y>[0-9]+),(?<x>[0-9]+): (?<width>[0-9]+)x(?<height>[0-9]+)");
            var lines = input.Trim().Split("\n");

            var fabric = new int[1000, 1000];

            // Place all rectangles
            foreach (var line in lines.Select(l => l.Trim()))
            {
                var match = linePattern.Match(line);

                var id = int.Parse(match.Groups["id"].Value);
                var startY = int.Parse(match.Groups["y"].Value);
                var startX = int.Parse(match.Groups["x"].Value);
                var width = int.Parse(match.Groups["width"].Value);
                var height = int.Parse(match.Groups["height"].Value);

                for (var offsetY = 0; offsetY < width; offsetY++)
                for (var offsetX = 0; offsetX < height; offsetX++)
                {
                    var isAlreadyUsed = fabric[startX + offsetX, startY + offsetY] != 0;
                    if (isAlreadyUsed)
                    {
                        fabric[startX + offsetX, startY + offsetY] = -1;
                    }
                    else
                    {
                        fabric[startX + offsetX, startY + offsetY] = id;
                    }
                }
            }

            // Check each rectangle for intersections
            foreach (var line in lines.Select(l => l.Trim()))
            {
                var match = linePattern.Match(line);

                var id = int.Parse(match.Groups["id"].Value);
                var startY = int.Parse(match.Groups["y"].Value);
                var startX = int.Parse(match.Groups["x"].Value);
                var width = int.Parse(match.Groups["width"].Value);
                var height = int.Parse(match.Groups["height"].Value);

                var isUntouched = true;
                for (var offsetY = 0; offsetY < width; offsetY++)
                for (var offsetX = 0; offsetX < height; offsetX++)
                {
                    if (fabric[startX + offsetX, startY + offsetY] != id)
                    {
                        isUntouched = false;
                    }
                }

                if (isUntouched)
                {
                    return id;
                }
            }

            return -1;
        }
    }
}