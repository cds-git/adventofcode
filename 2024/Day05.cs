static class Day05
{
    public static void Run()
    {
        var lines = File.ReadAllLines(@"./data/Day05.txt");

        var sortOrders = lines.TakeWhile(line => !string.IsNullOrWhiteSpace(line))
            .Select(ParseSortOrder).GroupBy(x => x.before, x => x.after)
            .ToDictionary(x => x.Key, x => x.ToList());

        var updates = lines.SkipWhile(line => !string.IsNullOrWhiteSpace(line)).Skip(1)
            .Select(Common.ExtractAllIntegersFromString);

        var comparer = Comparer<int>.Create((a, b) =>
        {
            if (sortOrders[a].Contains(b)) return -1;
            else if (sortOrders[b].Contains(a)) return 1;
            else return 0;
        });

        var sum = updates.Where(x => x.IsSorted(comparer)).Sum(GetMiddlePage);

        Console.WriteLine($"Sum of middle pages: {sum}");

        var sortedCorruptedUpdatesSum = updates.Where(x => !x.IsSorted(comparer))
            .Sum(pages => pages.Order(comparer).ToList().GetMiddlePage());

        Console.WriteLine($"Sum of sorted corrupted middle pages: {sortedCorruptedUpdatesSum}");
    }

    private static int GetMiddlePage(this List<int> pages) =>
        pages[pages.Count / 2];

    private static bool IsSorted(this List<int> pages, IComparer<int> comparer)
    {
        var sortedList = new List<int>(pages);
        sortedList.Sort(comparer);
        return pages.SequenceEqual(sortedList);
    }

    private static (int before, int after) ParseSortOrder(this string line)
    {
        var pair = line.Split("|");
        return (int.Parse(pair[0]), int.Parse(pair[1]));
    }
}