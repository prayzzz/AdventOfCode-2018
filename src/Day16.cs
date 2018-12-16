using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
    public class Day16 : IDay
    {
        private static readonly Dictionary<string, Action<int[], int, int, int>> Funcs = new Dictionary<string, Action<int[], int, int, int>>
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

        private static readonly Dictionary<int, string> OpcodeMapping = new Dictionary<int, string>
        {
            {0, "setr"},
            {1, "eqrr"},
            {2, "gtri"},
            {3, "muli"},
            {4, "eqir"},
            {5, "borr"},
            {6, "bori"},
            {7, "mulr"},
            {8, "gtrr"},
            {9, "seti"},
            {10, "banr"},
            {11, "eqri"},
            {12, "addr"},
            {13, "gtir"},
            {14, "addi"},
            {15, "bani"}
        };

        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.1.txt");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)} (Expected: 509)");
        }

        public void Part2()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.2.txt");
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(input)} (Expected: 496)");
        }

        private static int SolvePart1(string input)
        {
            var lines = input.Split("\n").Select(l => l.Trim()).ToList();

            var mapping = new Dictionary<int, HashSet<string>>();
            var result = 0;
            for (var l = 0; l < lines.Count; l += 4)
            {
                var ins = Regex.Matches(lines[l + 1], "\\d+").Select(x => int.Parse(x.Value)).ToArray();
                var after = Regex.Matches(lines[l + 2], "\\d+").Select(x => int.Parse(x.Value)).ToArray();

                var validOpCodes = 0;
                foreach (var (opcode, action) in Funcs)
                {
                    var before = Regex.Matches(lines[l], "\\d+").Select(x => int.Parse(x.Value)).ToArray();

                    try
                    {
                        action.Invoke(before, ins[1], ins[2], ins[3]);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                    
                    if (before.Select((v, i) => v == after[i]).All(x => x))
                    {
                        validOpCodes++;
                        mapping.TryAdd(ins[0], new HashSet<string>());
                        mapping[ins[0]].Add(opcode);
                    }
                }

                if (validOpCodes > 2)
                {
                    result++;
                }
            }

            FindMapping(mapping);

            return result;
        }

        private static void FindMapping(IReadOnlyDictionary<int, HashSet<string>> mapping)
        {
            var result = new Dictionary<string, int>();
            if (Find(0, mapping, result))
            {
                foreach (var (key, value) in result)
                {
                    Console.Write($"{{ {value}, \"{key}\" }}, ");
                }
                
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Use Backtracking to find mapping
        /// </summary>
        private static bool Find(int i, IReadOnlyDictionary<int, HashSet<string>> mapping, IDictionary<string, int> result)
        {
            if (i == 16)
            {
                return true;
            }

            foreach (var code in mapping[i])
            {
                if (result.ContainsKey(code))
                {
                    continue;
                }

                result.Add(code, i);

                if (Find(i + 1, mapping, result))
                {
                    return true;
                }

                // Revert previous decision
                result.Remove(code);
            }

            return false;
        }

        private static int SolvePart2(string input)
        {
            var lines = input.Split("\n").Select(l => l.Trim()).ToList();

            var register = new[] {0, 0, 0, 0};
            foreach (var line in lines)
            {
                var ins = Regex.Matches(line, "\\d+").Select(x => int.Parse(x.Value)).ToArray();
                Funcs[OpcodeMapping[ins[0]]].Invoke(register, ins[1], ins[2], ins[3]);
            }

            return register[0];
        }
    }
}