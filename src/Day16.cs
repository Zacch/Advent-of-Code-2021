using System.Text;

namespace Advent_of_Code_2021;

public static class Day16
{
    
    private static readonly Dictionary<char, string> Hex = new ()
    {
        ['0'] = "0000", ['1'] = "0001", ['2'] = "0010", ['3'] = "0011",
        ['4'] = "0100", ['5'] = "0101", ['6'] = "0110", ['7'] = "0111", 
        ['8'] = "1000", ['9'] = "1001", ['A'] = "1010", ['B'] = "1011",
        ['C'] = "1100", ['D'] = "1101", ['E'] = "1110", ['F'] = "1111"
    };

    private class Packet
    {
        private int version;
        private int type;
        private long literalValue;
        private readonly List<Packet> subPackets = new();
        
        public int VersionSum => version + subPackets.Select(p => p.VersionSum).Sum();

        public long Evaluate() =>
            type switch
            {
                0 => subPackets.Select(p => p.Evaluate()).Sum(),
                1 => subPackets.Aggregate(1L, (product, package) => product * package.Evaluate()),
                2 => subPackets.Select(p => p.Evaluate()).Min(),
                3 => subPackets.Select(p => p.Evaluate()).Max(),
                4 => literalValue,
                5 => subPackets[0].Evaluate() > subPackets[1].Evaluate() ? 1 : 0,
                6 => subPackets[0].Evaluate() < subPackets[1].Evaluate() ? 1 : 0,
                7 => subPackets[0].Evaluate() == subPackets[1].Evaluate() ? 1 : 0,
                _ => throw new InvalidDataException()
            };

        public int Parse(string bits, int startIndex)
        {
            var index = startIndex;
            version = Convert.ToInt32(bits[index..(index + 3)], 2);
            index += 3;
            type = Convert.ToInt32(bits[index..(index + 3)], 2);
            index += 3;

            if (type == 4)
            {
                var done = false;
                var binaryValue = new StringBuilder();
                while (!done)
                {
                    done = bits[index++] != '1';
                    binaryValue.Append(bits[index..(index + 4)]);
                    index += 4;
                }

                literalValue = Convert.ToInt64(binaryValue.ToString(), 2);
            }
            else
            {
                var lengthTypeId = bits[index++];
                if (lengthTypeId == '0')
                {
                    var subPacketLength = Convert.ToInt32(bits[index..(index + 15)], 2);
                    index += 15;
                    var subPacketEnd = index + subPacketLength;
                    while (index < subPacketEnd) 
                        index = ParseSubPacket(bits, index);
                    
                }
                else
                {
                    var subPacketCount = Convert.ToInt32(bits[index..(index + 11)], 2);
                    index += 11;
                    for (var i = 0; i < subPacketCount; i++) 
                        index = ParseSubPacket(bits, index);
                }
            }

            return index;
        }

        private int ParseSubPacket(string bits, int index)
        {
            var subPacket = new Packet();
            index = subPacket.Parse(bits, index);
            subPackets.Add(subPacket);
            return index;
        }
    }

    public static void Run()
    {
        var lines = File.ReadLines("Day16.txt").ToList();
        var bits = new StringBuilder();
        foreach (var c in lines[0].ToCharArray())
        {
            bits.Append(Hex[c]);
        }

        var packet = new Packet();
        packet.Parse(bits.ToString(), 0);
        
        Console.WriteLine($"Part 1: {packet.VersionSum}");
        Console.WriteLine($"Part 2: {packet.Evaluate()}");
    }
}