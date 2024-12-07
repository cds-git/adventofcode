using System.Text.RegularExpressions;

static class Common
{
    public static List<int> ExtractAllIntegersFromString(this string line) =>
        Regex.Matches(line, @"\d+").Select(match => int.Parse(match.Value)).ToList();
}