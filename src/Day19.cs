using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
    public class Day19 : IDay
    {
        private static readonly Dictionary<string, Action<long[], int, int, int>> Instructions = new Dictionary<string, Action<long[], int, int, int>>
        {
            {"addr", (r, a, b, c) => r[c] = r[a] + r[b]},
            {"addi", (r, a, b, c) => r[c] = r[a] + b},
            {"mulr", (r, a, b, c) => r[c] = r[a] * r[b]},
            {"muli", (r, a, b, c) => r[c] = r[a] * b},
            {"banr", (r, a, b, c) => r[c] = r[a] & r[b]},
            {"bani", (r, a, b, c) => r[c] = r[a] & b},
            {"borr", (r, a, b, c) => r[c] = r[a] | r[b]},
            {"bori", (r, a, b, c) => r[c] = r[a] | b},
            {"setr", (r, a, b, c) => r[c] = r[a]},
            {"seti", (r, a, b, c) => r[c] = a},
            {"gtir", (r, a, b, c) => r[c] = a > r[b] ? 1 : 0},
            {"gtri", (r, a, b, c) => r[c] = r[a] > b ? 1 : 0},
            {"gtrr", (r, a, b, c) => r[c] = r[a] > r[b] ? 1 : 0},
            {"eqir", (r, a, b, c) => r[c] = a == r[b] ? 1 : 0},
            {"eqri", (r, a, b, c) => r[c] = r[a] == b ? 1 : 0},
            {"eqrr", (r, a, b, c) => r[c] = r[a] == r[b] ? 1 : 0}
        };

        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)} (Expected: 878)");
        }

        public void Part2()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(input)} (Expected: 11510496)");
        }

        private static int SolvePart1(string input)
        {
            var insPattern = new Regex("(\\w+) (\\d+) (\\d+) (\\d+)", RegexOptions.Compiled);
            var lines = input.Split("\n").Select(x => x.Trim()).ToList();

            var register = new long[] {0, 0, 0, 0, 0, 0};
            var ip = 0;
            var iRegister = int.Parse(Regex.Match(lines[0], "\\d").Value);

            var insList = lines.Skip(1).ToList();
            while (ip < insList.Count)
            {
                var match = insPattern.Match(insList[ip]);
                var ins = match.Groups[1].Value;
                var a = int.Parse(match.Groups[2].Value);
                var b = int.Parse(match.Groups[3].Value);
                var c = int.Parse(match.Groups[4].Value);

                register[iRegister] = ip;
                Instructions[ins].Invoke(register, a, b, c);
                ip = (int) register[iRegister];

                ip++;
            }

            return (int) register[0];
        }


        /// <summary>
        ///     Calculates the sum of the factors
        ///     for r[3] .. 10551277
        ///     for r[1] .. 10551277
        ///     if r[3] * r[1] == 10551277
        ///     r[0] += r[3]
        /// </summary>
        private static int SolvePart2(string input)
        {
            var insPattern = new Regex("(\\w+) (\\d+) (\\d+) (\\d+)", RegexOptions.Compiled);
            var lines = input.Split("\n").Select(x => x.Trim()).ToList();

            var register = new long[] {1, 0, 0, 0, 0, 0};
            var ip = 0;
            var iRegister = int.Parse(Regex.Match(lines[0], "\\d").Value);

            var insList = lines.Skip(1).ToList();
            var target = 0L;
            while (ip < insList.Count)
            {
                var match = insPattern.Match(insList[ip]);
                var ins = match.Groups[1].Value;
                var a = int.Parse(match.Groups[2].Value);
                var b = int.Parse(match.Groups[3].Value);
                var c = int.Parse(match.Groups[4].Value);

                // Intercept start of loops
                if (ip == 1)
                {
                    // Find target value
                    target = register.Max(x => x);
                    break;
                }

                register[iRegister] = ip;
                Instructions[ins].Invoke(register, a, b, c);
                ip = (int) register[iRegister];
                ip++;
            }

            var sum = 0;
            for (var i = 1; i <= target; i++)
            {
                if (target % i == 0)
                {
                    sum += i;
                }
            }

            return sum;
        }
    }
}