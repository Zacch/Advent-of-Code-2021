using System.Drawing;
using Extensions;
using static System.Console;

namespace Advent_of_Code_2021;

public static class Day11
{
    // This class turned out to be unnecessary – it's just a boxed int 🙂.
    private class DumboOctopus
    {
        public int Energy;

        public DumboOctopus(int energy) { Energy = energy; }
    }

    public static void Run()
    {
        var lines = File.ReadLines("Day11.txt").ToList();
        var grid = new DumboOctopus[100];

        for (var y = 0; y < 10; y++)
        {
            var line = lines[y];
            for (var x = 0; x < 10; x++)
            {
                grid[y * 10 + x] = new DumboOctopus((int) char.GetNumericValue(line, x));
            }
        }

        var part1 = 0;
        var step = 0;
        while (grid.Any(o => o.Energy > 0))
        {
            step++;
            foreach (var octopus in grid) octopus.Energy++;
            
            for (var x = 0; x < 10; x++)
            {
                for (var y = 0; y < 10; y++)
                {
                    if (Octopus(x, y, grid).Energy <= 9) continue;
                    Flash(x, y, grid);
                }
            }

            part1 += grid.Count(o => o.Energy == 0);
            if (step == 100) WriteLine($"Part 1: {part1}");
        }
        WriteLine($"Part 2: {step}");
    }

    private static DumboOctopus Octopus(int x, int y, DumboOctopus[] grid)
    {
        return grid[y * 10 + x];
    }

    private static void Flash(int x, int y, DumboOctopus[] grid)
    {
        Octopus(x, y, grid).Energy = 0;
        foreach (var p in Neighbours(x, y))
        {
            var neighbour = Octopus(p.X, p.Y, grid);
            if (neighbour.Energy == 0) continue;
            neighbour.Energy++;
            if (neighbour.Energy > 9) Flash(p.X, p.Y, grid);
        }
    }

    private static List<Point> Neighbours(int x, int y)
    {
        var r = new Rectangle(0, 0, 10, 10);
        var p = new Point(x, y);
        return p.AllNeighbors().Where(neighbor => r.Contains(neighbor)).ToList();
    }
}