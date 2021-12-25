using System.Drawing;
using Extensions;
using static System.Console;

namespace Advent_of_Code_2021;

public class Day25
{
    
    private List<Point> east = new();
    private List<Point> south = new();

    public static void Run()
    {
        var lines = File.ReadLines("Day25.txt").ToList();

        new Day25().Solve(lines);
    }

    private void Solve(List<string> lines)
    {
        foreach (var (line, y) in lines.WithIndex())
            foreach (var (c, x) in line.ToCharArray().WithIndex())
                switch (c)
                {
                    case '>': east.Add(new Point(x, y)); break;
                    case 'v': south.Add(new Point(x, y)); break;
                }

        var width = lines[0].Length;
        var height = lines.Count;

        var allBlocked = false;
        var part1 = 0;
        while (!allBlocked)
        {
            var moving = east.Where(p => IsFree(new Point((p.X + 1) % width, p.Y))).ToList();
            allBlocked = moving.Count == 0;
            east = east.Select(p => moving.Contains(p) ? new Point((p.X + 1) % width, p.Y) : p).ToList();

            moving = south.Where(p => IsFree(new Point(p.X, (p.Y + 1) % height))).ToList();
            allBlocked &= moving.Count == 0;
            south = south.Select(p => moving.Contains(p) ? new Point(p.X, (p.Y + 1) % height) : p).ToList();
            part1++;
        }
        WriteLine($"Part 1: {part1}");
    }

    private bool IsFree(Point p) => !east.Contains(p) && !south.Contains(p);
}