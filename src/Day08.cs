using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2018
{
    [TestClass]
    public class Day08Part1
    {
        [TestMethod]
        public void MemoryManeuver()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day08.txt");
            Console.WriteLine(GetType().Name + ": " + Solve(input));
        }

        private static int Solve(string input)
        {
            var memory = input.Split(" ").Select(int.Parse).ToList();

            var result = 0;
            ProcessChild(memory, 0, ref result);
            
            return result;
        }

        private static int ProcessChild(IReadOnlyList<int> memory, int index, ref int result)
        {
            var childCount = memory[index];

            index++;
            
            var metaDataCount = memory[index];

            for (var c = 0; c < childCount; c++)
            {
                index++;
                index = ProcessChild(memory, index, ref result);
            }

            var upperBound = index + metaDataCount;
            while (index < upperBound)
            {
                index++;
                result += memory[index];
            }

            return index;
        }
    }

    [TestClass]
    public class Day08Part2
    {
        [TestMethod]
        public void MemoryManeuver()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day08.txt");
            Console.WriteLine(GetType().Name + ": " + Solve(input));
        }

        private static int Solve(string input)
        {
            var memory = input.Split(" ").Select(int.Parse).ToList();

            var result = 0;
            ProcessChild(memory, 0, ref result);
            
            return result;
        }

        private static int ProcessChild(IReadOnlyList<int> memory, int index, ref int result)
        {
            var childCount = memory[index];

            index++;
            
            var metaDataCount = memory[index];

            var childSums = new List<int>();
            for (var c = 0; c < childCount; c++)
            {
                var sum = 0;
                index++;
                index = ProcessChild(memory, index, ref sum);
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