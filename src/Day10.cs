using System.Collections.Immutable;
using static System.Console;

namespace Advent_of_Code_2021;

public static class Day10
{

    public static void Run()
    {
        var lines = File.ReadLines("Day10.txt").ToList();

        var part1 = 0;
        var completionScores = new List<long>();
        foreach (var line in lines)
        {
            var c = FindIllegalChar(line);
            part1 += c switch
            {
                ')' => 3,
                ']' => 57,
                '}' => 1197,
                '>' => 25137,
                _ => 0,
            };
            if (c == ' ')
            {
                completionScores.Add(CalculateCompletionScore(line)); 
            }
        }
        WriteLine($"Part 1: {part1}");

        var sorted = completionScores.ToImmutableSortedSet();
        
        WriteLine($"Part 2: {sorted[sorted.Count/2]}");
    }

    
    private static char FindIllegalChar(string line)
    {
        var stack = new Stack<char>();

        foreach (var c in line.ToCharArray())
        {
            switch (c)
            {
                case '(': case '[': case '{': case '<':
                    stack.Push(c);
                    break;
                case ')':
                    if (stack.Pop() != '(') return c;
                    break;
                case ']':
                    if (stack.Pop() != '[') return c;
                    break;
                case '}':
                    if (stack.Pop() != '{') return c;
                    break;
                case '>':
                    if (stack.Pop() != '<') return c;
                    break;
            }
        }
        return ' ';
    }
    
    private static long CalculateCompletionScore(string line)
    {
        var stack = new Stack<char>();
        foreach (var c in line.ToCharArray())
        {
            switch (c)
            {
                case '(': case '[': case '{': case '<':
                    stack.Push(c);
                    break;
                case ')': case ']':  case '}': case '>':
                    stack.Pop();
                    break;
            }
        }

        var score = 0L;
        while (stack.Count > 0)
        {
            score = stack.Pop() switch
            {
                '(' => 5 * score + 1,
                '[' => 5 * score + 2,
                '{' => 5 * score + 3,
                '<' => 5 * score + 4,
                _ => -1
            };
        }

        return score;
    }

}