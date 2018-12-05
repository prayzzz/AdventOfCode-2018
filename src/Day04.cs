using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2018
{
    [TestClass]
    public class Day04Part1
    {
        [TestMethod]
        public void ReposeRecord()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day04.txt");
            Console.WriteLine(GetType().Name + ": " + Solve(input));
        }

        private static int Solve(string input)
        {
            var linePattern = new Regex("\\[(?<time>.*)\\] (?<notice>.*)");
            var guardNoticePattern = new Regex("Guard #(?<id>\\d+) begins shift");

            var lines = input.Trim().Split("\n");
            var sleepCount = new Dictionary<string, int[]>();

            var guardId = "";
            var sleepStartMinute = -1;
            foreach (var line in lines.OrderBy(x => x))
            {
                var match = linePattern.Match(line);
                var time = match.Groups["time"].Value.Trim();
                var notice = match.Groups["notice"].Value.Trim();
                
                if (notice.StartsWith("Guard"))
                {
                    guardId = guardNoticePattern.Match(notice).Groups["id"].Value;
                    sleepCount.TryAdd(guardId, new int[60]);
                    sleepStartMinute = -1;
                }

                if (notice.StartsWith("falls"))
                {
                    sleepStartMinute = int.Parse(time.Substring(14, 2));
                }

                if (notice.StartsWith("wakes"))
                {
                    var sleepEndMinute = int.Parse(time.Substring(14, 2));
                    for (var t = sleepStartMinute; t < sleepEndMinute; t++)
                    {
                        sleepCount[guardId][t]++;
                    }
                }
            }

            var guardIdWithMostSleep = sleepCount.Aggregate((l, r) => l.Value.Sum(x => x) > r.Value.Sum(x => x) ? l : r).Key;
            var minuteMostAsleep = sleepCount[guardIdWithMostSleep].Select((times, index) => (times, index))
                                                                   .OrderByDescending(vi => vi.times)
                                                                   .First();

            return int.Parse(guardIdWithMostSleep) * minuteMostAsleep.index;
        }
    }

    [TestClass]
    public class Day04Part2
    {
        [TestMethod]
        public void ReposeRecord()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day04.txt");
            Console.WriteLine(GetType().Name + ": " + Solve(input));
        }

        private static int Solve(string input)
        {
            var linePattern = new Regex("\\[(?<time>.*)\\] (?<notice>.*)");
            var guardNoticePattern = new Regex("Guard #(?<id>\\d+) begins shift");

            var lines = input.Trim().Split("\n");
            var sleepCount = new Dictionary<string, int[]>();

            var guardId = "";
            var sleepStartMinute = -1;
            foreach (var line in lines.OrderBy(x => x))
            {
                var match = linePattern.Match(line);
                var time = match.Groups["time"].Value.Trim();
                var notice = match.Groups["notice"].Value.Trim();
                
                if (notice.StartsWith("Guard"))
                {
                    guardId = guardNoticePattern.Match(notice).Groups["id"].Value;
                    sleepCount.TryAdd(guardId, new int[60]);
                    sleepStartMinute = -1;
                }

                if (notice.StartsWith("falls"))
                {
                    sleepStartMinute = int.Parse(time.Substring(14, 2));
                }

                if (notice.StartsWith("wakes"))
                {
                    var sleepEndMinute = int.Parse(time.Substring(14, 2));
                    for (var t = sleepStartMinute; t < sleepEndMinute; t++)
                    {
                        sleepCount[guardId][t]++;
                    }
                }
            }

            var guardIdWithMostSleep = "";
            var minuteMostAsleep = -1;
            var timesAsSleep = -1;
            foreach (var (id, sleepCountsPerMinute) in sleepCount)
            {
                var minuteSleeping = sleepCountsPerMinute.Select((times, minute) => (times, minute))
                                                         .OrderByDescending(x => x.times)
                                                         .First();

                if (minuteSleeping.times > timesAsSleep)
                {
                    timesAsSleep = minuteSleeping.times;
                    minuteMostAsleep = minuteSleeping.minute;
                    guardIdWithMostSleep = id;
                }
            }

            return int.Parse(guardIdWithMostSleep) * minuteMostAsleep;
        }
    }
}