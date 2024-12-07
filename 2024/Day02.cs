static class Day02
{
    public static void Run()
    {
        var lines = File.ReadAllLines(@"./data/Day02.txt");

        var matrix = lines.Select(Common.ExtractAllIntegersFromString).ToList();
        // var matrix = new List<List<int>>()
        // {
        //     new(){7, 6, 4, 2, 1},
        //     new(){1, 2, 7, 8, 9},
        //     new(){9, 7, 6, 2, 1},
        //     new(){1, 3, 2, 4, 5},
        //     new(){8, 6, 4, 4, 1},
        //     new(){1, 3, 6, 7, 9},
        // };


        var safeCount = matrix.Count(IsSafe);
        Console.WriteLine($"Amount of safe reports: {safeCount}");

        var tolerantSafeCount = matrix.Count(x => x.Expand().Any(IsSafe));
        Console.WriteLine($"Amount of safe reports with tolerance: {tolerantSafeCount}");
    }

    private static bool IsSafe(this List<int> values) =>
        values.Count < 2 || values.IsSafe(Math.Sign(values[1] - values[0]));

    private static bool IsSafe(this List<int> values, int diffSign) =>
        values.ToPairs().All(pair => pair.IsSafe(diffSign));

    private static IEnumerable<(int prev, int next)> ToPairs(this IEnumerable<int> values)
    {
        using var enumerator = values.GetEnumerator();
        if (!enumerator.MoveNext()) yield break;

        int prev = enumerator.Current;
        while (enumerator.MoveNext())
        {
            yield return (prev, enumerator.Current);
            prev = enumerator.Current;
        }
    }

    private static bool IsSafe(this (int prev, int next) pair, int diffSign) =>
        Math.Abs(pair.next - pair.prev) >= 1 &&
        Math.Abs(pair.next - pair.prev) <= 3 &&
        Math.Sign(pair.next - pair.prev) == diffSign;

    private static IEnumerable<List<int>> Expand(this List<int> values) =>
        new[] { values }.Concat(Enumerable.Range(0, values.Count).Select(values.ExceptAt));

    private static List<int> ExceptAt(this List<int> values, int index) =>
        values.Take(index).Concat(values.Skip(index + 1)).ToList();
}
