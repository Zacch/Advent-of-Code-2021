using System.Data;

namespace Advent_of_Code_2021;

public static class Day18
{
    public static void Run()
    {
        var lines = File.ReadLines("Day18.txt").ToArray();

        var sum = lines[0];
        foreach (var line in lines[1..])
        {
            sum = Add(sum, line);
        }

        Console.WriteLine($"Part 1: {Magnitude(sum)}");
        Console.WriteLine($"Part 2: {Part2(lines)}");
    }
    
    private static string Add(string x, string y)
    {
        var sum = $"[{x},{y}]";
        return Reduce(sum);
    }

    private static string Reduce(string x)
    {
        bool done;
        do
        {
            var x0 = "";
            while (!x0.Equals(x))
            {
                x0 = x;
                x = Explode(x);
            }
            
            x = Split(x);
            done = x0.Equals(x);
        } while (!done);
        
        return x;
    }
    
    private static string Explode(string s)
    {
        int level = 0;
        int explodingPairStart = 0;
        for (var i = 0; i < s.Length; i++)
        {
            if (s[i] == '[')
            {
                level++;
                if (level < 5) continue;
                explodingPairStart = i;
                break;
            }
            if (s[i] == ']') level--;
        }
        if (explodingPairStart == 0) return s;

        var explodingPairStop = s.IndexOf(']', explodingPairStart);
        var pair = s[(explodingPairStart + 1)..explodingPairStop];
        var regularNumbers = pair.Split(',').Select(int.Parse).ToArray();

        var leftPart = s[0..explodingPairStart];
        for (var i = leftPart.Length - 1; i >= 0; i--)
        {
            if (!char.IsDigit(leftPart[i])) continue;

            if (char.IsDigit(leftPart[i - 1]))
            {
                var x = int.Parse(leftPart[(i - 1)..(i + 1)]);
                leftPart = $"{leftPart[..(i - 1)]}{x + regularNumbers[0]}{leftPart[(i + 1)..]}";
                break;
            }
            var y = (int) char.GetNumericValue(leftPart[i]);
            leftPart = $"{leftPart[..i]}{y + regularNumbers[0]}{leftPart[(i + 1)..]}";
            break;
        }
        
        var rightPart = s[(explodingPairStop + 1)..];
        for (var i = 0; i < rightPart.Length - 1; i++)
        {
            if (!char.IsDigit(rightPart[i])) continue;

            if (char.IsDigit(rightPart[i + 1]))
            {
                var x = int.Parse(rightPart[i..(i + 2)]);
                rightPart = $"{rightPart[..i]}{x + regularNumbers[1]}{rightPart[(i + 2)..]}";
                break;
            }
            var y = (int) char.GetNumericValue(rightPart[i]);
            rightPart = $"{rightPart[..i]}{y + regularNumbers[1]}{rightPart[(i + 1)..]}";
            break;

        }

        return $"{leftPart}0{rightPart}";
    }

    private static string Split(string s)
    {
        for (var i = 0; i < s.Length - 1; i++)
        {
            if (!char.IsDigit(s[i])) continue;
            if (!char.IsDigit(s[i + 1])) continue;

            var x = int.Parse(s[i..(i + 2)]);
            s = $"{s[..i]}[{x / 2},{x / 2 + x % 2}]{s[(i + 2)..]}";
            break;
        }

        return s;
    }

    private class TreeNode
    {
        // This was overkill :). Since each character becomes one token, I could have used chars instead
        // of tokens (the numbers in a reduced snailfish number are always a single digit, 0 - 9) 
        private enum TokenType { Open, Comma, Close, Number }
        private class Token
        {
            public readonly TokenType Type;
            public readonly int Value;

            public Token(TokenType type) { Type = type; }
            public Token(int value) { Type = TokenType.Number; Value = value; }
        }

        private readonly TreeNode? left;
        private readonly TreeNode? right;
        private readonly bool isLeaf;
        private readonly int regularNumber;

        public static TreeNode Parse(string s)
        {
            var tokens = Tokenize(s);
            return new TreeNode(tokens);
        }

        private static Queue<Token> Tokenize(string s)
        {
            var tokens = new Queue<Token>();
            foreach (var c in s.ToCharArray())
            {
                tokens.Enqueue( c switch
                {
                    '[' => new Token(TokenType.Open),
                    ',' => new Token(TokenType.Comma),
                    ']' => new Token(TokenType.Close),
                    _ => new Token((int) char.GetNumericValue(c)),
                });
            }

            return tokens;
        }
        
        private TreeNode(Queue<Token> tokens)
        {
            var token = tokens.Dequeue();
            if (token.Type == TokenType.Number)
            {
                left = null;
                right = null;
                isLeaf = true;
                regularNumber = token.Value;
                return;
            }

            if (token.Type != TokenType.Open) throw new DataException();
            left = new TreeNode(tokens);
            tokens.Dequeue();
            right = new TreeNode(tokens);
            tokens.Dequeue();
        }

        public int Magnitude()
        {
            if (isLeaf) return regularNumber;
            return 3 * left!.Magnitude() + 2 * right!.Magnitude();
        }
    }

    private static int Magnitude(string sum)
    {
        var tree = TreeNode.Parse(sum);
        return tree.Magnitude();
    }
    
    private static int Part2(string[] lines)
    {
        var result = 0;
        for (var x = 0; x < lines.Length; x++)
        {
            for (var y = 0; y < lines.Length; y++)
            {
                if (x == y) continue;
                result = Math.Max(result, Magnitude(Add(lines[x], lines[y])));
            }
        }

        return result;
    }
}