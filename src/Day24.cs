using System.Data;
using static System.Console;

namespace Advent_of_Code_2021;

public static class Day24
{
    
    public static void Run()
    {
        var lines = File.ReadLines("Day24.txt").ToList();

        // These were arrived at by hand:
        // 1. start with 99999999999999 (or 11111111111111 to find the lowest serial)
        // 2. run "ExecuteAndLog"
        // 3. Adjust the first possible digit so that an "eql x w" comparison returns 1
        // 4. Repeat until the alu's z is 0 at the end

        var highestSerialNumber = 79197919993985;
        var lowestSerialNumber = 13191913571211;

        ExecuteAndLog(highestSerialNumber.ToString(), lines);
        ExecuteAndLog(lowestSerialNumber.ToString(), lines);
        
        WriteLine($"Part 1: {highestSerialNumber}");
        WriteLine($"Part 2: {lowestSerialNumber}");
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