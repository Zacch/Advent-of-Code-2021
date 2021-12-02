using System.IO;
using System.Linq;
using static System.Console;

namespace Advent_of_Code_2021
{
    public static class Day02
    {
        public static void Run()
        {
            var lines = File.ReadLines("Day02.txt").ToList();
            var directions = lines.Select(s =>
            {
                var parts = s.Split(" ");
                return (parts[0], int.Parse(parts[1]));
            }).ToList();

            var position = 0;
            var depth = 0;
            foreach (var (command, argument) in directions)
            {
                switch (command)
                {
                    case "forward": 
                        position += argument;
                        break;
                    case "up": 
                        depth -= argument;
                        break;
                    case "down": 
                        depth += argument;
                        break;
                }
            }
            WriteLine($"Part 1: {position * depth}");

            position = 0;
            depth = 0;
            var aim = 0;
            foreach (var (command, argument) in directions)
            {
                switch (command)
                {
                    case "forward": 
                        position += argument;
                        depth += argument * aim;
                        break;
                    case "up": 
                        aim -= argument;
                        break;
                    case "down": 
                        aim += argument;
                        break;
                }
            }
            WriteLine($"Part 2: {position * depth}");

        }
    }
}