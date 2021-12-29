using System.Data;
using static System.Console;

namespace Advent_of_Code_2021;

public static class Day24
{
    
    /// <summary>
    /// This day is solved in Java using Choco-solver – see Day24.java.
    ///
    /// This method uses an emulation of the ALU to test the two solutions
    /// calculated by the Java code.
    /// </summary>
    public static void Run()
    {
        var lines = File.ReadLines("Day24.txt").ToList();
        
        const long part1Solution = 79197919993985;
        const long part2Solution = 13191913571211;

        WriteLine($"Part 1: {part1Solution}");
        ExecuteAndLog(part1Solution.ToString(), lines);

        WriteLine($"_________________________________________________________");
        WriteLine($"Part 2: {part2Solution}");
        ExecuteAndLog(part2Solution.ToString(), lines);
        
    }
    
    private static void ExecuteAndLog(string inputString, List<string> lines)
    {
        var alu = new Alu(inputString);

        foreach (var line in lines)
        {
            alu.Execute(line);
            if (line.StartsWith("inp")) WriteLine();
            WriteLine($"{line,-10} {alu}");
        }
    }


    private class Alu
    {
        private long w, x, y, z;

        private readonly Queue<char> input = new();

        public Alu(string inputString)
        {
            foreach (var c in inputString.ToCharArray()) input.Enqueue(c);
        }

        public void Execute(string instruction)
        {
            var parts = instruction.Split(' ');
            if (parts.Length < 2) return;

            switch (parts[0])
            {
                case "inp":
                    DoInput(parts[1]);
                    break;
                case "add":
                    DoAdd(parts[1], parts[2]);
                    break;
                case "mul":
                    DoMultiply(parts[1], parts[2]);
                    break;
                case "div":
                    DoDivide(parts[1], parts[2]);
                    break;
                case "mod":
                    DoModulus(parts[1], parts[2]);
                    break;
                case "eql":
                    DoEqual(parts[1], parts[2]);
                    break;
                default:
                    throw new DataException($"Unknown instruction {parts[0]}");
            }
        }

        private void DoInput(string variable)
        {
            Assign(variable, (int) char.GetNumericValue(input.Dequeue()));
        }

        private void DoAdd(string variable, string argument)
        {
            Assign(variable, GetValue(variable) + GetValue(argument));
        }

        private void DoMultiply(string variable, string argument)
        {
            Assign(variable, GetValue(variable) * GetValue(argument));
        }

        private void DoDivide(string variable, string argument)
        {
            Assign(variable, GetValue(variable) / GetValue(argument));
        }

        private void DoModulus(string variable, string argument)
        {
            Assign(variable, GetValue(variable) % GetValue(argument));
        }

        private void DoEqual(string variable, string argument)
        {
            Assign(variable, GetValue(variable) == GetValue(argument) ? 1 : 0);
        }

        private long GetValue(string argument)
        {
            return argument switch
            {
                "w" => w,
                "x" => x,
                "y" => y,
                "z" => z,
                _ => int.Parse(argument)
            };
        }

        private void Assign(string variable, long value)
        {
            switch (variable)
            {
                case "w": w = value; break;
                case "x": x = value; break;
                case "y": y = value; break;
                case "z": z = value; break;
            }
        }

        public override string ToString()
        {
            return $"[W {w,3}, X {x,3}, Y {y,3}, Z {z,3}] - [{string.Join(' ', input)}]";
        }
    }

}