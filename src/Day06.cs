using System;
using System.IO;
using System.Linq;
using static System.Console;

namespace Advent_of_Code_2021
{
    public static class Day06
    {
        public static void Run()
        {
            var lines = File.ReadLines("Day06.txt").ToList();
            var numbers = lines[0].Split(',').Select(int.Parse).ToList();
            
            var fish = new Int64[9];
            foreach (var number in numbers) fish[number]++;

            for (var day = 1; day <= 256; day++)
            {
                var newFish = new Int64[9];
                Array.Copy(fish, 1, newFish, 0, 8);
                newFish[6] += fish[0];
                newFish[8] += fish[0];
                fish = newFish;
                if (day == 80) WriteLine($"Part 1: {fish.ToList().Sum(n => n)}");
            }
            WriteLine($"Part 2: {fish.ToList().Sum(n => n)}");
        }
    }
}