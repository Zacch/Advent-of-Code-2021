using System.Collections.Immutable;
using System.Text;

namespace Advent_of_Code_2021;

public static class Day14
{
    public static void Run()
    {
        var lines = File.ReadLines("Day14.txt").ToList();

        var rules = new Dictionary<string, string>();
        foreach (var line in lines)
        {
            var parts = line.Split(" -> ");
            if (parts.Length == 2) rules[parts[0]] = parts[1];
        }

        Part1(lines[0], rules);
        Part2(lines[0], rules);
    }

    private static void Part1(string polymer, Dictionary<string, string> rules)
    {
        for (var turn = 1; turn <= 10; turn++)
        {
            var elements = polymer.ToCharArray();
            var newPolymer = new StringBuilder();
            for (var i = 0; i < polymer.Length - 1; i++)
            {
                newPolymer.Append(elements[i]);
                newPolymer.Append(rules[elements[i].ToString() + elements[i + 1]]);
            }

            newPolymer.Append(elements.Last());
            polymer = newPolymer.ToString();
        }

        var chars = new Dictionary<char, long>();
        foreach (var c in polymer.ToCharArray())
        {
            if (!chars.ContainsKey(c)) chars[c] = 0;
            chars[c]++;
        }
        var counts = chars.Values.ToImmutableSortedSet();
        
        Console.WriteLine($"Part1: {counts.Last() - counts.First()}");
    }
    
    // This could easily calculate part 1 as well
    private static void Part2(string polymer, Dictionary<string, string> rules)
    {
        var pairs = new Dictionary<string, long>();
        var elements = polymer.ToCharArray();

        for (var i = 0; i < elements.Length - 1; i++)
        {
            var pair = elements[i].ToString() + elements[i + 1];
            if (!pairs.ContainsKey(pair)) pairs[pair] = 0;
            pairs[pair]++;
        }

        for (var turn = 1; turn <= 40; turn++)
        {
            var newPairs = new Dictionary<string, long>();
            foreach (var (pair, count) in pairs)
            {
                var inserted = rules[pair];
                var p1 = pair[0] + inserted;
                var p2 = inserted + pair[1];

                if (!newPairs.ContainsKey(p1)) newPairs[p1] = 0;
                if (!newPairs.ContainsKey(p2)) newPairs[p2] = 0;
                newPairs[p1] += count;
                newPairs[p2] += count;
                pairs = newPairs;
            }
        }

        var chars = new Dictionary<char, long>();
        foreach (var (pair, count) in pairs)
        {
            var c = pair[0];
            if (!chars.ContainsKey(c)) chars[c] = 0;
            chars[c] += count;
        }

        chars[polymer.Last()]++;

        var counts = chars.Values.ToImmutableSortedSet();
        Console.WriteLine($"Part 2: {counts.Last() - counts.First()}");
    }
}