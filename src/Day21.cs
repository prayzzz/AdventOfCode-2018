using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
    public class Day21 : IDay
    {
        private static readonly Dictionary<string, Action<int[], int, int, int>> Instructions = new Dictionary<string, Action<int[], int, int, int>>
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
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)} (Expected: 12935354)");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1RevEng()} (Expected: 12935354) # Reverse Engineered");
        }

        public void Part2()
        {
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2()} (Expected: 12390302)");
        }

        private static int SolvePart1(string input)
        {
            var insPattern = new Regex("(\\w+) (\\d+) (\\d+) (\\d+)", RegexOptions.Compiled);
            var lines = input.Split("\n").Select(x => x.Trim()).ToList();

            var iRegister = int.Parse(Regex.Match(lines[0], "\\d").Value);
            var insList = lines.Skip(1).ToList();

            var ip = 0;
            var register = new[] {0, 0, 0, 0, 0, 0};

            while (ip < insList.Count)
            {
                if (ip == 28)
                {
                    return register[5];
                }

                var match = insPattern.Match(insList[ip]);
                var ins = match.Groups[1].Value;
                var a = int.Parse(match.Groups[2].Value);
                var b = int.Parse(match.Groups[3].Value);
                var c = int.Parse(match.Groups[4].Value);

                register[iRegister] = ip;
                Instructions[ins].Invoke(register, a, b, c);
                ip = register[iRegister];

                ip++;
            }

            return -1;
        }

        /// <summary>
        ///     Reverse engineered the input, but I have no clue what it actually does.
        /// </summary>
        private static int SolvePart1RevEng()
        {
            var r = new[] {0, 0, 0, 0, 0, 0};

            r[1] = r[5] | 65536;
            r[5] = 10678677;

            while (true)
            {
                r[5] = r[5] + (r[1] & 255);
                r[5] = r[5] & 16777215;
                r[5] = r[5] * 65899;
                r[5] = r[5] & 16777215;

                if (256 <= r[1])
                {
                    r[1] = r[1] / 256;
                }
                else
                {
                    return r[5];
                }
            }
        }

        private static int SolvePart2()
        {
            var r = new[] {0, 0, 0, 0, 0, 0};

            r[1] = r[5] | 65536;
            r[5] = 10678677;

            var seen = new HashSet<int>();
            var last = 0;
            while (true)
            {
                r[5] = r[5] + (r[1] & 255);
                r[5] = r[5] & 16777215;
                r[5] = r[5] * 65899;
                r[5] = r[5] & 16777215;

                if (256 <= r[1])
                {
                    r[1] = r[1] / 256;
                }
                else
                {
                    // find value before loop begins
                    if (!seen.Add(r[5]))
                    {
                        return last;
                    }

                    last = r[5];

                    r[1] = r[5] | 65536;
                    r[5] = 10678677;
                }
            }
        }
    }
}