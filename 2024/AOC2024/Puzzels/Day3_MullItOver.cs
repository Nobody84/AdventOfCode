namespace AOC2024.Puzzels;

using System.Text.RegularExpressions;

public class Day3_MullItOver : PuzzelBase
{
    private const string mullRegexPattern = @"mul\s*\(\s*(?<X>[\d]{1,3})\s*,(?<Y>[\d]{1,3})\s*\)";
    private const string dontRegexPattern = @"don't\(\)(?:.|\n)*?do\(\)";
    private const string lastDontRegexPattern = @"don't\(\)(?:.|\n)*";
    private string input = string.Empty;

    public Day3_MullItOver()
        : base(3, "Mull It Over")
    {
    }

    protected override void PreparePart1(string inputPath)
    {
        this.input = File.ReadAllText(inputPath);
    }

    protected override object Part1()
    {
        var count = 0;
        var matches = Regex.Matches(input, mullRegexPattern);

        foreach(Match match in matches)
        {
            var x = int.Parse(match.Groups["X"].Value);
            var y = int.Parse(match.Groups["Y"].Value);
            count += x * y;
        }

        return count;
    }

    protected override object Part2()
    {
        var count = 0;

        var cleandInput = Regex.Replace(input, dontRegexPattern, string.Empty);
        cleandInput = Regex.Replace(cleandInput, lastDontRegexPattern, string.Empty);
        var matches = Regex.Matches(cleandInput, mullRegexPattern);

        foreach (Match match in matches)
        {
            var x = int.Parse(match.Groups["X"].Value);
            var y = int.Parse(match.Groups["Y"].Value);
            count += x * y;
        }

        return count;
    }
}