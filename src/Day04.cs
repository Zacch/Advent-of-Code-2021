using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Console;

namespace Advent_of_Code_2021
{
    public static class Day04
    {
        public static void Run()
        {
            var lines = File.ReadLines("Day04.txt").ToArray();

            var numbers = lines[0].Split(',').Select(int.Parse).ToList();

            var boards = Board.Parse(lines);

            var part1 = 0;
            var part2 = 0;

            foreach (var number in numbers)
            {
                foreach (var board in boards.Where(b => b.Score == 0))
                {
                    if (board.Mark(number))
                    {
                        if (part1 == 0) part1 = board.Score;
                        part2 = board.Score;
                    }
                }
            }
            WriteLine($"Part 1: {part1}");
            WriteLine($"Part 2: {part2}");
        }

        private class Board
        {
            private readonly int[,] position = new int[5, 5];
            private readonly bool[,] marked = new bool[5, 5];
            public int Score; 

            public static List<Board> Parse(string[] lines)
            {
                var boards = new List<Board>();
                for (var i = 2; i < lines.Length; i += 6)
                {
                    boards.Add(ParseBoard(lines, i));
                }

                return boards;
            }

            private static Board ParseBoard(string[] lines, int firstLine)
            {
                var board = new Board();
                for (var row = 0; row < 5; row++)
                {
                    var numbers = lines[firstLine + row]
                        .Split(' ')
                        .Where(s => s.Length > 0)
                        .Select(int.Parse);
                    var col = 0;
                    foreach (var number in numbers)
                    {
                        board.position[row, col++] = number;
                    }
                }

                return board;
            }

            public bool Mark(int number)
            {
                for (var row = 0; row < 5; row++)
                {
                    for (var col = 0; col < 5; col++)
                    {
                        if (position[row, col] == number)
                        {
                            marked[row, col] = true;
                            return CheckWin(row, col);
                        }
                    }
                }
                return false;
            }

            private bool CheckWin(int row, int col)
            {
                var rowWin = true;
                var colWin = true;
                for (var i = 0; i < 5; i++)
                {
                    rowWin &= marked[row, i];
                    colWin &= marked[i, col];
                }

                if (!(rowWin || colWin)) return false;
                
                var sum = 0;
                for (var x = 0; x < 5; x++)
                {
                    for (var y = 0; y < 5; y++)
                    {
                        if (marked[x, y] == false) sum += position[x, y];
                    }
                }
                Score = position[row, col] * sum;
                return true;
            }
        }
    }
}