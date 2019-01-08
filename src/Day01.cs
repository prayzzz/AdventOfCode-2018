using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    public class Day01 : IDay
    {
        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)} (Expected: 553)");
        }

        public void Part2()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(input)} (Expected: 78724)");
        }

        private static int SolvePart1(string input)
        {
            var lines = input.Trim().Split("\n");

            var result = 0;
            foreach (var line in lines.Select(l => l.Trim()))
            {
                switch (line[0])
                {
                    case '+':
                        result += Convert.ToInt32(line.Substring(1));
                        break;
                    case '-':
                        result -= Convert.ToInt32(line.Substring(1));
                        break;
                    default:
                        throw new ArgumentException($"Illegal operator '{line[0]}'");
                }
            }

            return result;
        }

        /// <summary>
        ///     Input must contain trailing blank line.
        /// </summary>
        private static int SolvePart2(string input)
        {
            const char lineEnd = '\n';

            var inputSpan = input.AsSpan();

            var frequencies = new HashSet<int>();
            var frequency = 0;

            while (true)
            {
                // Position of the first char of a line
                var lineStart = 0;

                for (var j = 0; j < inputSpan.Length; j++)
                {
                    if (inputSpan[j] == lineEnd)
                    {
                        var line = inputSpan.Slice(lineStart, j - lineStart).Trim();

                        switch (line[0])
                        {
                            case '+':
                                frequency += int.Parse(line.Slice(1));
                                break;
                            case '-':
                                frequency -= int.Parse(line.Slice(1));
                                break;
                            default:
                                throw new ArgumentException($"Illegal operator '{line[0]}'");
                        }

                        lineStart = j + 1;

                        if (!frequencies.Add(frequency))
                        {
                            return frequency;
                        }
                    }
                }
            }
        }
    }
}