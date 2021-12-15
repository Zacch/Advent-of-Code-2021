using System.Drawing;
using Extensions;

namespace Advent_of_Code_2021;

public static class Day15
{
    public static void Run()
    {
        var lines = File.ReadLines("Day15.txt").ToList();

        var map = new List<List<int>>();
        foreach (var line in lines)
        {
            map.Add(line.ToCharArray().Select(c => (int) char.GetNumericValue(c)).ToList());
        }
        Console.WriteLine($"Part 1: {FindLowestRisk(map)}");

        var wideMap = new List<List<int>>();
        foreach (var row in map)
        {
            var wideRow = new List<int>(row);
            for (var i = 1; i < 5; i++)
            {
                wideRow.AddRange(row.Select(risk => risk + i > 9 ? risk + i - 9 : risk + i));
            }
            wideMap.Add(wideRow);
        }

        var part2Map = new List<List<int>>();
        for (var i = 0; i < 5; i++)
        {
            foreach (var wideRow in wideMap)
            {
                part2Map.Add(wideRow.Select(risk => risk + i > 9 ? risk + i - 9 : risk + i).ToList());
            }
        }
        
        Console.WriteLine($"Part 2: {FindLowestRisk(part2Map)}");
    }

    private static int FindLowestRisk(List<List<int>> map)
    {
        var width = map[0].Count;
        var height = map.Count;

        var start = new Point(0, 0);
        var goal = new Point(width - 1, height - 1);
        var bounds = new Rectangle(0, 0, width, height);
        var frontier = new PriorityQueue<Point, int>();
        var costSoFar = new Dictionary<Point, int>();

        frontier.Enqueue(start, 0);
        costSoFar[start] = 0;
        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            if (current == goal) break;
            foreach (var next in current.Neighbors())
            {
                if (!bounds.Contains(next)) continue;
                var newCost = costSoFar[current] + map[next.Y][next.X];
                if (costSoFar.ContainsKey(next) && costSoFar[next] <= newCost) continue;
                costSoFar[next] = newCost;
                frontier.Enqueue(next, newCost);
            }
        }

        var totalCost = costSoFar[goal];
        return totalCost;
    }
}