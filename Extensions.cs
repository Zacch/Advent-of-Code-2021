using System.Drawing;

// ReSharper disable once CheckNamespace
namespace Extensions;

public static class PointExtension
{
    public static Point North(this Point p) { return new Point(p.X, p.Y - 1); }
    public static Point South(this Point p) { return new Point(p.X, p.Y + 1); }
    public static Point East(this Point p) { return new Point(p.X + 1, p.Y); }
    public static Point West(this Point p) { return new Point(p.X - 1, p.Y); }

    public static List<Point> Neighbors(this Point p) { return new List<Point> { p.North(), p.South(), p.West(), p.East() }; }
}