using Advent_of_Code_2021.Utilities;
using static System.Console;

namespace Advent_of_Code_2021;

public static class Day19
{
    public static void Run()
    {
        var scanners = ParseInput();
        WriteLine($"Part 1: {Part1(scanners)}");
        WriteLine($"Part 2: {Part2(scanners)}");
    }

    private static List<Scanner> ParseInput()
    {
        var lines = File.ReadLines("Day19.txt").ToArray();
        var scanners = new List<Scanner>();

        var current = new Scanner("scanner 0");
        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            if (line.StartsWith("---"))
            {
                current.CalculateDeltas();
                scanners.Add(current);
                current = new Scanner(line.Replace("-", "").Trim());
                continue;
            }
            current.Beacons.Add(new Point3D(line.Split(',').Select(int.Parse).ToArray()));
        }
        current.CalculateDeltas();
        scanners.Add(current);
        return scanners;
    }

    private static int Part1(List<Scanner> scanners)
    {
        Dictionary<string, Scanner> scannersLeft = new();
        foreach (var scanner in scanners.Skip(1)) scannersLeft[scanner.Name] = scanner;
        
        while (scannersLeft.Count > 0)
        {
            for (var orientation = 0; orientation < 24; orientation++)
            {
                foreach (var scanner in scannersLeft.Values)
                {
                    var orientedScanner = scanner.ToOrientation(orientation);
                    if (!orientedScanner.Overlaps(scanners[0])) continue;
                    
                    var position = orientedScanner.Position;
                    scanner.Position = position;
                    scanners[0].AddBeacons(orientedScanner.Beacons.Select(b => b + position));
                    scannersLeft.Remove(orientedScanner.Name);
                }
            }
        }
        return scanners[0].Beacons.Count;
    }

    private static long Part2(List<Scanner> scanners)
    {
        var longest = 0L;
        for (var i = 0; i < scanners.Count - 1; i++)
        {
            for (var j = i + 1; j < scanners.Count; j++)
            {
                var dist = scanners[i].Position.ManhattanDistanceTo(scanners[j].Position);
                if (dist > longest) longest = dist;
            }
        }
        return longest;
    }
}

public class Scanner
{
    public readonly string Name;
    public Point3D Position = new();
    public List<Point3D> Beacons = new();
    private readonly Dictionary<Point3D, HashSet<string>> deltas = new();

    public Scanner(string name) { Name = name; }

    public Scanner ToOrientation(int orientation)
    {
        var result = new Scanner(Name)
        {
            Position = Position.ToOrientation(orientation),
            Beacons = Beacons.Select(b => b.ToOrientation(orientation)).ToList()
        };
        result.CalculateDeltas();
        return result;
    }

    public bool Overlaps(Scanner other)
    {
        var matchingBeacons = new Dictionary<Point3D, Point3D>();
        foreach (var beacon in Beacons)
        {
            var beaconDeltas = deltas[beacon];
            foreach (var (otherBeacon, otherDeltas) in other.deltas)
            {
                if (beaconDeltas.Count(delta => otherDeltas.Contains(delta)) <= 10) continue;
                matchingBeacons[beacon] = otherBeacon;
            }
        }

        if (matchingBeacons.Count < 10) return false;
        var distances = matchingBeacons.Keys
            .Select(b => matchingBeacons[b] - b).ToArray();
        if (distances.Distinct().Count() > 1) return false;
        Position = distances[0];
        return true;
    }
    
    public void CalculateDeltas()
    {
        foreach (var beacon in Beacons)
        {
            deltas[beacon] = new HashSet<string>(Beacons.Select(other => 
                beacon.DistanceTo(other)).Where(p => !p.IsOrigin).Select(p => p.ToString()));
        }
    }

    public void AddBeacons(IEnumerable<Point3D> newBeacons)
    {
        foreach (var newBeacon in newBeacons)
        {
            if (Beacons.Contains(newBeacon)) continue;
            foreach (var beacon in Beacons) deltas[beacon].Add(beacon.DistanceTo(newBeacon).ToString());
            deltas[newBeacon] = new HashSet<string>(Beacons.Select(b => newBeacon.DistanceTo(b).ToString()));
            Beacons.Add(newBeacon);
        }
    }
}