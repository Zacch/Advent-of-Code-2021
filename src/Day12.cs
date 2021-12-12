using static System.Console;

namespace Advent_of_Code_2021;

public static class Day12
{
    // This class turned out to be unnecessary – it's just a boxed int 🙂.
    private class Cave: IComparable<Cave>
    {
        public readonly string Name;
        public bool IsSmall => char.IsLower(Name.ElementAt(0));
        public readonly Dictionary<string, Cave> Connections = new();
        
        public Cave(string name) { Name = name; }

        public int CompareTo(Cave? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            return ReferenceEquals(null, other) ? 1 : string.Compare(Name, other.Name, StringComparison.Ordinal);
        }
    }

    private class Path
    {
        public List<Cave> Caves = new();
        public bool HasDoubleVisit;

        public Path() {}

        public Path(Path other)
        {
            Caves = new List<Cave>(other.Caves);
            HasDoubleVisit = other.HasDoubleVisit;
        }

        public override string ToString() { return string.Join(",", Caves.Select(c => c.Name)); }
    }

    public static void Run()
    {
        var lines = File.ReadLines("Day12.txt").ToList();

        var caves = new Dictionary<string, Cave>();
        foreach (var line in lines)
        {
            var ends = line.Split('-');
            if (!caves.ContainsKey(ends[0])) caves[ends[0]] = new Cave(ends[0]);
            if (!caves.ContainsKey(ends[1])) caves[ends[1]] = new Cave(ends[1]);
            var cave0 = caves[ends[0]];
            var cave1 = caves[ends[1]];
            cave0.Connections[cave1.Name] = cave1;
            cave1.Connections[cave0.Name] = cave0;
        }

        Part1(caves);
        Part2(caves);
    }

    private static void Part1(Dictionary<string, Cave> caves)
    {
        var paths = new List<List<Cave>>();

        var frontier = new Stack<List<Cave>>();
        frontier.Push(new List<Cave> {caves["start"]});

        while (frontier.Count > 0)
        {
            var current = frontier.Pop();
            foreach (var next in current.Last().Connections.Values)
            {
                if (next.IsSmall && current.Contains(next)) continue;
                var newPath = new List<Cave>(current) {next};
                if (next.Name == "end")
                {
                    paths.Add(newPath);
                }
                else
                {
                    frontier.Push(newPath);
                }
            }
        }

        WriteLine($"Part 1: {paths.Count}");
    }

    private static void Part2(Dictionary<string, Cave> caves)
    {
        var paths = new List<Path>();

        var startPath = new Path {Caves = new List<Cave> {caves["start"]}};
        var frontier = new Stack<Path>();
        frontier.Push( startPath);

        while (frontier.Count > 0)
        {
            var current = frontier.Pop();
            foreach (var next in current.Caves.Last().Connections.Values)
            {
                if (next.Name == "start") continue;
                if (next.IsSmall && current.Caves.Contains(next) && current.HasDoubleVisit) continue;
                var newPath = new Path(current);
                if (next.IsSmall && newPath.Caves.Contains(next)) newPath.HasDoubleVisit = true;
                newPath.Caves.Add(next);
                if (next.Name == "end")
                {
                    paths.Add(newPath);
                }
                else
                {
                    frontier.Push(newPath);
                }
            }
        }

        WriteLine($"Part 2: {paths.Count}");
    }
}