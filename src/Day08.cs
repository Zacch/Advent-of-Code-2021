using static System.Console;

namespace Advent_of_Code_2021;

public static class Day08
{
    /// <summary>The display segments used in each digit (plus two extra elements at the end – what a hack ;))</summary>
    private static readonly List<char>[] Segments = {
        new() {'a', 'b', 'c', 'e', 'f', 'g'},      // 0
        new() {'c', 'f'},
        new() {'a', 'c', 'd', 'e', 'g'},           // 2
        new() {'a', 'c', 'd', 'f', 'g'},
        new() {'b', 'c', 'd', 'f'},                // 4
        new() {'a', 'b', 'd', 'f', 'g'},
        new() {'a', 'b', 'd', 'e', 'f', 'g'},      // 6
        new() {'a', 'c', 'f'},
        new() {'a', 'b', 'c', 'd', 'e', 'f', 'g'}, // 8
        new() {'a', 'b', 'c', 'd', 'f', 'g'},
        new() {'b', 'c', 'e', 'f'},                // These segments are missing in some of the 5-segment digits
        new() {'c', 'd', 'e'}                      // These segments are missing in some of the 6-segment digits
    };

    public static void Run()
    {
        var lines = File.ReadLines("Day08.txt").ToList();
        var allShownDigits = lines.Select(line => line.Split("| ")[1].Split(' ')
            .ToList()).SelectMany(s => s);
        
        WriteLine($"Part 1: {allShownDigits.Where(s => s.Length != 5 && s.Length != 6).ToList().Count}");
        WriteLine($"Part 2: {lines.Select(DecodeLine).Sum()}");
    }

    private static int DecodeLine(string line)
    {
        var uniqueDigits = line.Split(" | ")[0].Split(' ').ToList();
        var displayDigits = line.Split(" | ")[1].Split(' ').ToList();

        var possibleValues = new Dictionary<char, List<char>>
        {
            {'a', new List<char>{'a', 'b', 'c', 'd', 'e', 'f', 'g'}},
            {'b', new List<char>{'a', 'b', 'c', 'd', 'e', 'f', 'g'}},
            {'c', new List<char>{'a', 'b', 'c', 'd', 'e', 'f', 'g'}},
            {'d', new List<char>{'a', 'b', 'c', 'd', 'e', 'f', 'g'}},
            {'e', new List<char>{'a', 'b', 'c', 'd', 'e', 'f', 'g'}},
            {'f', new List<char>{'a', 'b', 'c', 'd', 'e', 'f', 'g'}},
            {'g', new List<char>{'a', 'b', 'c', 'd', 'e', 'f', 'g'}}
        };

        foreach (var digit in uniqueDigits)
        {
            switch (digit.Length)
            {
                case 2:
                    Constrain(digit, possibleValues, 1);
                    break;
                case 3:
                    Constrain(digit, possibleValues, 7);
                    break;
                case 4:
                    Constrain(digit, possibleValues, 4);
                    break;
                case 5:
                    var missing = new string(Segments[8].Where(c => !digit.ToCharArray().Contains(c)).ToArray());
                    Constrain(missing, possibleValues, 10);
                    break;
                case 6:
                    missing = new string(Segments[8].Where(c => !digit.ToCharArray().Contains(c)).ToArray());
                    Constrain(missing, possibleValues, 11);
                    break;
            }
        }
        var mapping = MakeMapping(possibleValues);

        var result = 0;
        foreach (var displayDigit in displayDigits)
        {
            result = result * 10 + SegmentsToDigit(displayDigit.ToCharArray().Select(c => mapping[c]).ToList());
        }
        return result;
    }

    private static void Constrain(string digit, Dictionary<char, List<char>> possibleValues, int segmentIndex)
    {
        foreach (var c in digit.ToCharArray())
        {
            possibleValues[c] = possibleValues[c].Where(c2 => Segments[segmentIndex].Contains(c2)).ToList();
        }
    }

    private static Dictionary<char, char> MakeMapping(Dictionary<char,List<char>> possibleValues)
    {
        Dictionary<char, char> mapping = new();
        List<char> usedValues = new();
        var leftToAssign = new Dictionary<char, List<char>>(possibleValues);
        while (leftToAssign.Count > 0)
        {
            foreach (var (c, values) in leftToAssign)
            {
                var alternatives = values.Where(value => !usedValues.Contains(value)).ToList();
                if (alternatives.Count != 1) continue;
                mapping[c] = alternatives[0];
                usedValues.Add(alternatives[0]);
                leftToAssign.Remove(c);
            }
        }

        return mapping;
    }

    private static int SegmentsToDigit(List<char> digitSegments)
    {
        for (var i = 0; i <= 9; i++)
        {
            if (Segments[i].Count != digitSegments.Count) continue;
            if (digitSegments.TrueForAll(c => Segments[i].Contains(c)))
            {
                return i;
            }
        }
        throw new ArgumentException("That's not a valid digit :P");
    }
}