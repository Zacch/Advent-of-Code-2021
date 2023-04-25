using System.Diagnostics.CodeAnalysis;
using System.Text;
using Extensions;

namespace Advent_of_Code_2021;

public static class Day23Part2
{
    /*
        #################
        #01  2  3  4  56#
        ### 7# 8# 9#10###
          #11#12#13#14#
          #15#16#17#18#
          #19#20#21#22#
          #############
    */

    private static readonly Dictionary<char, List<int>> DistanceHome = new()
    {
        ['A'] = new List<int> {3, 2, 2, 4, 6, 8, 9, 0, 4, 6, 8, 0, 5, 7, 9, 0, 6, 8, 10, 0, 7, 9, 11},
        ['B'] = new List<int> {5, 4, 2, 2, 4, 6, 7, 4, 0, 4, 6, 5, 0, 5, 7, 6, 0, 8, 8, 7, 0, 7, 9},
        ['C'] = new List<int> {7, 6, 4, 2, 2, 4, 5, 6, 4, 0, 4, 7, 5, 0, 5, 8, 6, 0, 6, 9, 7, 0, 7},
        ['D'] = new List<int> {9, 8, 6, 4, 2, 2, 3, 8, 6, 4, 0, 9, 7, 5, 0, 10, 8, 6, 0, 11, 9, 7, 0},
    };

    
    private static readonly Dictionary<int, List<int>> Edges = new()
    {
        [0] = new List<int> {1},
        [1] = new List<int> {0, 2, 7},
        [2] = new List<int> {1, 3, 7, 8},
        [3] = new List<int> {2, 4, 8, 9},
        [4] = new List<int> {3, 5, 9, 10},
        [5] = new List<int> {4, 6, 10},
        [6] = new List<int> {5},
        [7] = new List<int> {1, 2, 11},
        [8] = new List<int> {2, 3, 12},
        [9] = new List<int> {3, 4, 13},
        [10] = new List<int> {4, 5, 14},
        [11] = new List<int> {7, 15},
        [12] = new List<int> {8, 16},
        [13] = new List<int> {9, 17},
        [14] = new List<int> {10, 18},
        [15] = new List<int> {11, 19},
        [16] = new List<int> {12, 20},
        [17] = new List<int> {13, 21},
        [18] = new List<int> {14, 22},
        [19] = new List<int> {15},
        [20] = new List<int> {16},
        [21] = new List<int> {17},
        [22] = new List<int> {18}
    };

    private static readonly Dictionary<int, List<int>> DoubleCostEdge = new()
    {
        [0] = new List<int>(),
        [1] = new List<int> {2, 7},
        [2] = new List<int> {1, 3, 7, 8},
        [3] = new List<int> {2, 4, 8, 9},
        [4] = new List<int> {3, 5, 9, 10},
        [5] = new List<int> {4, 10},
        [6] = new List<int>(),
        [7] = new List<int> {1, 2},
        [8] = new List<int> {2, 3},
        [9] = new List<int> {3, 4},
        [10] = new List<int> {4, 5},
        [11] = new List<int>(),
        [12] = new List<int>(),
        [13] = new List<int>(),
        [14] = new List<int>(),
        [15] = new List<int>(),
        [16] = new List<int>(),
        [17] = new List<int>(),
        [18] = new List<int>(),
        [19] = new List<int>(),
        [20] = new List<int>(),
        [21] = new List<int>(),
        [22] = new List<int>()
    };

    private static readonly Dictionary<char, List<int>> Rooms = new()
    {
        ['A'] = new List<int> {7, 11, 15, 19},
        ['B'] = new List<int> {8, 12, 16, 20},
        ['C'] = new List<int> {9, 13, 17, 21},
        ['D'] = new List<int> {10, 14, 18, 22}
    };

