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
                        input = input.Remove(i, 2);
                        i = Math.Max(i - 2, -1);
                    }
                }

                return input.Length;
            }
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
                        preparedInput = preparedInput.Remove(i, 2);
                        i = Math.Max(i - 2, -1);
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