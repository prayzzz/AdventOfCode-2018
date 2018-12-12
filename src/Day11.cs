using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2018
{
    [TestClass]
    public class Day11Part1
    {
        [TestMethod]
        public void ChronalCharge()
        {
            Console.WriteLine(GetType().Name + ": " + Solve(18));
        }

        private static string Solve(int input)
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

            return $"{maxX},{maxY} Power: {maxPower}";
        }

        public static int GetPowerLevel(int input, int x, int y)
        {
            var rackId = x + 10;
            var powerLevel = (rackId * y + input) * rackId;
            return powerLevel / 100 % 10 - 5;
        }
    }

    [TestClass]
    public class Day11Part2
    {
        [TestMethod]
        public void ChronalCharge()
        {
            Console.WriteLine(GetType().Name + ": " + Solve(1723));
        }

        /// <summary>
        /// Values after size 20 are getting lower
        /// </summary>
        private static string Solve(int input)
        {
            var cells = new int[300, 300];

            for (var x = 0; x < 300; x++)
            for (var y = 0; y < 300; y++)
            {
                cells[x, y] = Day11Part1.GetPowerLevel(input, x, y);
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

            return $"{result.x},{result.y},{result.size} Power: {result.power}";
        }
    }
}