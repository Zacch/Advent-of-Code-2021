using System.Text;
using Extensions;
using static System.Console;

namespace Advent_of_Code_2021;

public static class Day23
{
    /*
        #################
        #01  2  3  4  56#
        ### 7# 8# 9#10###
          #11#12#13#14#
          #############
    */
    private static readonly Dictionary<int, List<int>> Edges = new()
    {
        [0] = new(){1},
        [1] = new(){0, 2, 7},
        [2] = new(){1, 3, 7, 8},
        [3] = new(){2, 4, 8, 9},
        [4] = new(){3, 5, 9, 10},
        [5] = new(){4, 6, 10},
        [6] = new(){5},
        [7] = new(){1, 2, 11},
        [8] = new(){2, 3, 12},
        [9] = new(){3, 4, 13},
        [10] = new(){4, 5, 14},
        [11] = new(){7},
        [12] = new(){8},
        [13] = new(){9},
        [14] = new(){10}
    };

    private static readonly Dictionary<int, List<int>> DoubleCostEdge = new()
    {
        [0] = new(),
        [1] = new(){2, 7},
        [2] = new(){1, 3, 7, 8},
        [3] = new(){2, 4, 8, 9},
        [4] = new(){3, 5, 9, 10},
        [5] = new(){4, 10},
        [6] = new(),
        [7] = new(){1, 2},
        [8] = new(){2, 3},
        [9] = new(){3, 4},
        [10] = new(){4, 5},
        [11] = new(),
        [12] = new(),
        [13] = new(),
        [14] = new()
    };

    private static readonly Dictionary<char, List<int>> Rooms = new()
    {
        ['A'] = new(){7, 11},
        ['B'] = new(){8, 12},
        ['C'] = new(){9, 13},
        ['D'] = new(){10, 14}
    };

    private static readonly Dictionary<int, char> HomeTo = new()
    {
        [0] = ' ', [1] = ' ', [2] = ' ', [3] = ' ', [4] = ' ', [5] = ' ', [6] = ' ', 
        [7] = 'A', [8] = 'B', [9] = 'C', [10] = 'D', [11] = 'A', [12] = 'B', [13] = 'C', [14] = 'D',
    };


    private static readonly Dictionary<char, int> Costs = new() { [' '] = 0, ['A'] = 1, ['B'] = 10, ['C'] = 100, ['D'] = 1000 };

    private const string Goal = "       ABCDABCD";

    public static void Run()
    {
        var lines = File.ReadLines("Day23.txt").ToArray();

        var start = new StringBuilder("       ");
        start.Append(lines[2][3]).Append(lines[2][5]).Append(lines[2][7]).Append(lines[2][9]);
        start.Append(lines[3][3]).Append(lines[3][5]).Append(lines[3][7]).Append(lines[3][9]);
        
        WriteLine($"Part 1: {Part1(start.ToString())}");
    }

    private static int Part1(string start)
    {
        var frontier = new PriorityQueue<string, int>();
        frontier.Enqueue(start, 0);
        var cameFrom = new Dictionary<string, string>() { [start] = "" };
        var costSoFar = new Dictionary<string, int>() { [start] = 0 };

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            var currentCost = costSoFar[current];

            foreach (var (next, nextCost) in PossibleMoves(current))
            {
                var newCost = currentCost + nextCost;
                if (costSoFar.ContainsKey(next) && newCost >= costSoFar[next]) continue;
                
                costSoFar[next] = newCost;
                frontier.Enqueue(next, newCost + Heuristic(next));
                cameFrom[next] = current;
            }
        }

        PrintSolution(start, cameFrom, costSoFar);

        return costSoFar[Goal];
    }

    private static List<(string, int)> PossibleMoves(string state)
    {
        List<(string, int)> result = new();
        foreach (var (amphipod, location) in state.ToCharArray().WithIndex())
        {
            if (amphipod == ' ') continue;
            var cost = Costs[amphipod];
            var frontier = new Queue<(int, int)>();
            frontier.Enqueue((location, 0));
            var reached = new HashSet<string> {state};
            while (frontier.Count > 0)
            {
                var (current, currentCost) = frontier.Dequeue();
                foreach (var next in Edges[current])
                {
                    if (state[next] != ' ') continue;
                    if (current < 7 && next > 6)
                    {
                        if (HomeTo[next] != amphipod) continue;
                        if (Rooms[amphipod].Any(room => state[room] != ' ' && state[room] != amphipod)) continue;
                    }

                    var newStateChars = state.ToCharArray();
                    newStateChars[location] = ' ';
                    newStateChars[next] = amphipod;
                    var newState = new string(newStateChars);

                    if (reached.Contains(newState)) continue;

                    var newCost = currentCost + cost;
                    if (DoubleCostEdge[current].Contains(next)) newCost += cost;
                    reached.Add(newState);
                    result.Add((newState, newCost));
                    frontier.Enqueue((next, newCost));
                }
            }
        }

        return result;
    }
    
    
    private static int Heuristic(string state)
    {
        var result = 0;

        foreach (var (type, locations) in Rooms)
        {
            result += locations.Where(location => state[location] != type)
                .Sum(location => Costs[type] + Costs[state[location]]);
        }
        return result;
    }

    
    private static void PrintSolution(string start, Dictionary<string, string> cameFrom, Dictionary<string, int> costSoFar)
    {
        var path = new List<string>() {Goal};
        var location = Goal;
        while (location != start)
        {
            location = cameFrom[location];
            path.Add(location);
        }

        path.Reverse();

        WriteLine("-----------------------------------");
        foreach (var s in path)
        {
            PrintState(s);
            WriteLine($"    {costSoFar[s]}  --- \"{s}\"");
        }

        WriteLine("-----------------------------------");
    }

    private static void PrintState(string state)
    {
        var chars = state.ToCharArray();
        WriteLine("\n#############");
        WriteLine($"#{chars[0]}{chars[1]} {chars[2]} {chars[3]} {chars[4]} {chars[5]}{chars[6]}#");
        WriteLine($"###{chars[7]}#{chars[8]}#{chars[9]}#{chars[10]}###");
        WriteLine($"  #{chars[11]}#{chars[12]}#{chars[13]}#{chars[14]}#");
        WriteLine("  #########");
    }
}