using System.Text.RegularExpressions;

static class Day06
{
    public static void Run()
    {
        // var map = File.ReadAllText(@"./data/Day06.txt");
        var map = """
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
        """;
        Console.WriteLine(map);

        var rows = map.Split(Environment.NewLine).ToList();
        var width = rows[0].Length;
        var height = rows.Count;

        Console.WriteLine($"Width: {width} | Height: {height}");

        var initialY = rows.FindIndex(x => x.Contains('^'));
        var initialX = rows[initialY].IndexOfAny(DirectionCharacters);

        var currentPosition = new Position(new Point(initialX, initialY), new North());

        Console.WriteLine(currentPosition);
        Console.WriteLine(new string('-', 80));

        var isInsideMap = true;
        while (isInsideMap)
        {
            var nextPoint = CalculateNextPoint(currentPosition);
            Console.WriteLine("Next point: " + nextPoint);

            if (!nextPoint.IsWithinBounds(width, height))
            {
                MarkLocation(rows, currentPosition.Coordinates, 'X');
                Console.WriteLine(string.Join(Environment.NewLine, rows));
                isInsideMap = false;
                break;
            }

            var isObstruction = rows[nextPoint.Y][nextPoint.X] == '#';
            if (isObstruction)
            {
                currentPosition = currentPosition with
                {
                    Direction = currentPosition.Direction.Rotate90Degrees()
                };
            }
            else
            {
                // Mark current position with X
                MarkLocation(rows, currentPosition.Coordinates, 'X');
                currentPosition = currentPosition with { Coordinates = nextPoint };
            }

            // Mark next position with direction
            MarkLocation(rows, currentPosition.Coordinates, currentPosition.Direction.Symbol);

            Console.WriteLine(string.Join(Environment.NewLine, rows));
            Console.WriteLine(currentPosition);
            Console.WriteLine(new string('-', 80));
            Thread.Sleep(100);
        }

        Console.Write("Unique positions: ");
        Console.WriteLine(Regex.Matches(string.Join(Environment.NewLine, rows), Regex.Escape("X")).Count);
    }

    private static bool IsWithinBounds(this Point nextPoint, int width, int height)
    {
        return nextPoint.X >= 0 && nextPoint.X < width && nextPoint.Y >= 0 && nextPoint.Y < height;
    }

    private static void MarkLocation(List<string> rows, Point coords, char symbol)
    {
        var chars = rows[coords.Y].ToCharArray();
        chars[coords.X] = symbol;
        rows[coords.Y] = new string(chars);
    }

    private static Point CalculateNextPoint(Position currentPosition) => currentPosition.Direction switch
    {
        North => new Point(currentPosition.Coordinates.X, currentPosition.Coordinates.Y - 1),
        East => new Point(currentPosition.Coordinates.X + 1, currentPosition.Coordinates.Y),
        South => new Point(currentPosition.Coordinates.X, currentPosition.Coordinates.Y + 1),
        West => new Point(currentPosition.Coordinates.X - 1, currentPosition.Coordinates.Y),
        _ => throw new InvalidOperationException()
    };

    private static char[] DirectionCharacters => ['^', '>', 'v', '<'];

    private static Direction Rotate90Degrees(this Direction direction) => direction switch
    {
        North => new East(),
        East => new South(),
        South => new West(),
        West => new North(),
        _ => throw new InvalidOperationException()
    };
}

record Point(int X, int Y);

record Position(Point Coordinates, Direction Direction);

abstract record Direction(char Symbol);

record North() : Direction('^');
record East() : Direction('>');
record South() : Direction('v');
record West() : Direction('<');