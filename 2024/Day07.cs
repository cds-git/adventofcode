static class Day07
{
    public static void Run()
    {
        // var input = File.ReadAllLines(@"./data/Day07.txt");
        var input = """
        190: 10 19
        3267: 81 40 27
        83: 17 5
        156: 15 6
        7290: 6 8 6 15
        161011: 16 10 13
        192: 17 8 14
        21037: 9 7 18 13
        292: 11 6 16 20
        """.Split(Environment.NewLine);

        var equations = input.ParseEquations();

        var totalCalibrationResult = equations
            .Where(e => e.HasValidResult([Add, Multiply]))
            .Sum(e => e.result);
        Console.WriteLine(totalCalibrationResult);


        var totalConcatenatedCalibrationResult = equations
            .Where(e => e.HasValidResult([Add, Multiply, Concatenate]))
            .Sum(e => e.result);
        Console.WriteLine(totalConcatenatedCalibrationResult);
    }

    private static bool HasValidResult(this (long result, long firstValue, List<long> other) equation, List<Func<long, long, long>> operations)
    {
        if (equation.other.Count == 0) return equation.result == equation.firstValue;

        var evaluatedExpressions = GenerateExpression(equation.other, 0, equation.firstValue, operations);

        return evaluatedExpressions.Contains(equation.result);
    }

    private static List<long> GenerateExpression(List<long> numbers, int index, long currentValue, List<Func<long, long, long>> operations)
    {
        if (index == numbers.Count) return [currentValue];

        var results = new List<long>();

        operations.ForEach(
            operation => results.AddRange(
                GenerateExpression(numbers, index + 1, operation(currentValue, numbers[index]), operations))
        );

        return results;
    }

    private static long Add(long a, long b) => a + b;
    private static long Multiply(long a, long b) => a * b;
    private static long Concatenate(long a, long b) => long.Parse($"{a}{b}");

    private static IEnumerable<(long result, long firstValue, List<long> other)> ParseEquations(this string[] input) => input
        .Select(Common.ExtractAllLongsFromString)
        .Select(numbers => (numbers[0], numbers[1], numbers[2..]));
}