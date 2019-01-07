using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    public class Day08 : IDay
    {
        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)} (Expected: 37439)");
        }

        public void Part2()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(input)} (Expected: 20815)");
        }

        private static int SolvePart1(string input)
        {
            var memory = input.Split(" ").Select(int.Parse).ToList();

            var result = 0;
            ProcessChild1(memory, 0, ref result);

            return result;
        }

        private static int ProcessChild1(IReadOnlyList<int> memory, int index, ref int result)
        {
            var childCount = memory[index];

            index++;

            var metaDataCount = memory[index];

            for (var c = 0; c < childCount; c++)
            {
                index++;
                index = ProcessChild1(memory, index, ref result);
            }

            var upperBound = index + metaDataCount;
            while (index < upperBound)
            {
                index++;
                result += memory[index];
            }

            return index;
        }

        private static int SolvePart2(string input)
        {
            var memory = input.Split(" ").Select(int.Parse).ToList();

            var result = 0;
            ProcessChild2(memory, 0, ref result);

            return result;
        }

        private static int ProcessChild2(IReadOnlyList<int> memory, int index, ref int result)
        {
            var childCount = memory[index];

            index++;

            var metaDataCount = memory[index];

            var childSums = new List<int>();
            for (var c = 0; c < childCount; c++)
            {
                var sum = 0;
                index++;
                index = ProcessChild2(memory, index, ref sum);
                childSums.Add(sum);
            }

            if (childSums.Count == 0)
            {
                var upperBound = index + metaDataCount;
                while (index < upperBound)
                {
                    index++;
                    result += memory[index];
                }
            }
            else
            {
                var upperBound = index + metaDataCount;
                while (index < upperBound)
                {
                    index++;

                    var childIndex = memory[index] - 1;
                    if (childIndex < childSums.Count)
                    {
                        result += childSums[childIndex];
                    }
                }
            }

            return index;
        }
    }
}