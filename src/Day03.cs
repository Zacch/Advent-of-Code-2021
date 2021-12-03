using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Console;

namespace Advent_of_Code_2021
{
    public static class Day03
    {
        public static void Run()
        {
            var lines = File.ReadLines("Day03.txt").ToList();

            Part1(lines);
            Part2(lines);
        }
        
        private static void Part1(List<string> lines)
        {
            var gammaBits = "";

            for (var i = 0; i < lines[0].Length; i++)
            {
                var ones = lines.Count(s => s[i] == '1');
                gammaBits += ones >= lines.Count / 2 ? "1" : "0";
            }

            var epsilonBits = gammaBits
                .Replace('0', 'x')
                .Replace('1', '0')
                .Replace('x', '1');

            var gamma = Convert.ToInt32(gammaBits, 2);
            var epsilon = Convert.ToInt32(epsilonBits, 2);

            WriteLine($"Part 1: {gamma * epsilon}");
        }
        
        private static void Part2(List<string> lines)
        {
            var oxygenLines = new List<string>(lines);
            for (var i = 0; i < lines[0].Length; i++)
            {
                var ones = oxygenLines.Count(s => s[i] == '1');
                var mostCommonBit = ones >= oxygenLines.Count / 2d ? '1' : '0';
                oxygenLines = oxygenLines.Where(s => s[i] == mostCommonBit).ToList();

                if (oxygenLines.Count == 1) break;
            }
            var oxygenGeneratorRating = Convert.ToInt32(oxygenLines[0], 2);

            var co2Lines = new List<string>(lines);
            for (var i = 0; i < lines[0].Length; i++)
            {
                var ones = co2Lines.Count(s => s[i] == '1');
                var leastCommonBit = ones < co2Lines.Count / 2d ? '1' : '0';
                co2Lines = co2Lines.Where(s => s[i] == leastCommonBit).ToList();

                if (co2Lines.Count == 1) break;
            }
            var co2ScrubberRating = Convert.ToInt32(co2Lines[0], 2);

            WriteLine($"Part 2: {oxygenGeneratorRating * co2ScrubberRating}");
        }
    }
}