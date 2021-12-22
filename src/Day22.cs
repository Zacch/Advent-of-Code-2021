using System.Collections;
using System.Drawing;
using static System.Console;
using static System.Math;

namespace Advent_of_Code_2021;

public static class Day22
{
    private class Range : IEnumerable<int>, IComparable<Range>
    {
        public readonly int Start;
        public readonly int End; // Note: Inclusive! 
        public int Length => End - Start + 1;

        public Range(int start, int end) { Start = start; End = end; }

        public Range(string rangeString)
        {
            var integers = rangeString[2..].Split("..").Select(int.Parse).ToArray();
            Start = integers[0];
            End = integers[1];
        }

        public IEnumerator<int> GetEnumerator() { for (var i= Start; i <= End; i++) yield return i; }
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int CompareTo(Range? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var startComparison = Start.CompareTo(other.Start);
            return startComparison != 0 ? startComparison : End.CompareTo(other.End);
        }

        public bool Overlaps(Range other) => Start <= other.End && End >= other.Start;
        public bool Contains(Range other) => Start <= other.Start && other.End <= End;
        public Range MergeWith(Range other) => new(Min(Start, other.Start), Max(End, other.End));
        
        public override string ToString() => $"{Start}..{End}";
    }
    
    private class Cuboid  // Rectangular cuboid really
    {
        private readonly Range x, y, z;
        public Cuboid(Range x, Range y, Range z) { this.x = x; this.y = y; this.z = z; }

        public bool Overlaps(Cuboid other) => x.Overlaps(other.x) && y.Overlaps(other.y) && z.Overlaps(other.z);
        public bool Contains(Cuboid other) => x.Contains(other.x) && y.Contains(other.y) && z.Contains(other.z);
        public long Count => (long) x.Length * y.Length * z.Length;

        public List<Cuboid> Subtract(Cuboid other)
        {
            var parts = new List<Cuboid>();

            if (x.Start < other.x.Start) parts.Add(new Cuboid(new Range(x.Start, other.x.Start - 1), y, z));
            if (x.End > other.x.End) parts.Add(new Cuboid(new Range(other.x.End + 1, x.End), y, z));
            var xIntersection = new Range(Max(x.Start, other.x.Start), Min(x.End, other.x.End));

            if (y.Start < other.y.Start) parts.Add(new Cuboid(xIntersection, new Range(y.Start, other.y.Start - 1), z));
            if (y.End > other.y.End) parts.Add(new Cuboid(xIntersection, new Range(other.y.End + 1, y.End), z));
            var yIntersection = new Range(Max(y.Start, other.y.Start), Min(y.End, other.y.End));

            if (z.Start < other.z.Start) parts.Add(new Cuboid(xIntersection, yIntersection, new Range(z.Start, other.z.Start - 1)));
            if (z.End > other.z.End) parts.Add(new Cuboid(xIntersection, yIntersection, new Range(other.z.End + 1, z.End)));
            
            return parts;
        }
    }

    // I implemented part 1 using a range for the Z coordinate only and points for the X and Y coordinates.
    // That was enough for part 1, but for part 2 I had to do it properly with cuboids.
    // So here are really two implementations of the same thing.
    public static void Run()
    {
        var lines = File.ReadLines("Day22.txt").ToArray();

        var xyToZ = new Dictionary<Point, SortedSet<Range>>();
        var cuboids = new List<Cuboid>();
        foreach (var line in lines)
        {
            var parts = line.Split(' ');
            var ranges = parts[1].Split(',');
            var xRange = new Range(ranges[0]);
            var yRange = new Range(ranges[1]);
            var zRange = new Range(ranges[2]);

            var cuboid = new Cuboid(xRange, yRange, zRange);
            if (parts[0] == "on")
            {
                Add(cuboid, cuboids);
                if (xRange.Start > 50 || xRange.End < -50 || yRange.Start > 50 || yRange.End < -50 || 
                    zRange.Start > 50 || zRange.End < -50) continue;

                foreach (var x in xRange)
                    foreach (var y in yRange)
                    {
                        var xy = new Point(x, y);
                        if (!xyToZ.ContainsKey(xy)) xyToZ[xy] = new SortedSet<Range>();
                        AddRange(zRange, xyToZ[xy]);
                    }
                continue;
            }
            Subtract(cuboid, cuboids);

            if (xRange.Start > 50 || xRange.End < -50 || yRange.Start > 50 || yRange.End < -50 || 
                zRange.Start > 50 || zRange.End < -50) continue;

            foreach (var x in xRange)
                foreach (var y in yRange)
                {
                    var xy = new Point(x, y);
                    if (!xyToZ.ContainsKey(xy)) xyToZ[xy] = new SortedSet<Range>();
                    RemoveRange(zRange, xyToZ[xy]);
                }
        }
        
        WriteLine($"\nPart 1: {xyToZ.Values.SelectMany(s => s).Select(r => r.Length).Sum()}");
        WriteLine($"\nPart 2: {cuboids.Select(c => c.Count).Sum()}");
    }

    private static void Add(Cuboid newCuboid, List<Cuboid> cuboids)
    {
        if (cuboids.Any(c => c.Contains(newCuboid))) return;
        cuboids.RemoveAll(newCuboid.Contains);
    
        var overlap = cuboids.Where(c => c.Overlaps(newCuboid)).ToList();

        if (overlap.Count == 0)
        {
            cuboids.Add(newCuboid);
            return;
        }

        List<Cuboid> fragments = new() {newCuboid};
        foreach (var cuboid in overlap)
        {
            var newFragments = new List<Cuboid>();
            foreach (var fragment in fragments.Where(f => !cuboid.Contains(f)))
                if (cuboid.Overlaps(fragment))
                    newFragments.AddRange(fragment.Subtract(cuboid));
                else
                    newFragments.Add(fragment);
            
            fragments = newFragments;
        }
        cuboids.AddRange(fragments);
    }
    
    
    private static void Subtract(Cuboid hole, List<Cuboid> cuboids)
    {
        cuboids.RemoveAll(hole.Contains);

        var overlap = cuboids.Where(c => c.Overlaps(hole)).ToList();
        if (overlap.Count == 0) return;
        
        cuboids.RemoveAll(hole.Overlaps);
        foreach (var cuboid in overlap) cuboids.AddRange(cuboid.Subtract(hole));
    }

    private static void AddRange(Range range, SortedSet<Range> set)
    {
        var overlap = set.Where(r => r.Overlaps(range)).ToList();

        if (overlap.Count == 0)
        {
            set.Add(range);
            return;
        }
        
        var newRange = range;
        foreach (var setRange in overlap) newRange = newRange.MergeWith(setRange);

        set.ExceptWith(overlap);
        set.Add(newRange);
    }

    private static void RemoveRange(Range range, SortedSet<Range> set)
    {
        var overlap = set.Where(r => r.Overlaps(range)).ToList();

        if (overlap.Count == 0) return;

        set.ExceptWith(overlap);
        foreach (var overlapping in overlap)
        {
            if (overlapping.Start < range.Start) set.Add(new Range(overlapping.Start, range.Start - 1));
            if (range.End < overlapping.End) set.Add(new Range(range.End + 1, overlapping.End));
        }
    }
}