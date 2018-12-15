using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    public class Day13 : IDay
    {
        private static readonly Dictionary<(char, char), char> CurveMapping = new Dictionary<(char, char), char>
        {
            {('/', '^'), '>'},
            {('/', '>'), '^'},
            {('/', 'v'), '<'},
            {('/', '<'), 'v'},
            {('\\', '^'), '<'},
            {('\\', '>'), 'v'},
            {('\\', 'v'), '>'},
            {('\\', '<'), '^'}
        };

        private static readonly Dictionary<(int, char), char> CrossingMapping = new Dictionary<(int, char), char>
        {
            {(0, 'v'), '>'},
            {(0, '>'), '^'},
            {(0, '<'), 'v'},
            {(0, '^'), '<'},
            {(2, 'v'), '<'},
            {(2, '>'), 'v'},
            {(2, '<'), '^'},
            {(2, '^'), '>'}
        };

        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)}");
        }

        public void Part2()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(input)}");
        }

        private static string SolvePart1(string input)
        {
            var lines = input.Split("\n").Select(x => x.TrimEnd().ToCharArray()).ToArray();

            // Find carts
            var carts = new List<Cart>();
            for (var x = 0; x < lines.Length; x++)
            for (var y = 0; y < lines[x].Length; y++)
            {
                var current = lines[x][y];
                if (current == 'v' || current == '^')
                {
                    carts.Add(new Cart(x, y, current, '|'));
                }
                else if (current == '<' || current == '>')
                {
                    carts.Add(new Cart(x, y, current, '-'));
                }
            }

            while (true)
            {
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
                        return $"{x2},{y2}";
                    }

                    if (lines[y2][x2] == '+')
                    {
                        lines[cart.Y][cart.X] = cart.Beneath;
                        cart.Beneath = lines[y2][x2];

                        cart.Direction = CrossingMapping.GetValueOrDefault((cart.CrossingCount % 3, cart.Direction), cart.Direction);
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

                        cart.Direction = CurveMapping[(lines[y2][x2], cart.Direction)];
                        lines[y2][x2] = cart.Direction;
                    }

                    cart.Y = y2;
                    cart.X = x2;
                }
            }
        }

        private static string SolvePart2(string input)
        {
            var lines = input.Split("\n").Select(x => x.TrimEnd().ToCharArray()).ToArray();
            var carts = new List<Cart>();

            // Find carts
            for (var x = 0; x < lines.Length; x++)
            for (var y = 0; y < lines[x].Length; y++)
            {
                var current = lines[x][y];
                if (current == 'v' || current == '^')
                {
                    carts.Add(new Cart(x, y, current, '|'));
                }
                else if (current == '<' || current == '>')
                {
                    carts.Add(new Cart(x, y, current, '-'));
                }
            }

            while (true)
            {
                var orderedCarts = carts.OrderBy(c => c.Y).ThenBy(c => c.X).ToList();
                if (orderedCarts.Count == 1)
                {
                    return $"{orderedCarts[0].X},{orderedCarts[0].Y}";
                }

                var crashed = new List<Cart>();
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

                        cart.Direction = CrossingMapping.GetValueOrDefault((cart.CrossingCount % 3, cart.Direction), cart.Direction);
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

                        cart.Direction = CurveMapping[(lines[y2][x2], cart.Direction)];
                        lines[y2][x2] = cart.Direction;
                    }

                    cart.Y = y2;
                    cart.X = x2;
                }
            }
        }

        private class Cart
        {
            public Cart(int x, int y, char direction, char beneath)
            {
                Y = x;
                X = y;
                CrossingCount = 0;
                Direction = direction;
                Beneath = beneath;
            }

            public int Y { get; set; }
            public int X { get; set; }
            public int CrossingCount { get; set; }
            public char Direction { get; set; }
            public char Beneath { get; set; }
        }
    }
}