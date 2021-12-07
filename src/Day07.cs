using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Console;

namespace Advent_of_Code_2021
{
    public static class Day07
    {
        public static void Run()
        {
            var lines = File.ReadLines("Day07.txt").ToList();
            var numbers = lines[0].Split(',').Select(int.Parse).ToList();

            var max = numbers.Max();
            var costs = new int[max + 1];
            var sum = 0;
            for (var i = 0; i < max; i++)
            {
                sum += i;
                costs[i] = sum;
            }

            var part1 = int.MaxValue;
            var part2 = int.MaxValue;
            for (var i = numbers.Min(); i < max; i++)
            {
                var cost = numbers.Sum(n => Math.Abs(n - i));
                if (cost < part1) part1 = (int) cost;
                
                var cost2 = numbers.Sum(n => costs[Math.Abs(n - i)]);
                if (cost2 < part2) part2 = (int) cost2;
            }
            WriteLine($"Part 1: {part1}");
            WriteLine($"Part 2: {part2}");
        }
    }
}