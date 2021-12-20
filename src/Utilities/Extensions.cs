using System.Drawing;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable once CheckNamespace

namespace Extensions;

public static class EnumerableExtension
{
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self) 
        => self.Select((item, index) => (item, index)); 
}

public static class PointExtension
{
    public static Point Plus(this Point a, Point b) => new Point(a.X + b.X, a.Y + b.Y);
    public static Point Minus(this Point a, Point b) => new Point(a.X - b.X, a.Y - b.Y);
    
    public static Point North(this Point p) { return new Point(p.X, p.Y - 1); }
    public static Point South(this Point p) { return new Point(p.X, p.Y + 1); }
    public static Point East(this Point p) { return new Point(p.X + 1, p.Y); }
    public static Point West(this Point p) { return new Point(p.X - 1, p.Y); }

    public static List<Point> Neighbors(this Point p) { return new List<Point> { p.North(), p.South(), p.West(), p.East() }; }

    public static List<Point> AllNeighbors(this Point p)
    {
        return new List<Point>
        {
            p.North(), p.South(), p.West(), p.East(),
            p.North().East(), p.North().West(), p.South().East(), p.South().West()
        };
    }
}