using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2018
{
    [TestClass]
    public class Day05Part1
    {
        [TestMethod]
        public void AlchemicalReduction()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day05.txt");
            Console.WriteLine(GetType().Name + ": " + Solve(input));
        }

        private static int Solve(string input)
        {
            while (true)
            {
                var reduction = false;
                for (var i = 0; i < input.Length; i++)
                {
                    if (i + 1 >= input.Length)
                    {
                        break;
                    }

                    var c = input[i];
                    var c2 = input[i + 1];

                    if (char.ToUpper(c) == char.ToUpper(c2) && c != c2)
                    {
                        reduction = true;
                        input = input.Remove(i, 2);
                    }
                }

                if (!reduction)
                {
                    break;
                }
            }

            return input.Length;
        }
    }

    [TestClass]
    public class Day05Part2
    {
        [TestMethod]
        public void AlchemicalReduction()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day05.txt");
            Console.WriteLine(GetType().Name + ": " + Solve(input));
        }

        private static int Solve(string input)
        {
            var minLength = input.Length;

            for (int charI = 'a'; charI <= 'z'; charI++)
            {
                var lower = ((char) charI).ToString().ToLower();
                var upper = ((char) charI).ToString().ToUpper();

                var preparedInput = input.Replace(lower, "").Replace(upper, "");

                while (true)
                {
                    var isReduced = false;
                    for (var i = 0; i < preparedInput.Length; i++)
                    {
                        if (i + 1 >= preparedInput.Length)
                        {
                            break;
                        }

                        var c = preparedInput[i];
                        var c2 = preparedInput[i + 1];

                        if (char.ToUpper(c) == char.ToUpper(c2) && c != c2)
                        {
                            isReduced = true;
                            preparedInput = preparedInput.Remove(i, 2);
                        }
                    }

                    if (!isReduced)
                    {
                        break;
                    }
                }

                if (preparedInput.Length < minLength)
                {
                    minLength = preparedInput.Length;
                }
            }

            return minLength;
        }
    }
}