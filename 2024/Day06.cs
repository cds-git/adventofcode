static class Day06
{
    public static void Run()
    {
        // var input = File.ReadAllLines(@"./data/Day06.txt");
        var input = """
        ....#.....
        .........#
        ..........
        ..#.......
        .......#..
        ..........
        .#..^.....
        ........#.
        #.........
        ......#...
        """.Split(Environment.NewLine);

        var map = input.Select(row => row.ToCharArray()).ToArray();
        var height = map.Length;
        var width = map[0].Length;
        var currentPosition = map.GetStartingPosition();
        var uniquePositions = new HashSet<Point>();

        map.Print();
        Console.WriteLine(currentPosition);
        Console.WriteLine(new string('-', 80));

        while (map.IsWithinBounds(currentPosition.Coordinates))
        {
            var nextPosition = currentPosition.CalculateNextPosition();
            Console.WriteLine($"Next: {nextPosition}");
            if (nextPosition.IsObstructed(map))
            {
                currentPosition = currentPosition with
                {
                    Direction = currentPosition.Direction.Rotate90Degrees()
                };
            }
            else
            {
                map.MarkLocation(currentPosition.Coordinates, 'X');
                uniquePositions.Add(currentPosition.Coordinates);
                currentPosition = currentPosition with { Coordinates = nextPosition };
            }

            if (map.IsWithinBounds(nextPosition))
                map.MarkLocation(currentPosition.Coordinates, currentPosition.Direction.Symbol);

            map.Print();
            Console.WriteLine($"Current: {currentPosition}");
            Console.WriteLine(new string('-', 80));
            Thread.Sleep(100);
        }

        Console.Write("Unique positions: ");
        Console.WriteLine(uniquePositions.Count);
    }

    private static bool IsObstructed(this Point coords, char[][] map) =>
        map.IsWithinBounds(coords) && map[coords.X][coords.Y] == '#';

    private static Position GetStartingPosition(this char[][] map) =>
        map.SelectMany((row, rowIndex) => row.Select((cell, colIndex) =>
                new Position(new Point(rowIndex, colIndex), GetDirection(cell))))
            .First(position => position.Direction is not Invalid);

    private static bool IsWithinBounds(this char[][] map, Point coords) =>
        coords.X >= 0 && coords.X < map.Length && coords.Y >= 0 && coords.Y < map[0].Length;

    private static void MarkLocation(this char[][] map, Point coords, char symbol) =>
        map[coords.X][coords.Y] = symbol;

    private static Point CalculateNextPosition(this Position currentPosition) => currentPosition.Direction switch
    {
        North => new Point(currentPosition.Coordinates.X - 1, currentPosition.Coordinates.Y),
        East => new Point(currentPosition.Coordinates.X, currentPosition.Coordinates.Y + 1),
        South => new Point(currentPosition.Coordinates.X + 1, currentPosition.Coordinates.Y),
        West => new Point(currentPosition.Coordinates.X, currentPosition.Coordinates.Y - 1),
        _ => throw new InvalidOperationException()
    };

    private static Direction Rotate90Degrees(this Direction direction) => direction switch
    {
        North => new East(),
        East => new South(),
        South => new West(),
        West => new North(),
        _ => throw new InvalidOperationException()
    };

    private static Direction GetDirection(char symbol) => symbol switch
    {
        '^' => new North(),
        '>' => new East(),
        'v' => new South(),
        '<' => new West(),
        _ => new Invalid()
    };

    private static void Print(this char[][] map) =>
        Console.WriteLine(string.Join(Environment.NewLine, map.Select(row => new string(row))));
}

record Point(int X, int Y);

record Position(Point Coordinates, Direction Direction);

abstract record Direction(char Symbol);

record North() : Direction('^');
record East() : Direction('>');
record South() : Direction('v');
record West() : Direction('<');

record Invalid() : Direction('~');