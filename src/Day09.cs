using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
    public class Day09 : IDay
    {
        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)} (Expected: 386151)");
        }

        public void Part2()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(input)} (Expected: 3211264152)");
        }

        private static long SolvePart1(string input)
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

        private static long SolvePart2(string input)
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