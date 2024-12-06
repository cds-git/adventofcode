using System.Text.RegularExpressions;

static class Day03
{
    public static void Run()
    {
        var text = File.ReadAllText(@"./data/Day03.txt");

        // var sum = text.ExtractInstructions().Select(x => x.ExtractNumbers().Multiply()).Sum();

        var sum = text.Parse().OfType<Multiply>().Evaluate();
        Console.WriteLine($"Sum: {sum}");

        var sumWithConditions = text.Parse().Evaluate();
        Console.WriteLine($"Sum with conditions: {sumWithConditions}");
    }

    private static IEnumerable<Instruction> Parse(this string text) =>
        Regex.Matches(text, @"(?<mul>mul)\((?<a>\d+),(?<b>\d+)\)|(?<do>do(n't)?)\(\)")
            .Select(match =>
                {
                    if (match.Groups["do"].Success)
                    {
                        return match.Groups["do"].Value == "do"
                            ? new Continue()
                            : new Stop() as Instruction;
                    }
                    else
                    {
                        var a = int.Parse(match.Groups["a"].Value);
                        var b = int.Parse(match.Groups["b"].Value);
                        return new Multiply(a, b);
                    }
                });

    private static int Evaluate(this IEnumerable<Instruction> instructions) => instructions.Aggregate(
        (sum: 0, include: true),
        (acc, instruction) => instruction switch
        {
            Continue => (acc.sum, true),
            Stop => (acc.sum, false),
            Multiply multiply when acc.include => (acc.sum + multiply.Product, acc.include),
            _ => acc
        }).sum;

    private static List<string> ExtractInstructions(this string text) =>
            Regex.Matches(text, @"mul\(\d+,\d+\)").Select(match => match.Value).ToList();

    private static List<int> ExtractNumbers(this string text) =>
            Regex.Matches(text, @"\b\d+\b").Select(match => int.Parse(match.Value)).ToList();

    private static int Multiply(this List<int> values) => values.Aggregate(1, (acc, next) => acc * next);
}

abstract record Instruction;

sealed record Multiply(int A, int B) : Instruction
{
    public int Product => A * B;
}

sealed record Stop : Instruction;

sealed record Continue : Instruction;
