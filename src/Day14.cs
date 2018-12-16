using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode2018
{
    public class Day14 : IDay
    {
        public void Part1()
        {
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(157901)} (Expected: 9411137133)");
        }

        public void Part2()
        {
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(157901)} (Expected: 20317612)");
        }

        private static string SolvePart1(int input)
        {
            var scores = new LinkedList<int>();

            scores.AddFirst(3);
            scores.AddLast(7);

            var elf1 = scores.First;
            var elf2 = scores.Last;

            for (var i = 0; i < input + 10; i++)
            {
                var sum = elf1.Value + elf2.Value;

                var digit1 = sum / 10 % 10;
                if (digit1 != 0)
                {
                    scores.AddLast(digit1);
                }

                scores.AddLast(sum % 10);

                var moveElf1 = 1 + elf1.Value;
                var moveElf2 = 1 + elf2.Value;

                for (var j = 0; j < moveElf1; j++)
                {
                    elf1 = elf1.Next ?? scores.First;
                }

                for (var j = 0; j < moveElf2; j++)
                {
                    elf2 = elf2.Next ?? scores.First;
                }
            }

            var current = scores.First;
            for (var i = 0; i < input; i++)
            {
                current = current.Next;
            }

            var sb = new StringBuilder();
            for (var i = 0; i < 10; i++)
            {
                sb.Append(current.Value);
                current = current.Next ?? scores.First;
            }

            return sb.ToString();
        }

        private static int SolvePart2(int input)
        {
            var inputSplit = input.ToString().Select(x => int.Parse(x.ToString())).ToList();

            var scores = new LinkedList<int>();
            scores.AddFirst(3);
            scores.AddLast(7);

            var elf1 = scores.First;
            var elf2 = scores.Last;

            while (true)
            {
                var sum = elf1.Value + elf2.Value;

                var digit1 = sum / 10 % 10;
                if (digit1 != 0)
                {
                    scores.AddLast(digit1);
                    if (CheckForInput(scores, inputSplit))
                    {
                        return scores.Count - inputSplit.Count;
                    }
                }

                scores.AddLast(sum % 10);
                if (CheckForInput(scores, inputSplit))
                {
                    return scores.Count - inputSplit.Count;
                }

                var moveElf1 = 1 + elf1.Value;
                var moveElf2 = 1 + elf2.Value;

                for (var j = 0; j < moveElf1; j++)
                {
                    elf1 = elf1.Next ?? scores.First;
                }

                for (var j = 0; j < moveElf2; j++)
                {
                    elf2 = elf2.Next ?? scores.First;
                }
            }
        }

        private static bool CheckForInput(LinkedList<int> scores, IReadOnlyList<int> inputSplit)
        {
            var check = scores.Last;
            var isMatch = true;
            for (var i = 0; i < inputSplit.Count; i++)
            {
                if (check.Value != inputSplit[inputSplit.Count - 1 - i])
                {
                    isMatch = false;
                    break;
                }

                check = check.Previous ?? scores.Last;
            }

            return isMatch;
        }
    }
}