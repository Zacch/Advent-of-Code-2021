using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using static System.Console;

namespace Advent_of_Code_2021
{
    public static class Day05
    {
        public static void Run()
        {
            var lines = File.ReadLines("Day05.txt").Select(Line.Parse).ToList();

            var points = new Dictionary<Point, int>();

            foreach (var line in lines.Where(l => l.IsVertical))
            {
                for (var y = Math.Min(line.P1.Y, line.P2.Y); y <= Math.Max(line.P1.Y, line.P2.Y); y++)
                {
                    var point = new Point(line.P1.X, y);
                    if (!points.ContainsKey(point)) points[point] = 0;
                    points[point]++;
                }
            }
            foreach (var line in lines.Where(l => l.IsHorizontal))
            {
                for (var x = Math.Min(line.P1.X, line.P2.X); x <= Math.Max(line.P1.X, line.P2.X); x++)
                {
                    var point = new Point(x, line.P1.Y);
                    if (!points.ContainsKey(point)) points[point] = 0;
                    points[point]++;
                }
            }
            var part1 = points.Values.Count(v => v > 1);

            foreach (var line in lines.Where(l => l.IsDiagonal))
            {
                var x = line.P1.X;
                var y = line.P1.Y;
                var deltaX = line.P1.X < line.P2.X ? 1 : -1;
                var deltaY = line.P1.Y < line.P2.Y ? 1 : -1;
                while (x - deltaX != line.P2.X)
                {
                    var point = new Point(x, y);
                    if (!points.ContainsKey(point)) points[point] = 0;
                    points[point]++;
                    x += deltaX;
                    y += deltaY;
                }
            }
            var part2 = points.Values.Count(v => v > 1);
            
            WriteLine($"Part 1: {part1}");
            WriteLine($"Part 2: {part2}");
        }

        private class Line
        {
            public readonly Point P1, P2;
            public bool IsHorizontal => P1.Y == P2.Y;
            public bool IsVertical => P1.X == P2.X;
            public bool IsDiagonal => !(IsVertical || IsHorizontal);

            public Line(Point p1, Point p2)
            {
                P1 = p1;
                P2 = p2;
            }

            public static Line Parse(string s)
            {
                var parts = string.Join(",", s.Split(" -> "));
                var coords = parts.Split(',').Select(int.Parse).ToList();
                return new Line(new Point(coords[0], coords[1]), new Point(coords[2], coords[3]));
            }
        }
    }
}