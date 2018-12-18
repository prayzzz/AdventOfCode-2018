using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018
{
    public class Day18 : IDay
    {
        public void Part1()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 1: {SolvePart1(input)} (Expected: 584714)");
        }

        public void Part2()
        {
            var input = Helper.ReadEmbeddedFile(GetType().Assembly, $"Input.{GetType().Name}.txt");
            Console.WriteLine($"{GetType().Name} Part 2: {SolvePart2(input)} (Expected: 161160)");
        }

        private static int SolvePart1(string input)
        {
            const int size = 50;
            var area = new char[size, size];

            var lines = input.Split("\n").Select(x => x.Trim()).ToList();
            for (var l = 0; l < size; l++)
            for (var c = 0; c < size; c++)
            {
                area[l, c] = lines[l][c];
            }

            var currentArea = area;
            for (var i = 0; i < 10; i++)
            {
                var newArea = new char[size, size];

                for (var x = 0; x < size; x++)
                for (var y = 0; y < size; y++)
                {
                    var trees = 0;
                    var lumberyars = 0;

                    CountSafe(x - 1, y - 1);
                    CountSafe(x - 1, y + 1);
                    CountSafe(x - 1, y);

                    CountSafe(x + 1, y - 1);
                    CountSafe(x + 1, y + 1);
                    CountSafe(x + 1, y);

                    CountSafe(x, y - 1);
                    CountSafe(x, y + 1);

                    if (currentArea[x, y] == '.' && trees > 2)
                    {
                        newArea[x, y] = '|';
                    }
                    else if (currentArea[x, y] == '|' && lumberyars > 2)
                    {
                        newArea[x, y] = '#';
                    }
                    else if (currentArea[x, y] == '#' && (lumberyars <= 0 || trees <= 0))
                    {
                        newArea[x, y] = '.';
                    }
                    else
                    {
                        newArea[x, y] = currentArea[x, y];
                    }

                    void CountSafe(int x2, int y2)
                    {
                        if (x2 > -1 && x2 < size && y2 > -1 && y2 < size)
                        {
                            if (currentArea[x2, y2] == '#')
                            {
                                lumberyars++;
                            }
                            else if (currentArea[x2, y2] == '|')
                            {
                                trees++;
                            }
                        }
                    }
                }

                currentArea = newArea;
            }

            var treeResult = 0;
            var lumberyarsResult = 0;

            for (var x = 0; x < size; x++)
            for (var y = 0; y < size; y++)
            {
                if (currentArea[x, y] == '#')
                {
                    lumberyarsResult++;
                }
                else if (currentArea[x, y] == '|')
                {
                    treeResult++;
                }
            }

            return treeResult * lumberyarsResult;
        }

        private static int SolvePart2(string input)
        {
            const int size = 50;
            var area = new char[size, size];

            var lines = input.Split("\n").Select(x => x.Trim()).ToList();
            for (var l = 0; l < size; l++)
            for (var c = 0; c < size; c++)
            {
                area[l, c] = lines[l][c];
            }

            var results = new List<string>();
            var currentArea = area;
            for (var i = 0; i < 1_000_000_000; i++)
            {
                var newArea = new char[size, size];

                for (var x = 0; x < size; x++)
                for (var y = 0; y < size; y++)
                {
                    var trees = 0;
                    var lumberyars = 0;

                    CountSafe(x - 1, y - 1);
                    CountSafe(x - 1, y + 1);
                    CountSafe(x - 1, y);

                    CountSafe(x + 1, y - 1);
                    CountSafe(x + 1, y + 1);
                    CountSafe(x + 1, y);

                    CountSafe(x, y - 1);
                    CountSafe(x, y + 1);

                    if (currentArea[x, y] == '.' && trees > 2)
                    {
                        newArea[x, y] = '|';
                    }
                    else if (currentArea[x, y] == '|' && lumberyars > 2)
                    {
                        newArea[x, y] = '#';
                    }
                    else if (currentArea[x, y] == '#' && (lumberyars <= 0 || trees <= 0))
                    {
                        newArea[x, y] = '.';
                    }
                    else
                    {
                        newArea[x, y] = currentArea[x, y];
                    }

                    void CountSafe(int x2, int y2)
                    {
                        if (x2 > -1 && x2 < size && y2 > -1 && y2 < size)
                        {
                            if (currentArea[x2, y2] == '#')
                            {
                                lumberyars++;
                            }
                            else if (currentArea[x2, y2] == '|')
                            {
                                trees++;
                            }
                        }
                    }
                }

                currentArea = newArea;

                var areaStr = currentArea.Join();
                if (!results.Contains(areaStr))
                {
                    results.Add(areaStr);
                }
                else
                {
                    var indexOf = results.IndexOf(areaStr);
                    var loopLength = i - indexOf;
                    var resultAtLoopIndex = (1_000_000_000 - (i + 1)) % loopLength;

                    var resultArea = results[indexOf + resultAtLoopIndex];

                    var treeResult = resultArea.Count(c => c == '|');
                    var lumberyarsResult = resultArea.Count(c => c == '#');

                    return treeResult * lumberyarsResult;
                }
            }

            return -1;
        }
    }
}