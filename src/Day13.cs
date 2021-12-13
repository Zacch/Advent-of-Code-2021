using System.Text;

namespace Advent_of_Code_2021;

public static class Day13
{

    private class ComparablePoint : IComparable<ComparablePoint>
    {
        public readonly int X;
        public readonly int Y;

        public ComparablePoint(int x, int y) { X = x; Y = y; }

        public int CompareTo(ComparablePoint? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var yComparison = Y.CompareTo(other.Y);
            return yComparison != 0 ? yComparison : X.CompareTo(other.X);
        }
    }

    public static void Run()
    {
        var lines = File.ReadLines("Day13.txt").ToList();

        var points = new SortedSet<ComparablePoint>();
        var folds = new List<(char, int)>();

        foreach (var line in lines)
        {
            if (line.Contains(','))
            {
                var parts = line.Split(',').Select(int.Parse).ToList();
                points.Add(new ComparablePoint(parts[0], parts[1]));
            }
            if (line.Contains('='))
            {
                folds.Add((line[11], int.Parse(line[13..])));
            }
        }

        var part1Points = Fold(points, folds[0]);
        Console.WriteLine($"Part1: {part1Points.Count}");

        foreach (var fold in folds)
        {
            points = Fold(points, fold);
        }
        Print(points);
        Console.WriteLine($"Part2: {points.Count}");
    }
    
    private static void Print(SortedSet<ComparablePoint> points)
    {
        Console.WriteLine();

        var maxX = points.Select(p => p.X).Max();
        var maxY = points.Select(p => p.Y).Max();
        for (var y = 0; y <= maxY; y++)
        {
            var line = new StringBuilder(maxX);
            for (var x = 0; x <= maxX; x++)
            {
                line.Append(points.Contains(new ComparablePoint(x, y)) ? '#' : ' ');
            }
            Console.WriteLine(line.ToString());
        }
    }

    private static SortedSet<ComparablePoint> Fold(SortedSet<ComparablePoint> points, (char direction, int place) fold)
    {
        var result = new SortedSet<ComparablePoint>();
        
        foreach (var point in points)
        {
            switch (fold.direction)
            {
                case 'x':
                    result.Add(new ComparablePoint(
                        point.X < fold.place ? point.X : 2 * fold.place - point.X, point.Y));
                    break; 
                case 'y':
                    result.Add(new ComparablePoint(point.X,
                        point.Y < fold.place ? point.Y : 2 * fold.place - point.Y));
                    break; 
            }
        }
        
        return result; 
    }
}