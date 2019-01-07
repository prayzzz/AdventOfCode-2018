using System;

namespace AdventOfCode2018
{
    public class Day11 : IDay
    {
        public void Part1()
        {
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(1723)} (Expected: 34,13)");
        }

        public void Part2()
        {
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(1723)} (Expected: 280,218,11)");
        }

        private static string SolvePart1(int input)
        {
            var maxX = 0;
            var maxY = 0;
            var maxPower = 0;
            for (var startX = 1; startX < 300; startX++)
            for (var startY = 1; startY < 300; startY++)
            {
                var power = 0;
                for (var x = startX; x < startX + 3; x++)
                for (var y = startY; y < startY + 3; y++)
                {
                    power += GetPowerLevel(input, x, y);
                }

                if (power > maxPower)
                {
                    maxX = startX;
                    maxY = startY;
                    maxPower = power;
                }
            }

            return $"{maxX},{maxY}";
        }

        private static int GetPowerLevel(int input, int x, int y)
        {
            var rackId = x + 10;
            var powerLevel = (rackId * y + input) * rackId;
            return powerLevel / 100 % 10 - 5;
        }

        /// <summary>
        ///     Values after size 20 are getting lower
        /// </summary>
        private static string SolvePart2(int input)
        {
            var cells = new int[300, 300];

            for (var x = 0; x < 300; x++)
            for (var y = 0; y < 300; y++)
            {
                cells[x, y] = GetPowerLevel(input, x, y);
            }

            (int x, int y, int size, int power) result = (0, 0, 0, 0);

            for (var startX = 0; startX < 300; startX++)
            for (var startY = 0; startY < 300; startY++)
            {
                var maxSize = Math.Min(299 - Math.Max(startX, startY), 20);
                for (var size = 0; size < maxSize; size++)
                {
                    var power = 0;
                    for (var x = startX; x < startX + size && x < 300; x++)
                    for (var y = startY; y < startY + size && y < 300; y++)
                    {
                        power += cells[x, y];
                    }

                    if (power > result.power)
                    {
                        result = (startX, startY, size, power);
                    }
                }
            }

            return $"{result.x},{result.y},{result.size}";
        }
    }
}