using static System.Console;

namespace Advent_of_Code_2021;

public class Day21
{
    private int rolls;
    private int score1;
    private int score2;
    private int die = 100;

    public static void Run()
    {
        var lines = File.ReadLines("Day21.txt").ToArray();

        var player1 = (int) char.GetNumericValue(lines[0].Last());
        var player2 = (int) char.GetNumericValue(lines[1].Last());

        new Day21().Part1(player1, player2);
        Part2(player1, player2);
    }

    void Part1(int player1, int player2)
    {
        while (true)
        {
            player1 = player1 + RollDie() + RollDie() + RollDie();
            while (player1 > 10) player1 -= 10;
            score1 += player1;
            if (score1 >= 1000)
            {
                WriteLine($"Part 1: {score2 * rolls}");
                return;
            }
            
            player2 = player2 + RollDie() + RollDie() + RollDie();
            while (player2 > 10) player2 -= 10;
            score2 += player2;
            if (score2 >= 1000)
            {
                WriteLine($"Part 1: {score1 * rolls}");
                return;
            }
        }
    }

    private int RollDie()
    {
        die++;
        if (die == 101) die = 1;
        rolls++;
        return die;
    }

    private static readonly Dictionary<int, long> ScoreToMultiplier = new()
    {
        [3] = 1,
        [4] = 3,
        [5] = 6,
        [6] = 7,
        [7] = 6,
        [8] = 3,
        [9] = 1
    };

    private class State : IEquatable<State>
    {
        public readonly int Position1;
        public readonly int Position2;
        public readonly int Score1;
        public readonly int Score2;

        public State(int position1, int position2, int score1, int score2)
        { Position1 = position1; Position2 = position2; Score1 = score1; Score2 = score2; }

        public bool Equals(State? other) => other != null && Position1 == other.Position1 && Position2 == other.Position2 &&
                                            Score1 == other.Score1 && Score2 == other.Score2;

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((State) obj);
        }

        public override int GetHashCode() => HashCode.Combine(Position1, Position2, Score1, Score2);
    }
    
    private static void Part2(int player1, int player2)
    {
        var universesP1 = new Dictionary<State, long>();
        var universesP2 = new Dictionary<State, long>();

        universesP1[new State(player1, player2, 0, 0)] = 1;
        var totalWinsP1 = 0L;
        var totalWinsP2 = 0L;
        while (universesP1.Count > 0)
        {
            foreach (var (state, count) in universesP1)
            {
                foreach (var (die, multiplier) in ScoreToMultiplier)
                {
                    var newCount = count * multiplier;
                    var newPosition = state.Position1 + die;
                    if (newPosition > 10) newPosition -= 10;
                    var newState = new State(newPosition, state.Position2, state.Score1 + newPosition, state.Score2);
                    if (newState.Score1 >= 21)
                    {
                        totalWinsP1 += newCount;
                    }
                    else
                    {
                        if (!universesP2.ContainsKey(newState)) universesP2[newState] = 0;
                        universesP2[newState] += newCount;
                    }
                }
            }
            universesP1.Clear();
            
            foreach (var (state, count) in universesP2)
            {
                foreach (var (die, multiplier) in ScoreToMultiplier)
                {
                    var newCount = count * multiplier;
                    var newPosition = state.Position2 + die;
                    if (newPosition > 10) newPosition -= 10;
                    var newState = new State(state.Position1, newPosition, state.Score1, state.Score2 + newPosition);
                    if (newState.Score2 >= 21)
                    {
                        totalWinsP2 += newCount;
                    }
                    else
                    {
                        if (!universesP1.ContainsKey(newState)) universesP1[newState] = 0;
                        universesP1[newState] += newCount;
                    }
                }
            }
            universesP2.Clear();
        }
        
        WriteLine($"Part 2: {Math.Max(totalWinsP1, totalWinsP2) }");
    }
}