using System;

namespace AdventOfCode2018
{
    public class Day05 : IDay
    {
        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)} (Expected: 10250)");
        }

        public void Part2()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(input)} (Expected: 6188)");
        }

        private static int SolvePart1(string input)
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

        private static int SolvePart2(string input)
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