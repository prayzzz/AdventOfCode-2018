using System;
using System.Linq;

namespace AdventOfCode2018
{
    public class Day02 : IDay
    {
        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)} (Expected: 5928)");
        }

        public void Part2()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(input)} (Expected: bqlporuexkwzyabnmgjqctvfs)");
        }

        private static int SolvePart1(string input)
        {
            var lines = input.Trim().Split("\n");

            var two = 0;
            var three = 0;
            foreach (var line in lines)
            {
                var twoFound = false;
                var threeFound = false;

                foreach (var current in line)
                {
                    var count = line.Count(c => c == current);

                    if (count == 2 && !twoFound)
                    {
                        two++;
                        twoFound = true;
                    }

                    if (count == 3 && !threeFound)
                    {
                        three++;
                        threeFound = true;
                    }
                }
            }

            return two * three;
        }

        private static string SolvePart2(string input)
        {
            var lines = input.Trim().Split("\n");

            foreach (var id1 in lines.Select(l => l.Trim()))
            foreach (var id2 in lines.Select(l => l.Trim()))
            {
                var missed = 0;
                var indexOfDifferentChar = -1;
                for (var i = 0; i < id1.Length; i++)
                {
                    if (id1[i] != id2[i])
                    {
                        missed++;
                        indexOfDifferentChar = i;
                    }
                }

                if (missed == 1)
                {
                    return id1.Remove(indexOfDifferentChar, 1);
                }
            }

            return "";
        }
    }
}