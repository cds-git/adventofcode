static class Day01
{
    public static void Run()
    {
        var lines = File.ReadAllLines(@"./data/Day01.txt");

        var (left, right) = lines.Select(Common.ExtractAllIntegersFromString).Transpose().ToPair();

        var dist = left.Order()
            .Zip(right.Order(), (x, y) => Math.Abs(x - y))
            .Sum();

        var similarity = right.Where(new HashSet<int>(left).Contains).Sum();

        Console.WriteLine($"Total items: {left.Count}");
        Console.WriteLine($"Distance: {dist}");
        Console.WriteLine($"Similarity: {similarity}");
    }

    private static List<List<T>> Transpose<T>(this IEnumerable<IEnumerable<T>> values) =>
         values.Aggregate(
             new List<List<T>>(),
             (acc, row) =>
             {
                 var i = 0;
                 foreach (var cell in row)
                 {
                     if (acc.Count <= i) acc.Add([]);
                     acc[i++].Add(cell);
                 }
                 return acc;
             }
         );

    private static (T a, T b) ToPair<T>(this List<T> list) => list switch
    {
        [T a, T b] => (a, b),
        _ => throw new ArgumentException("Not a pair")
    };
}