    private static readonly Dictionary<int, char> HomeTo = new()
    {
        [0] = ' ', [1] = ' ', [2] = ' ', [3] = ' ', [4] = ' ', [5] = ' ', [6] = ' ', 
        [7] = 'A', [8] = 'B', [9] = 'C', [10] = 'D', [11] = 'A', [12] = 'B', [13] = 'C', [14] = 'D',
        [15] = 'A', [16] = 'B', [17] = 'C', [18] = 'D', [19] = 'A', [20] = 'B', [21] = 'C', [22] = 'D',
    };
    
    private static readonly Dictionary<char, int> Costs = new() { [' '] = 0, ['A'] = 1, ['B'] = 10, ['C'] = 100, ['D'] = 1000 };

    private const string Goal = "       ABCDABCDABCDABCD";

    public static void Run()
    {
        var lines = File.ReadLines("Day23.txt").ToArray();

        var start = new StringBuilder("       ");
        start.Append(lines[2][3]).Append(lines[2][5]).Append(lines[2][7]).Append(lines[2][9]);
        // ReSharper disable once StringLiteralTypo
        start.Append("DCBADBAC");
        start.Append(lines[3][3]).Append(lines[3][5]).Append(lines[3][7]).Append(lines[3][9]);
        
        Console.WriteLine("Please wait – this takes about 5 minutes on an M1 Mac");
        Console.WriteLine($"Part 2: {Part2(start.ToString())}");
    }
    
    private static int Part2(string start)
    {
        var frontier = new PriorityQueue<string, int>();
        frontier.Enqueue(start, 0);
        var cameFrom = new Dictionary<string, string>() { [start] = "" };
        var costSoFar = new Dictionary<string, int>() { [start] = 0 };

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();
            var currentCost = costSoFar[current];

            if (current == Goal) break;
            foreach (var (next, nextCost) in PossibleMoves(current))
            {
                var newCost = currentCost + nextCost;
                if (costSoFar.ContainsKey(next) && newCost >= costSoFar[next]) continue;
                
                costSoFar[next] = newCost;
                frontier.Enqueue(next, newCost + Heuristic(next));
                cameFrom[next] = current;
            }
        }

        // PrintSolution(start, cameFrom, costSoFar);

        Console.WriteLine($"\nVisited states: {cameFrom.Count}\n");
        return costSoFar[Goal];
    }

    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
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

                    if (location > 6 || next > 6) 
                        result.Add((newState, newCost));
                    frontier.Enqueue((next, newCost));
                }
            }
        }

        return result;
    }
    
    

    [SuppressMessage("ReSharper.DPA", "DPA0001: Memory allocation issues")]
    private static int Heuristic(string state)
    {
        var result = 0;

        foreach (var (type, locations) in Rooms)
        {
            foreach (var (location, depth) in locations.WithIndex().Where(l => state[l.item] != type))
            {
                result += (depth + 2) * Costs[state[location]] + depth * Costs[type];
            }
        }

        foreach (var (type, location) in state.ToCharArray().WithIndex().Where(l => l.item != ' '))
        {
            result += Costs[type] * DistanceHome[type][location];
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

        Console.WriteLine("-----------------------------------");
        foreach (var s in path)
        {
            PrintState(s);
            Console.WriteLine($"    {costSoFar[s]}  --- \"{s}\"");
        }

        Console.WriteLine("-----------------------------------");
    }

    private static void PrintState(string state)
    {
        var chars = state.ToCharArray();
        Console.WriteLine("\n#############");
        Console.WriteLine($"#{chars[0]}{chars[1]} {chars[2]} {chars[3]} {chars[4]} {chars[5]}{chars[6]}#");
        Console.WriteLine($"###{chars[7]}#{chars[8]}#{chars[9]}#{chars[10]}###");
        Console.WriteLine($"  #{chars[11]}#{chars[12]}#{chars[13]}#{chars[14]}#");
        Console.WriteLine($"  #{chars[15]}#{chars[16]}#{chars[17]}#{chars[18]}#");
        Console.WriteLine($"  #{chars[19]}#{chars[20]}#{chars[21]}#{chars[22]}#");
        Console.WriteLine("  #########");
    }
}