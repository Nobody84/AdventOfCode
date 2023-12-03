using AOC2023.Puzzels.Day3;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AOC2023.Puzzels;

public class Day3_GearRatios
{
    public int Part1()
    {
        var numberRegex = new Regex(@"(\d+)");
        var symbolRegex = new Regex(@"([^\d\.]{1})");

        var lines = File.ReadAllLines("Inputs/Day3_Part1.txt");
        var numbers = new List<Number>();
        var symbols = new List<Point>();

        var y = 0;
        foreach (var line in lines)
        {
            var numberMatches = numberRegex.Matches(line);
            foreach (Match match in numberMatches)
            {
                numbers.Add(new Number { X = match.Index, Y = y, Value = match.Value });
            }

            var symbolMatches = symbolRegex.Matches(line);
            foreach (Match match in symbolMatches)
            {
                symbols.Add(new Point(match.Index, y));
            }

            y++;
        }

        var partNumbers = numbers.Where(n => symbols.Any(s => n.IsAdjacent(s))).ToList();

        return partNumbers.Sum(p => int.Parse(p.Value));
    }

    public int Part2()
    {

        return 0;
    }
}

