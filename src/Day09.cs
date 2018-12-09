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

        private static long Solve(string input)
        {
            var line = Regex.Match(input, "(\\d+) players; last marble is worth (\\d+) points");

            var players = int.Parse(line.Groups[1].Value);
            var lastMarble = int.Parse(line.Groups[2].Value);

            var scores = new Dictionary<int, long>();
            var board = new LinkedList<long>();
            var current = board.AddFirst(0);

            var currentPlayer = 0;
            
            for (var i = 1; i <= lastMarble; i++)
            {
                currentPlayer = currentPlayer % players + 1;

                if (i % 23 == 0)
                {
                    scores.TryAdd(currentPlayer, 0);
                    scores[currentPlayer] += i;
                    
                    for (var j = 0; j < 7; j++)
                    {
                        current = current.Previous ?? board.Last;
                    }

                    scores[currentPlayer] += current.Value;
                    
                    var remove = current;
                    current = remove.Next;
                    board.Remove(remove);
                }
                else
                {
                    current = board.AddAfter(current.Next ?? board.First, i);
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

        private static long Solve(string input)
        {
            var line = Regex.Match(input, "(\\d+) players; last marble is worth (\\d+) points");

            var players = int.Parse(line.Groups[1].Value);
            var lastMarble = int.Parse(line.Groups[2].Value) * 100;

            var scores = new Dictionary<int, long>();
            var board = new LinkedList<long>();
            var current = board.AddFirst(0);

            var currentPlayer = 0;
            
            for (var i = 1; i <= lastMarble; i++)
            {
                currentPlayer = currentPlayer % players + 1;

                if (i % 23 == 0)
                {
                    scores.TryAdd(currentPlayer, 0);
                    scores[currentPlayer] += i;
                    
                    for (var j = 0; j < 7; j++)
                    {
                        current = current.Previous ?? board.Last;
                    }

                    scores[currentPlayer] += current.Value;
                    
                    var remove = current;
                    current = remove.Next;
                    board.Remove(remove);
                }
                else
                {
                    current = board.AddAfter(current.Next ?? board.First, i);
                }
            }

            return scores.Aggregate((l, r) => l.Value > r.Value ? l : r).Value;
        }
    }
}