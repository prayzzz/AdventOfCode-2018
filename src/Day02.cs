using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdventOfCode2018
{
    [TestClass]
    public class Day02Part1
    {
        [TestMethod]
        public void InventoryManagementSystem()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day02.txt");
            Console.WriteLine("Day02Part1: " + Solve(input));
        }

        private static int Solve(string input)
        {
            var lines = input.Trim().Split("\n");

            var two = 0;
            var three = 0;
            foreach (var line in lines)
            {
                var twoFound = false;
                var threeFound = false;
                
                foreach (var current in line)
                {
                    var count = line.Count(c => c == current);

                    if (count == 2 && !twoFound)
                    {
                        two++;
                        twoFound = true;
                    }

                    if (count == 3 && !threeFound)
                    {
                        three++;
                        threeFound = true;
                    }
                }
            }
            
            return two * three;
        }
    }

    [TestClass]
    public class Day02Part2
    {
        [TestMethod]
        public void InventoryManagementSystem()
        {
            var input = TestHelper.ReadEmbeddedFile(GetType().Assembly, "Input.Day02.txt");
            Console.WriteLine("Day02Part2: " + Solve(input));
        }

        private static string Solve(string input)
        {
            var lines = input.Trim().Split("\n");

            foreach (var id1 in lines.Select(l => l.Trim()))
            {
                foreach (var id2 in lines.Select(l => l.Trim()))
                {
                    var missed = 0;
                    var indexOfDifferentChar = -1;
                    for (var i = 0; i < id1.Length; i++)
                    {
                        if (id1[i] != id2[i])
                        {
                            missed++;
                            indexOfDifferentChar = i;
                        }
                    }

                    if (missed == 1)
                    {
                        return id1.Remove(indexOfDifferentChar, 1);
                    }
                }
            }
            
            return "";
        }
    }
}