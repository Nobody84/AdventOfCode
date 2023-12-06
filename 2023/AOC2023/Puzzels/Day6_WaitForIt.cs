namespace AOC2023.Puzzels;

using System.Text.RegularExpressions;

public class Day6_WaitForIt
{
    private record Race(long Time, long Distance);

    public long Part1()
    {
        var lines = File.ReadAllLines("Inputs/Day6.txt");
        var races = GetRaces(lines);

        return races.Select(r => this.GetPossibleButtonPressTimes(r).Count).Aggregate((a1, a2) => a1 * a2);
    }

    public long Part2()
    {
        var lines = File.ReadAllLines("Inputs/Day6.txt");
        var race = GetRace(lines);

        return this.GetPossibleButtonPressTimes(race).Count;
    }

    private IEnumerable<Race> GetRaces(string[] lines)
    {
        var numberRegex = new Regex(@"\b(\d+)\b");
        var times = numberRegex.Matches(lines[0]).Select(m => int.Parse(m.Value));
        var distaces = numberRegex.Matches(lines[1]).Select(m => int.Parse(m.Value));

        foreach (var (time, distance) in times.Zip(distaces))
        {
            yield return new Race(time, distance);
        }
    }
    private Race GetRace(string[] lines)
    {
        var numberRegex = new Regex(@"(\d+)");
        var time = int.Parse(numberRegex.Match(lines[0].Replace(" ", null)).Value);
        var distance = long.Parse(numberRegex.Match(lines[1].Replace(" ", null)).Value);

        return new Race(time, distance);
    }

    private List<int> GetPossibleButtonPressTimes(Race race)
    {
        var possibleButtonPressTimes = new List<int>();
        for (int i = 0; i < race.Time; i++)
        {
            var value = i * (race.Time - i);
            if (value <= race.Distance && possibleButtonPressTimes.Any())
            {
                break;
            }

            if (value > race.Distance)
            {
                possibleButtonPressTimes.Add(i);
            }
        }

        return possibleButtonPressTimes;
    }
}