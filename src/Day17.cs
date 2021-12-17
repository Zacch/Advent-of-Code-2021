using System.Drawing;
using System.Text.RegularExpressions;

namespace Advent_of_Code_2021;

public static class Day17
{
    private const int MaxSteps = 1000;
    private const int MaxY0 = 1000;

    public static void Run()
    {
        var lines = File.ReadLines("Day17.txt").ToList();
        var numbers = Regex.Matches(lines[0], @"(\-?\d)+")
            .Select(match => match.Value)
            .Where(s => !string.IsNullOrEmpty(s))
            .Select(int.Parse).ToArray();
        var topLeft = new Point(numbers[0], numbers[3]);
        var bottomRight = new Point(numbers[1], numbers[2]);

        var stepSums = new int[MaxSteps];
        var sum = 0;
        for (var i = 0; i < MaxSteps; i++)
        {
            sum += i;
            stepSums[i] = sum;
        }
        
        int part1 = 0;
        for (var y0 = 1; y0 < MaxSteps; y0++)
        {
            var maxY = 0;
            for (var step = 0; step < MaxSteps; step++)
            {
                var y = y0 * step - stepSums[step];
                maxY = Math.Max(y, maxY);
                if (y <= topLeft.Y && y >= bottomRight.Y)
                {
                    if (maxY > part1) part1 = maxY;
                    break;
                }
            }
        }
        Console.WriteLine($"Part 1: {part1}");
        Console.WriteLine($"Part 2: {Part2(topLeft, bottomRight)}");
    }

    private static int Part2(Point topLeft, Point bottomRight)
    {
        var target = new Rectangle(topLeft.X, bottomRight.Y, 
            bottomRight.X - topLeft.X + 1, topLeft.Y - bottomRight.Y + 1);

        var part2 = 0;
        for (var x0 = 0; x0 <= bottomRight.X; x0++)
        {
            for (var y0 = bottomRight.Y; y0 < MaxY0; y0++)
            {
                var x = 0;
                var y = 0;
                var deltaX = x0;
                var deltaY = y0;
                while (x < bottomRight.X && y > bottomRight.Y)
                {
                    x += deltaX;
                    y += deltaY;
                    if (target.Contains(x, y))
                    {
                        part2++;
                        break;
                    }
                    deltaX = Math.Max(0, deltaX - 1);
                    deltaY -= 1;
                }
            }
        }
        return part2;
    }
}