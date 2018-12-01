using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2018
{
    [TestClass]
    public class Day01Part1
    {
        [TestMethod]
        public void ChronalCalibration()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day01.txt");
            Console.WriteLine("Day01Part1: " + Solve(input));
        }

        private int Solve(string input)
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
    }

    [TestClass]
    public class Day01Part2
    {
        [TestMethod]
        public void ChronalCalibration()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day01.txt");
            Console.WriteLine("Day01Part2: " + Solve(input));
        }

        private static int Solve(string input)
        {
            const char lineEnd = '\n';

            var inputSpan = input.AsSpan();
            
            // Position of the first char of a line
            var lineStart = 0;

            var frequencies = new HashSet<int>();
            var frequency = 0;
            
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
                        // Frequency already in list => Result found
                        return frequency;
                    }
                }

                if (j == inputSpan.Length - 1)
                {
                    // Duplicate not found.

                    // Reset cursor
                    lineStart = 0;
                    
                    // Loop list again
                    j = -1;
                }
            }

            return int.MaxValue;
        }
    }
}