using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2018
{
    public class Day13Cart
    {
        public int Y { get; set; }
        public int X { get; set; }
        public int CrossingCount { get; set; }
        public char Direction { get; set; }
        public char Beneath { get; set; }

        public Day13Cart(int x, int y, char direction, char beneath)
        {
            Y = x;
            X = y;
            CrossingCount = 0;
            Direction = direction;
            Beneath = beneath;
        }
    }

    [TestClass]
    public class Day13Part1
    {
        [TestMethod]
        public void MineCartMadness()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day13.txt");
            Console.WriteLine(GetType().Name + ": " + Solve(input));
        }

        private static string Solve(string input)
        {
            var curveMapping = new Dictionary<(char, char), char>();
            curveMapping.Add(('/', '^'), '>');
            curveMapping.Add(('/', '>'), '^');
            curveMapping.Add(('/', 'v'), '<');
            curveMapping.Add(('/', '<'), 'v');
            curveMapping.Add(('\\', '^'), '<');
            curveMapping.Add(('\\', '>'), 'v');
            curveMapping.Add(('\\', 'v'), '>');
            curveMapping.Add(('\\', '<'), '^');

            var crossingMapping = new Dictionary<(int, char), char>();
            crossingMapping.Add((0, 'v'), '>');
            crossingMapping.Add((0, '>'), '^');
            crossingMapping.Add((0, '<'), 'v');
            crossingMapping.Add((0, '^'), '<');
            crossingMapping.Add((2, 'v'), '<');
            crossingMapping.Add((2, '>'), 'v');
            crossingMapping.Add((2, '<'), '^');
            crossingMapping.Add((2, '^'), '>');

            var lines = input.Split("\n").Select(x => x.TrimEnd().ToCharArray()).ToArray();

            // Find carts
            var carts = new List<Day13Cart>();
            for (var x = 0; x < lines.Length; x++)
            for (var y = 0; y < lines[x].Length; y++)
            {
                var current = lines[x][y];
                if (current == 'v' || current == '^')
                {
                    carts.Add(new Day13Cart(x, y, current, '|'));
                }
                else if (current == '<' || current == '>')
                {
                    carts.Add(new Day13Cart(x, y, current, '-'));
                }
            }

            var i = 0;
            while (true)
            {
                i++;
                var currentCarts = carts.OrderBy(c => c.Y).ThenBy(c => c.X);

                foreach (var cart in currentCarts)
                {
                    var y2 = cart.Y;
                    var x2 = cart.X;

                    switch (cart.Direction)
                    {
                        case '^':
                            y2 -= 1;
                            break;
                        case '>':
                            x2 += 1;
                            break;
                        case '<':
                            x2 -= 1;
                            break;
                        case 'v':
                            y2 += 1;
                            break;
                    }

                    // Check for crash
                    if (lines[y2][x2] == '^' || lines[y2][x2] == '>' || lines[y2][x2] == '<' || lines[y2][x2] == 'v')
                    {
                        return $"x: {x2} y: {y2} i: {i}";
                    }

                    if (lines[y2][x2] == '+')
                    {
                        lines[cart.Y][cart.X] = cart.Beneath;
                        cart.Beneath = lines[y2][x2];

                        cart.Direction = crossingMapping.GetValueOrDefault((cart.CrossingCount % 3, cart.Direction), cart.Direction);
                        lines[y2][x2] = cart.Direction;

                        cart.CrossingCount++;
                    }
                    else if (lines[y2][x2] == '|' || lines[y2][x2] == '-')
                    {
                        lines[cart.Y][cart.X] = cart.Beneath;
                        cart.Beneath = lines[y2][x2];
                        lines[y2][x2] = cart.Direction;
                    }
                    else
                    {
                        lines[cart.Y][cart.X] = cart.Beneath;
                        cart.Beneath = lines[y2][x2];

                        cart.Direction = curveMapping[(lines[y2][x2], cart.Direction)];
                        lines[y2][x2] = cart.Direction;
                    }

                    cart.Y = y2;
                    cart.X = x2;
                }
            }
        }
    }

    [TestClass]
    public class Day13Part2
    {
        [TestMethod]
        public void MineCartMadness()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day13.txt");
            Console.WriteLine(GetType().Name + ": " + Solve(input));
        }

        private static string Solve(string input)
        {
            var curveMapping = new Dictionary<(char, char), char>();
            curveMapping.Add(('/', '^'), '>');
            curveMapping.Add(('/', '>'), '^');
            curveMapping.Add(('/', 'v'), '<');
            curveMapping.Add(('/', '<'), 'v');
            curveMapping.Add(('\\', '^'), '<');
            curveMapping.Add(('\\', '>'), 'v');
            curveMapping.Add(('\\', 'v'), '>');
            curveMapping.Add(('\\', '<'), '^');

            var crossingMapping = new Dictionary<(int, char), char>();
            crossingMapping.Add((0, 'v'), '>');
            crossingMapping.Add((0, '>'), '^');
            crossingMapping.Add((0, '<'), 'v');
            crossingMapping.Add((0, '^'), '<');
            crossingMapping.Add((2, 'v'), '<');
            crossingMapping.Add((2, '>'), 'v');
            crossingMapping.Add((2, '<'), '^');
            crossingMapping.Add((2, '^'), '>');

            var lines = input.Split("\n").Select(x => x.TrimEnd().ToCharArray()).ToArray();
            var carts = new List<Day13Cart>();

            // Find carts
            for (var x = 0; x < lines.Length; x++)
            for (var y = 0; y < lines[x].Length; y++)
            {
                var current = lines[x][y];
                if (current == 'v' || current == '^')
                {
                    carts.Add(new Day13Cart(x, y, current, '|'));
                }
                else if (current == '<' || current == '>')
                {
                    carts.Add(new Day13Cart(x, y, current, '-'));
                }
            }

            var i = 0;
            while (true)
            {
                i++;

                var orderedCarts = carts.OrderBy(c => c.Y).ThenBy(c => c.X).ToList();
                if (orderedCarts.Count == 1)
                {
                    return $"x: {orderedCarts[0].X} y: {orderedCarts[0].Y} i: {i}";
                }

                var crashed = new List<Day13Cart>();
                foreach (var cart in orderedCarts)
                {
                    if (crashed.Contains(cart))
                    {
                        continue;
                    }

                    var y2 = cart.Y;
                    var x2 = cart.X;

                    switch (cart.Direction)
                    {
                        case '^':
                            y2 -= 1;
                            break;
                        case 'v':
                            y2 += 1;
                            break;
                        case '<':
                            x2 -= 1;
                            break;
                        case '>':
                            x2 += 1;
                            break;
                    }

                    // Check for crash
                    if (lines[y2][x2] == '^' || lines[y2][x2] == '>' || lines[y2][x2] == '<' || lines[y2][x2] == 'v')
                    {
                        var crashedCard = orderedCarts.First(c => c.X == x2 && c.Y == y2);

                        carts.Remove(crashedCard);
                        lines[crashedCard.Y][crashedCard.X] = crashedCard.Beneath;

                        carts.Remove(cart);
                        lines[cart.Y][cart.X] = cart.Beneath;

                        crashed.Add(cart);
                        crashed.Add(crashedCard);

                        continue;
                    }

                    if (lines[y2][x2] == '+')
                    {
                        lines[cart.Y][cart.X] = cart.Beneath;
                        cart.Beneath = lines[y2][x2];

                        cart.Direction = crossingMapping.GetValueOrDefault((cart.CrossingCount % 3, cart.Direction), cart.Direction);
                        lines[y2][x2] = cart.Direction;

                        cart.CrossingCount++;
                    }
                    else if (lines[y2][x2] == '|' || lines[y2][x2] == '-')
                    {
                        lines[cart.Y][cart.X] = cart.Beneath;
                        cart.Beneath = lines[y2][x2];
                        lines[y2][x2] = cart.Direction;
                    }
                    else
                    {
                        lines[cart.Y][cart.X] = cart.Beneath;
                        cart.Beneath = lines[y2][x2];

                        cart.Direction = curveMapping[(lines[y2][x2], cart.Direction)];
                        lines[y2][x2] = cart.Direction;
                    }

                    cart.Y = y2;
                    cart.X = x2;
                }
            }
        }
    }
}