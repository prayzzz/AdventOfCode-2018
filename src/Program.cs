using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AdventOfCode2018
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dayArg = args.Length >= 1 ? int.Parse(args[0]) : -1;
            var partArg = args.Length >= 2 ? int.Parse(args[1]) : -1;

            var iDay = typeof(IDay);
            var days = Assembly.GetAssembly(typeof(Program))
                               .GetTypes()
                               .Where(t => iDay.IsAssignableFrom(t)).ToList();

            if (dayArg > 0)
            {
                var dayClassName = $"Day{dayArg:D2}";
                days = days.Where(t => t.Name == dayClassName).ToList();

                if (!days.Any())
                {
                    var tmp = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"[ERROR] {dayClassName} not found");
                    Console.ForegroundColor = tmp;
                    return;
                }
            }

            foreach (var day in days)
            {
                if (Activator.CreateInstance(day) is IDay dayInstance)
                {
                    var sw = Stopwatch.StartNew();

                    if (partArg == 1)
                    {
                        dayInstance.Part1();
                    }
                    else if (partArg == 2)
                    {
                        dayInstance.Part2();
                    }
                    else
                    {
                        dayInstance.Part1();
                        dayInstance.Part2();
                    }

                    Console.WriteLine($"Finished after {sw.Elapsed}");
                }
            }
        }
    }

    public interface IDay
    {
        void Part1();

        void Part2();
    }
}