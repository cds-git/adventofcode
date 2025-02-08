static class Day08
{
    public static void Run()
    {
        // var input = File.ReadAllLines(@"./data/Day08.txt");
        var input = """
        ............
        ........0...
        .....0......
        .......0....
        ....0.......
        ......A.....
        ............
        ............
        ........A...
        .........A..
        ............
        ............
        """.Split(Environment.NewLine);

        var map = input.ParseMap();

        var antennas = map.GetGroupedAntennas().ToList();

        var antinodesCount = antennas
            .SelectMany(a => a.GetAntinodes(map, GetNonResonatingAntinodes))
            .Distinct()
            .Count();
        Console.WriteLine($"Antinodes: {antinodesCount}");

        var resonatingAntinodesCount = antennas
           .SelectMany(a => a.GetAntinodes(map, GetResonatingAntinodes))
           .Distinct()
           .Count();
        Console.WriteLine($"Resonating Antinodes: {resonatingAntinodesCount}");
    }

    private static IEnumerable<Position> GetAntinodes(this (char frequency, List<Position> positions) antennaSet, char[][] map, Func<char[][], Position, Position, List<Position>> getAntinodes) =>
        antennaSet.GetPairs().SelectMany(pair => getAntinodes(map, pair.a, pair.b));

    private static List<Position> GetNonResonatingAntinodes(this char[][] map, Position a, Position b)
    {
        var xDiff = a.X - b.X;
        var yDiff = a.Y - b.Y;

        var antinodes = new List<Position>();

        var antinode1 = new Position(a.X + xDiff, a.Y + yDiff);
        if (map.IsWithinBounds(antinode1)) antinodes.Add(antinode1);

        var antinode2 = new Position(b.X - xDiff, b.Y - yDiff);
        if (map.IsWithinBounds(antinode2)) antinodes.Add(antinode2);

        return antinodes;
    }

    private static List<Position> GetResonatingAntinodes(this char[][] map, Position a, Position b)
    {
        var antinodes = new List<Position>();

        var xDiff = a.X - b.X;
        var yDiff = a.Y - b.Y;

        var pos1 = a;
        while (map.IsWithinBounds(pos1))
        {
            antinodes.Add(pos1);
            pos1 = new Position(pos1.X + xDiff, pos1.Y + yDiff);
        }

        var pos2 = b;
        while (map.IsWithinBounds(pos2))
        {
            antinodes.Add(pos2);
            pos2 = new Position(pos2.X - xDiff, pos2.Y - yDiff);
        }

        return antinodes;
    }

    private static IEnumerable<(Position a, Position b)> GetPairs(this (char frequency, List<Position> positions) antennaSet) =>
        antennaSet.positions.SelectMany((a, index) =>
            antennaSet.positions.Skip(index + 1).Select(b => (a, b)));

    private static IEnumerable<(char frequency, List<Position> positions)> GetGroupedAntennas(this char[][] map) =>
        map.GetAntennas().ToList().GroupBy(
            a => a.Frequency,
            (frequency, antennas) => (frequency, antennas.Select(a => a.Position).ToList()));

    private static IEnumerable<Antenna> GetAntennas(this char[][] map) =>
        map.SelectMany((row, x) =>
            row.Select((freq, y) => (freq, x, y))
                .Where(pos => pos.freq != '.' && !char.IsWhiteSpace(pos.freq))
                .Select(pos => new Antenna(pos.freq, new Position(pos.x, pos.y))));

    private static bool IsWithinBounds(this char[][] map, Position position) =>
        position.X >= 0 && position.X < map.Length &&
        position.Y >= 0 && position.Y < map.Length;

    private static char[][] ParseMap(this string[] rows) =>
        rows.Select(row => row.ToCharArray()).ToArray();

    record Position(int X, int Y);
    record Antenna(char Frequency, Position Position);
}