using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2018
{
    [TestClass]
    public class Day07Part1
    {
        [TestMethod]
        public void TheSumOfItsParts()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day07.txt");
            Console.WriteLine(GetType().Name + ": " + Solve(input));
        }

        private static string Solve(string input)
        {
            var linePattern = new Regex("Step (\\w) must be finished before step (\\w) can begin");

            var dependsOn = new Dictionary<char, HashSet<char>>();
            foreach (var line in input.Split("\n").Select(x => x.Trim()))
            {
                var match = linePattern.Match(line);

                var dependsOnStep = match.Groups[1].Value[0];
                var enabledByStep = match.Groups[2].Value[0];

                dependsOn.TryAdd(dependsOnStep, new HashSet<char>());
                dependsOn.TryAdd(enabledByStep, new HashSet<char>());
                dependsOn[enabledByStep].Add(dependsOnStep);
            }

            var allSteps = dependsOn.Keys.Distinct().OrderBy(s => s).ToList();
            var result = new List<char>();

            while (true)
            {
                if (allSteps.Count == 0)
                {
                    break;
                }

                for (var i = 0; i < allSteps.Count; i++)
                {
                    var step = allSteps[i];
                    var dependsOnSteps = dependsOn[step];

                    if (dependsOnSteps.All(s => result.Contains(s)))
                    {
                        allSteps.Remove(step);
                        result.Add(step);
                        break;
                    }
                }
            }

            return string.Join("", result);
        }
    }

    [TestClass]
    public class Day07Part2
    {
        [TestMethod]
        public void TheSumOfItsParts()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day07.txt");
            Console.WriteLine(GetType().Name + ": " + Solve(input));
        }

        private static int Solve(string input)
        {
            var linePattern = new Regex("Step (\\w) must be finished before step (\\w) can begin");

            var dependsOn = new Dictionary<char, HashSet<char>>();
            foreach (var line in input.Split("\n").Select(x => x.Trim()))
            {
                var match = linePattern.Match(line);

                var dependsOnStep = match.Groups[1].Value[0];
                var enabledByStep = match.Groups[2].Value[0];

                dependsOn.TryAdd(dependsOnStep, new HashSet<char>());
                dependsOn.TryAdd(enabledByStep, new HashSet<char>());
                dependsOn[enabledByStep].Add(dependsOnStep);
            }

            var allSteps = dependsOn.Keys.Distinct().OrderBy(s => s).ToList();
            var doneSteps = new List<char>();
            var workers = new Dictionary<char, int>(); // Step -> Time Left

            const int workerCount = 5;
            var time = -1;
            while (true)
            {
                time++;

                // work on step or remove finished step
                foreach (var stepInWork in workers.Keys.ToList())
                {
                    if (workers[stepInWork] == 0)
                    {
                        workers.Remove(stepInWork);
                        doneSteps.Add(stepInWork);
                    }
                    else
                    {
                        workers[stepInWork]--;
                    }
                }

                // assign new step to worker
                if (workers.Count != workerCount)
                {
                    for (var i = 0; i < allSteps.Count; i++)
                    {
                        var step = allSteps[i];
                        var dependsOnSteps = dependsOn[step];

                        if (dependsOnSteps.All(s => doneSteps.Contains(s)))
                        {
                            allSteps.Remove(step);
                            workers[step] = 60 + (step - 'A');
                        }
                    }
                }

                // if done
                if (allSteps.Count == 0 && workers.Count == 0)
                {
                    break;
                }
            }

            return time;
        }
    }
}