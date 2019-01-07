using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode2018
{
    public class Day04 : IDay
    {
        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)} (Expected: 85296)");
        }

        public void Part2()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(input)} (Expected: 58559)");
        }

        private static int SolvePart1(string input)
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

        private static int SolvePart2(string input)
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