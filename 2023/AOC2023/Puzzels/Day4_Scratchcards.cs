using AOC2023.Puzzels.Day3;
using AOC2023.Puzzels.Day4;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AOC2023.Puzzels;

public class Day4_Scratchcards
{
    public int Part1()
    {
        var lines = File.ReadLines("Inputs/Day4_Part1.txt");
        var cards = GetCards(lines);

        return cards.Sum(c => c.GetScore());
    }

    public int Part2()
    {
        var lines = File.ReadLines("Inputs/Day4_Part2.txt");
        var cards = GetCards(lines);

        var numberOfMatches = cards.ToDictionary(c => c.Id, c => c.GetMatchingNumbers());

        return cards.Sum(c => this.GetCardScore(c.Id, numberOfMatches)) + cards.Count;
    }

    private List<Scratchcard> GetCards(IEnumerable<string> lines)
    {
        var gameIdRegex = new Regex(@"^Card +(?<id>\d+): (?<scrachedNumber>[0-9\ ]*) \| (?<winningNumber>[0-9\ ]*)$");

        var cards = new List<Scratchcard>();
        foreach (var line in lines)
        {
            // Game id
            var match = gameIdRegex.Match(line);
            var cardId = int.Parse(match.Groups["id"].Value);
            var scratshedNumberString = match.Groups["scrachedNumber"].Value.Trim().Replace("  ", " ");
            var winningNumberString = match.Groups["winningNumber"].Value.Trim().Replace("  ", " ");

            var scratchedNumbers = scratshedNumberString.Split(' ').Select(int.Parse).ToList();
            var winningNumbers = winningNumberString.Split(' ').Select(int.Parse).ToList();

            cards.Add(new Scratchcard(cardId, scratchedNumbers, winningNumbers));
        }

        return cards;
    }

    private int GetCardScore(int cardId, Dictionary<int, int> cardNumberOfMatches)
    {
        var numberOfMatchingNumbers = cardNumberOfMatches[cardId];
        if (numberOfMatchingNumbers == 0)
        {
            return numberOfMatchingNumbers;
        }

        foreach (var nextCardId in Enumerable.Range(cardId + 1, numberOfMatchingNumbers))
        {
            numberOfMatchingNumbers += this.GetCardScore(nextCardId, cardNumberOfMatches);
        }

        return numberOfMatchingNumbers;
    }
}