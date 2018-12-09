using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2018
{
    [TestClass]
    public class Day09Part1
    {
        [TestMethod]
        public void MarbleMania()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day09.txt");
            Console.WriteLine(GetType().Name + ": " + Solve(input));
        }

        private static int Solve(string input)
        {
            var line = Regex.Match(input, "(\\d+) players; last marble is worth (\\d+) points");

            var players = int.Parse(line.Groups[1].Value);
            var lastMarble = int.Parse(line.Groups[2].Value);

            var scores = new Dictionary<int, int>();
            var board = new List<int>(lastMarble) {0};

            var currentPlayer = 0;
            var currentIndex = 0;

            for (var i = 1; i <= lastMarble; i++)
            {
                currentPlayer = currentPlayer % players + 1;

                if (i % 23 == 0)
                {
                    scores.TryAdd(currentPlayer, 0);
                    scores[currentPlayer] += i;

                    currentIndex = currentIndex - 7;
                    if (currentIndex < 0)
                    {
                        currentIndex = board.Count + currentIndex;
                    }
                    
                    scores[currentPlayer] += board[currentIndex];
                    board.RemoveAt(currentIndex);
                }
                else
                {
                    var insertIndex = (currentIndex + 1) % board.Count + 1;
                    board.Insert(insertIndex, i);
                    currentIndex = insertIndex;
                }
            }

            return scores.Aggregate((l, r) => l.Value > r.Value ? l : r).Value;
        }
    }

    [TestClass]
    public class Day09Part2
    {
        [TestMethod]
        public void MarbleMania()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day09.txt");
            Console.WriteLine(GetType().Name + ": " + Solve(input));
        }

        private static int Solve(string input)
        {
            return -1;
        }
    }
}