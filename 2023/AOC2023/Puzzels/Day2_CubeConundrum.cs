using System.Text.RegularExpressions;

namespace AOC2023.Puzzels;

public class Day2_CubeConundrum
{
    private record Set(int Red, int Green, int Blue);
    private record Game(int Id, List<Set> Sets);

    public int Part1()
    {
        var possibleRed = 12;
        var possibleGreen = 13;
        var possibleBlue = 14;

        var lines = File.ReadLines("Inputs/Day2.txt");
        var games = GetGames(lines);

        var possibleGames = new List<int>();
        foreach (var game in games)
        {
            var impossible = false;
            foreach (var set in game.Sets)
            {
                if (set.Red > possibleRed || set.Green > possibleGreen || set.Blue > possibleBlue)
                {
                    impossible = true;
                }
            }

            if (!impossible)
            {
                possibleGames.Add(game.Id);
            }
        }

        return possibleGames.Sum();
    }

    public int Part2()
    {
        var lines = File.ReadLines("Inputs/Day2.txt");
        var games = GetGames(lines);

        var gamePowers = new List<int>();
        foreach (var game in games)
        {
            var minRed = 0;
            var minGreen = 0;
            var minBlue = 0;
            foreach (var set in game.Sets)
            {
                minRed = Math.Max(minRed, set.Red);
                minGreen = Math.Max(minGreen, set.Green);
                minBlue = Math.Max(minBlue, set.Blue);
            }

            gamePowers.Add(minRed * minGreen * minBlue);
        }

        return gamePowers.Sum();
    }

    private List<Game> GetGames(IEnumerable<string> lines)
    {
        var gameIdRegex = new Regex(@"^Game\ (?<id>\d+):\ (?<sets>.*)$");
        var colorRegex = new Regex(@"^(?<number>\d+)\ (?<color>.*)$");

        var games = new List<Game>();
        foreach (var line in lines)
        {
            // Game id
            var match = gameIdRegex.Match(line);
            var gameId = int.Parse(match.Groups["id"].Value);
            var setsString = match.Groups["sets"].Value;

            var sets = new List<Set>();
            var reds = 0;
            var greens = 0;
            var blues = 0;
            foreach (var setString in setsString.Split(';'))
            {
                foreach (var colorString in setString.Split(','))
                {
                    var colorMatch = colorRegex.Match(colorString.Trim());
                    switch (colorMatch.Groups["color"].Value)
                    {
                        case "red":
                            reds = int.Parse(colorMatch.Groups["number"].Value);
                            break;
                        case "green":
                            greens = int.Parse(colorMatch.Groups["number"].Value);
                            break;
                        case "blue":
                            blues = int.Parse(colorMatch.Groups["number"].Value);
                            break;
                    }
                }

                sets.Add(new Set(reds, greens, blues));
            }

            games.Add(new Game(gameId, sets));
        }

        return games;
    }
}

