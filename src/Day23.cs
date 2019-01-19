using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
    public class Day23 : IDay
    {
        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)} (Expected: 761)");
        }

        public void Part2()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(input)} (Expected: 89915526)");
            // But is 89915524. ¯\_(ツ)_/¯
        }

        private static int SolvePart1(string input)
        {
            var linePattern = new Regex("pos=<(?<x>[-]?\\d+),(?<y>[-]?\\d+),(?<z>[-]?\\d+)>, r=(?<radius>\\d+)");

            var bots = new List<Bot>();
            var lines = input.Split("\n").Select(x => x.Trim());
            foreach (var line in lines)
            {
                var match = linePattern.Match(line);
                var x = int.Parse(match.Groups["x"].Value);
                var y = int.Parse(match.Groups["y"].Value);
                var z = int.Parse(match.Groups["z"].Value);
                var radius = int.Parse(match.Groups["radius"].Value);

                bots.Add(new Bot(x, y, z, radius));
            }

            var bestBot = bots.Aggregate((l, r) => l.Radius > r.Radius ? l : r);

            var botsInRange = 0;
            foreach (var bot in bots)
            {
                var distance = Math.Abs(bestBot.X - bot.X) + Math.Abs(bestBot.Y - bot.Y) + Math.Abs(bestBot.Z - bot.Z);
                if (distance <= bestBot.Radius) botsInRange++;
            }

            return botsInRange;
        }

        private static int SolvePart2(string input)
        {
            var linePattern = new Regex("pos=<(?<x>[-]?\\d+),(?<y>[-]?\\d+),(?<z>[-]?\\d+)>, r=(?<radius>\\d+)");

            var bots = new List<Bot>();
            var lines = input.Split("\n").Select(x => x.Trim());
            foreach (var line in lines)
            {
                var match = linePattern.Match(line);
                var x = int.Parse(match.Groups["x"].Value);
                var y = int.Parse(match.Groups["y"].Value);
                var z = int.Parse(match.Groups["z"].Value);
                var radius = int.Parse(match.Groups["radius"].Value);

                bots.Add(new Bot(x, y, z, radius));
            }

            var neighbours = new Dictionary<Bot, List<Bot>>();
            foreach (var bot1 in bots)
            {
                neighbours.Add(bot1, new List<Bot>());
                foreach (var bot2 in bots.Where(b => b != bot1))
                    if (bot1.Radius + bot2.Radius >= Math.Abs(bot1.X - bot2.X) + Math.Abs(bot1.Y - bot2.Y) + Math.Abs(bot1.Z - bot2.Z))
                        neighbours[bot1].Add(bot2);
            }

            var cliques = FindCliques(neighbours);

            var result = int.MaxValue;
            foreach (var clique in cliques)
            {
                var dist = clique.Max(b => Math.Abs(b.X) + Math.Abs(b.Y) + Math.Abs(b.Z) - b.Radius);
                if (dist < result) result = dist;
            }

            return result;
        }

        private static IEnumerable<List<Bot>> FindCliques(IReadOnlyDictionary<Bot, List<Bot>> neighbours)
        {
            var bots = neighbours.Keys.ToList();
            var cliques = new List<List<Bot>>();

            BronKerbosch2(neighbours, cliques, new List<Bot>(), bots, new List<Bot>());

            return cliques;
        }

        private static void BronKerbosch2(
            IReadOnlyDictionary<Bot, List<Bot>> graph,
            List<List<Bot>> cliques,
            IReadOnlyCollection<Bot> currentClique,
            IReadOnlyCollection<Bot> vertices,
            List<Bot> exclude)
        {
            if (vertices.Count == 0 && exclude.Count == 0)
            {
                if (cliques.Count == 0)
                {
                    cliques.Add(currentClique.ToList());
                    return;
                }

                // larger clique
                var currentLength = cliques[0].Count;
                if (currentClique.Count > currentLength) cliques.Clear();

                // clique of same size
                if (currentClique.Count >= currentLength) cliques.Add(currentClique.ToList());

                return;
            }

            var verticesCopy = vertices.ToList();
            var pivot = vertices.Union(exclude).Aggregate((l, r) => graph[l].Count > graph[r].Count ? l : r);

            foreach (var v in vertices.Except(graph[pivot]))
            {
                var currentClique1 = currentClique.Union(new[] { v }).ToList();
                var vertices1 = verticesCopy.Intersect(graph[v]).ToList();
                var exclude1 = exclude.Intersect(graph[v]).ToList();
                BronKerbosch2(graph, cliques, currentClique1, vertices1, exclude1);

                verticesCopy.Remove(v);
                exclude.Add(v);
            }
        }

        private class Bot
        {
            public Bot(int x, int y, int z, int radius)
            {
                X = x;
                Y = y;
                Z = z;
                Radius = radius;
            }

            public int X { get; }
            public int Y { get; }
            public int Z { get; }
            public int Radius { get; }
        }
    }
}