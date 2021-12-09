using System.Drawing;
using Extensions;
using static System.Console;

namespace Advent_of_Code_2021
{
    public static class Day09
    {

        public static void Run()
        {
            var lines = File.ReadLines("Day09.txt").ToList();

            var part1 = 0;
            for (var x = 0; x < lines[0].Length; x++)
            {
                for (var y = 0; y < lines.Count; y++)
                {
                    var point = new Point(x, y);
                    if (IsLowPoint(point, lines)) part1 += GetHeight(point, lines) + 1;
                }
            }
            WriteLine($"Part 1: {part1}");

            var basins = FindBasins(lines);

            var sizes = basins.Select(b => b.Count).ToList();
            sizes.Sort();
            sizes.Reverse();
            
            WriteLine($"Part 2: {sizes[0] * sizes[1] * sizes[2]}");
        }

        private static bool IsLowPoint(Point point, List<string> map)
        {
            var height = GetHeight(point, map);
            return point.Neighbors().TrueForAll(neighbor => GetHeight(neighbor, map) > height);
        }

        private static int GetHeight(Point point, List<string> map)
        {
            if (point.X < 0 || point.Y < 0 || point.X >= map[0].Length || point.Y >= map.Count) return 9;
            return int.Parse(map[point.Y].ToCharArray()[point.X].ToString());
        }
        
        private static List<List<Point>> FindBasins(List<string> map)
        {
            var basins = new List<List<Point>>();

            for (var x = 0; x < map[0].Length; x++)
            {
                for (var y = 0; y < map.Count; y++)
                {
                    var p = new Point(x, y);
                    if (GetHeight(p, map) == 9 || basins.SelectMany(list => list).Contains(p)) continue;
                    basins.Add(ExploreBasin(p, map));
                }
            }
            
            return basins;
        }

        private static List<Point> ExploreBasin(Point start, List<string> map)
        {
            var frontier = new Queue<Point>();
            frontier.Enqueue(start);
            var basin = new List<Point> { start };

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();
                foreach (var next in current.Neighbors())
                {
                    if (GetHeight(next, map) == 9) continue;
                    if (basin.Contains(next)) continue;

                    frontier.Enqueue(next);
                    basin.Add(next);
                }
            }
            
            return basin;
        }
    }
}