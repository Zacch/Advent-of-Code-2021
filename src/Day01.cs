using System.IO;
using System.Linq;
using static System.Console;

namespace Advent_of_Code_2021
{
    public static class Day01
    {
        public static void Run()
        {
            var lines = File.ReadLines("Day01.txt").ToList();
            var depths = lines.Select(int.Parse).ToList();
            
            var lastDepth = int.MaxValue;
            var part1 = 0;
            foreach (var depth in depths)
            {
                if (depth > lastDepth) part1++;
                lastDepth = depth;
            }
            WriteLine($"Part 1: {part1}");

            var lastsum = int.MaxValue;
            var part2 = 0;
            for (var i = 0; i < depths.Count - 2; i++)
            {
                var sum = depths[i] + depths[i + 1] + depths[i + 2];
                if (sum > lastsum) part2++;
                lastsum = sum;
            }
            WriteLine($"Part 2: {part2}");
        }
    }
}