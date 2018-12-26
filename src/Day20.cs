using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    public class Day20 : IDay
    {
        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)} (Expected: 3502)");
        }

        public void Part2()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(input)} (Expected: 8000)");
        }

        /// <summary>
        ///     Build list of strings with all possible paths.
        ///     Skips circular sub paths.
        /// </summary>
        private static int SolvePart1(string input)
        {
            var index = 0;

            var paths = Step();

            List<string> Step()
            {
                var allPaths = new List<string>();
                var currentPaths = new List<string>();

                while (true)
                {
                    var current = input[++index];

                    switch (current)
                    {
                        case '(':
                            var subPaths = Step();

                            if (subPaths.Any())
                            {
                                var newPaths = new List<string>();
                                foreach (var i in currentPaths)
                                {
                                    foreach (var j in subPaths)
                                    {
                                        newPaths.Add(i + j);
                                    }
                                }

                                currentPaths = newPaths;
                            }

                            break;
                        case '|':
                            allPaths.AddRange(currentPaths);
                            currentPaths.Clear();
                            break;
                        case ')':
                            if (input[index - 1] == '|')
                            {
                                return new List<string>();
                            }

                            allPaths.AddRange(currentPaths);
                            return allPaths;
                        case '$':
                            allPaths.AddRange(currentPaths);
                            return allPaths;
                        default:
                            if (currentPaths.Any())
                            {
                                for (var i = 0; i < currentPaths.Count; i++)
                                {
                                    currentPaths[i] += current;
                                }
                            }
                            else
                            {
                                currentPaths.Add(current.ToString());
                            }

                            break;
                    }
                }
            }

            return paths.Max(x => x.Length);
        }

        /// <summary>
        ///     Couldn't make my solution from part 1 work for part 2.
        /// </summary>
        private static int SolvePart2(string input)
        {
            var current = (X: 0, Y: 0, Dist: 0);
            var stack = new Stack<(int, int, int)>();
            var map = new Dictionary<(int, int), int>();

            foreach (var c in input)
            {
                switch (c)
                {
                    case 'N':
                        AddToMap(0, -1);
                        break;
                    case 'S':
                        AddToMap(0, 1);
                        break;
                    case 'E':
                        AddToMap(1, 0);
                        break;
                    case 'W':
                        AddToMap(-1, 0);
                        break;
                    case '(':
                        stack.Push(current);
                        break;
                    case ')':
                        current = stack.Pop();
                        break;
                    case '|':
                        current = stack.Peek();
                        break;
                    case '$':
                        break;
                }
            }

            void AddToMap(int dX, int dY)
            {
                var x = current.X + dX;
                var y = current.Y + dY;

                var dist = map.GetValueOrDefault((x, y), int.MaxValue);
                dist = Math.Min(dist, current.Dist + 1);
                current = (x, y, dist);

                map.AddOrReplace((x, y), dist);
            }

            return map.Values.Count(x => x >= 1000);
        }
    }
}