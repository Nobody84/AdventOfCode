namespace AOC2024.Puzzels;

using System.Text.RegularExpressions;

public class Day3_MullItOver
{
    private const string mullRegexPattern = @"mul\s*\(\s*(?<X>[\d]{1,3})\s*,(?<Y>[\d]{1,3})\s*\)";
    private const string dontRegexPattern = @"don't\(\)(?:.|\n)*?do\(\)";
    private const string lastDontRegexPattern = @"don't\(\)(?:.|\n)*";


    public int Part1()
    {
        var count = 0;
        var input = File.ReadAllText("Inputs/Day3.txt");
        var matches = Regex.Matches(input, mullRegexPattern);

        foreach(Match match in matches)
        {
            var x = int.Parse(match.Groups["X"].Value);
            var y = int.Parse(match.Groups["Y"].Value);
            count += x * y;
        }

        return count;
    }

    public int Part2()
    {
        var count = 0;
        var input = File.ReadAllText("Inputs/Day3.txt");
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