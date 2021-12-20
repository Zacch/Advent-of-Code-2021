using System.Drawing;
using System.Text;
using Extensions;
using static System.Console;

namespace Advent_of_Code_2021;

public static class Day20
{
    public static void Run()
    {
        var lines = File.ReadLines("Day20.txt").ToArray();

        var rules = lines[0];
        var image = new HashSet<Point>();

        foreach (var (line, y) in lines.Skip(2).WithIndex())
            foreach (var (c, x) in line.ToCharArray().WithIndex())
                if (c == '#') image.Add(new Point(x, y));

        var bounds = new Rectangle(0, 0, lines[2].Length, lines.Length - 2);
        bounds.Inflate(new Size(60, 60));

        var outerBounds = bounds;
        outerBounds.Inflate(new Size(60, 60));

        for (var i = 1; i <= 50; i++)
        {
            image = Enhance(image, rules, outerBounds);
            if (i == 2) WriteLine($"Part 1: {image.Where(p => bounds.Contains(p)).ToHashSet().Count}");
        }

        WriteLine($"Part 2: {image.Where(p => bounds.Contains(p)).ToHashSet().Count}");
    }

    private static readonly List<Point> BitOffsets = new()
    {
        new Point(-1, -1), new Point(0, -1), new Point(1, -1),
        new Point(-1, 0), new Point(0, 0), new Point(1, 0),
        new Point(-1, 1), new Point(0, 1), new Point(1, 1)
    };
    
    private static HashSet<Point> Enhance(HashSet<Point> image, string rules, Rectangle outerBounds)
    {
        var enhancedImage = new HashSet<Point>();

        for (var x = outerBounds.Left; x < outerBounds.Right; x++)
            for (var y = outerBounds.Top; y < outerBounds.Bottom; y++)
            {
                var address = new StringBuilder();
                var p = new Point(x, y);
                foreach (var offset in BitOffsets) address.Append(image.Contains(p.Plus(offset)) ? '1' : '0');

                var index = Convert.ToInt16(address.ToString(), 2);
                if (rules[index] == '#') enhancedImage.Add(p);
            }

        return enhancedImage;
    }
}