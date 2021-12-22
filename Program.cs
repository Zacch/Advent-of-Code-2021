
using System.Diagnostics;
using Advent_of_Code_2021;

var timer = new Stopwatch();
timer.Start();

Day22.Run();

timer.Stop();
Console.WriteLine($"Done in {timer.ElapsedMilliseconds/1000d} seconds");