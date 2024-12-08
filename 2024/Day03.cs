using System.Text.RegularExpressions;

static class Day03
{
    public static void Run()
    {
        var text = File.ReadAllText(@"./data/Day03.txt");

        var sum = text.Parse().OfType<Multiply>().Evaluate();
        Console.WriteLine($"Sum: {sum}");

        var sumWithConditions = text.Parse().Evaluate();
        Console.WriteLine($"Sum with conditions: {sumWithConditions}");
    }

    private static IEnumerable<Instruction> Parse(this string text) =>
        Regex.Matches(text, @"(?<mul>mul)\((?<a>\d+),(?<b>\d+)\)|(?<do>do(n't)?)\(\)")
            .Select(match => match switch
            {
                _ when match.Groups["do"].Success && match.Groups["do"].Value == "don't" => new Pause() as Instruction,
                _ when match.Groups["do"].Success && match.Groups["do"].Value == "do" => new Resume(),
                _ => new Multiply(int.Parse(match.Groups["a"].Value), int.Parse(match.Groups["b"].Value))
            });

    private static int Evaluate(this IEnumerable<Instruction> instructions) => instructions.Aggregate(
        (sum: 0, include: true),
        (acc, instruction) => instruction switch
        {
            Resume => (acc.sum, true),
            Pause => (acc.sum, false),
            Multiply multiply when acc.include => (acc.sum + multiply.Product, acc.include),
            _ => acc
        }).sum;
}

abstract record Instruction;

sealed record Multiply(int A, int B) : Instruction
{
    public int Product => A * B;
}

sealed record Pause : Instruction;

sealed record Resume : Instruction;