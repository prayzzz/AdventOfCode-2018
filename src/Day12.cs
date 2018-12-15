using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2018
{
    [TestClass]
    public class Day12Part1
    {
        [TestMethod]
        public void SubterraneanSustainability()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day12.txt");
            Console.WriteLine(GetType().Name + ": " + Solve(input));
        }

        private static int Solve(string input)
        {
            const int padding = 5;
            var lines = input.Split("\n");

            var pods = new List<bool>();

            foreach (var c in lines[0].Split(":")[1].Trim())
            {
                var isPlant = c == '#';
                pods.Add(isPlant);
            }

            // add padding
            for (var i = 0; i < padding; i++)
            {
                pods.Insert(0, false);
                pods.Add(false);
            }

            var rules = new Dictionary<bool[], bool>();
            for (var i = 2; i < lines.Length; i++)
            {
                var rule = lines[i].Split("=>");

                var pattern = new bool[5];
                for (var index = 0; index < rule[0].Trim().Length; index++)
                {
                    pattern[index] = rule[0][index] == '#';
                }

                rules.Add(pattern, rule[1].Trim()[0] == '#');
            }

            WritePods(pods);

            for (var i = 0; i < 20; i++)
            {
                var newPods = new List<bool>();

                for (var currentPod = 0; currentPod < pods.Count; currentPod++)
                {
                    foreach (var (pattern, value) in rules)
                    {
                        var isMatch = true;
                        for (var checkPod = -2; checkPod < 3; checkPod++)
                        {
                            var patternI = checkPod + 2;

                            // outside of range
                            if (currentPod + checkPod < 0 || currentPod + checkPod >= pods.Count)
                            {
                                if (pattern[patternI])
                                {
                                    isMatch = false;
                                    break;
                                }

                                continue;
                            }

                            if (pods[currentPod + checkPod] != pattern[patternI])
                            {
                                isMatch = false;
                                break;
                            }
                        }

                        if (isMatch)
                        {
                            newPods.Add(value);
                        }
                    }
                }

                var neededPadding = 0;
                for (var ip = 1; ip <= 5; ip++)
                {
                    if (newPods[newPods.Count - ip])
                    {
                        neededPadding = 6 - ip;
                        break;
                    }
                }

                for (var j = 0; j < neededPadding; j++) newPods.Add(false);

                pods = newPods;
                WritePods(pods);
            }

            return pods.Select((b, i) => b ? i - padding : 0).Sum();
        }

        private static void WritePods(IEnumerable<bool> pods)
        {
            foreach (var pod in pods)
            {
                Console.Write(pod ? '#' : '.');
            }

            Console.WriteLine();
        }
    }

    [TestClass]
    public class Day12Part2
    {
        [TestMethod]
        public void SubterraneanSustainability()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day12.txt");
            Console.WriteLine(GetType().Name + ": " + Solve(input));
        }

        private static long Solve(string input)
        {
            const int padding = 5;
            var lines = input.Split("\n");

            var pods = new List<bool>();

            foreach (var c in lines[0].Split(":")[1].Trim())
            {
                var isPlant = c == '#';
                pods.Add(isPlant);
            }

            // add padding
            for (var i = 0; i < padding; i++)
            {
                pods.Insert(0, false);
                pods.Add(false);
            }

            var rules = new Dictionary<bool[], bool>();
            for (var i = 2; i < lines.Length; i++)
            {
                var rule = lines[i].Split("=>");

                var pattern = new bool[5];
                for (var index = 0; index < rule[0].Trim().Length; index++)
                {
                    pattern[index] = rule[0][index] == '#';
                }

                rules.Add(pattern, rule[1].Trim()[0] == '#');
            }

            var podPatterns = new List<List<bool>>();

            for (var generation = 0; generation < 2000; generation++)
            {
                var newPods = new List<bool>();

                for (var currentPod = 0; currentPod < pods.Count; currentPod++)
                {
                    foreach (var (pattern, value) in rules)
                    {
                        var isMatch = true;
                        for (var patternI = 0; patternI < 5; patternI++)
                        {
                            var podToCheck = patternI - 2;
                            
                            // outside of range
                            if (currentPod + podToCheck < 0 || currentPod + podToCheck >= pods.Count)
                            {
                                if (pattern[patternI])
                                {
                                    isMatch = false;
                                    break;
                                }

                                continue;
                            }

                            if (pods[currentPod + podToCheck] != pattern[patternI])
                            {
                                isMatch = false;
                                break;
                            }
                        }

                        if (isMatch)
                        {
                            newPods.Add(value);
                        }
                    }
                }

                var neededPadding = 0;
                for (var ip = 1; ip <= 5; ip++)
                {
                    if (newPods[newPods.Count - ip])
                    {
                        neededPadding = 6 - ip;
                        break;
                    }
                }

                // add additional padding for next generation
                for (var j = 0; j < neededPadding; j++) newPods.Add(false);

                // extract relevant pods and try find already existing pattern
                var start = newPods.IndexOf(true);
                var end = newPods.Count - (newPods.Count - newPods.LastIndexOf(true)) - start + 1;
                var podPattern = newPods.GetRange(start, end);

                foreach (var pattern in podPatterns.Where(p => p.Count == podPattern.Count))
                {
                    var patternFound = pattern.Select((value, index) => value == podPattern[index]).All(x => x);
                    if (patternFound)
                    {
                        // pattern will move 1 index per generation
                        // calculate how much it moved after last generation
                        var offset = 50_000_000_000 - generation;
                        return pods.Select((b, i) => b ? i - padding + offset : 0).Sum();
                    }
                }

                podPatterns.Add(podPattern);
                pods = newPods;
            }

            return -1;
        }
    }
}