namespace AOC2023.Puzzels;

using System.Text.RegularExpressions;

public class Day7_CamelCards
{
    private readonly Dictionary<char, string> cardStrenght = new()
    {
        { 'A', "14" },
        { 'K', "13" },
        { 'Q', "12" },
        { 'J', "11" },
        { 'T', "10" },
        { '9', "09" },
        { '8', "08" },
        { '7', "07" },
        { '6', "06" },
        { '5', "05" },
        { '4', "04" },
        { '3', "03" },
        { '2', "02" },
    };

    private readonly Dictionary<char, string> cardStrenght2 = new()
    {
        { 'A', "14" },
        { 'K', "13" },
        { 'Q', "12" },
        { 'T', "11" },
        { '9', "10" },
        { '8', "09" },
        { '7', "08" },
        { '6', "07" },
        { '5', "06" },
        { '4', "05" },
        { '3', "04" },
        { '2', "03" },
        { 'J', "02" },
    };

    private enum Hand
    {
        HighCard = 0,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind,
    }

    private record Play(char[] Cards, int CardValue, int Bet);

    public long Part1()
    {
        var lines = File.ReadAllLines("Inputs/Day7.txt");
        var plays = this.GetPlays(lines);

        var handsPerPlay = this.GetHandsFromPlay(plays).OrderBy((p) => (int)p.Value * 10000000000 + p.Key.CardValue);
        var result = 0;
        for (var i = 0; i < handsPerPlay.Count(); i++)
        {
            result += handsPerPlay.ElementAt(i).Key.Bet * (i + 1);
        }

        return result;
    }

    public long Part2()
    {
        var lines = File.ReadAllLines("Inputs/Day7.txt");
        var plays = this.GetPlays(lines);

        var handsPerPlay = this.GetHandsFromPlay2(plays).OrderBy((p) => (int)p.Value * 10000000000 + p.Key.CardValue);
        var result = 0;
        for (var i = 0; i < handsPerPlay.Count(); i++)
        {
            result += handsPerPlay.ElementAt(i).Key.Bet * (i + 1);
        }

        return result;
    }

    private Dictionary<Play, Hand> GetHandsFromPlay(IEnumerable<Play> plays)
    {
        var handsPerPlay = new Dictionary<Play, Hand>();
        foreach (var play in plays)
        {
            var cardsCount = new Dictionary<char, int>();
            foreach (var distinctCard in play.Cards.Distinct())
            {
                var cardCount = play.Cards.Count(c => c == distinctCard);
                cardsCount.Add(distinctCard, cardCount);
            }

            handsPerPlay.Add(play, this.GetHand(cardsCount));
        }

        return handsPerPlay;
    }

    private Dictionary<Play, Hand> GetHandsFromPlay2(IEnumerable<Play> plays)
    {
        var handsPerPlay = new Dictionary<Play, Hand>();
        foreach (var play in plays)
        {
            var cardsCounts = new List<(Dictionary<char, int>, int)>();
            var distinctCards = play.Cards.Distinct();
            if (!distinctCards.Contains('J') || distinctCards.All(c => c == 'J'))
            {
                var cardsCount = new Dictionary<char, int>();
                foreach (var distinctCard in distinctCards)
                {
                    var cardCount = play.Cards.Count(c => c == distinctCard);
                    cardsCount.Add(distinctCard, cardCount);
                }

                cardsCounts.Add((cardsCount, play.CardValue));
            }
            else
            {
                var countOfJ = play.Cards.Count(c => c == 'J');
                var distinctCardsWithoutJ = distinctCards.Where(c => c != 'J');
                foreach (var focusedCard in distinctCardsWithoutJ)
                {
                    var cardsCount = new Dictionary<char, int>();
                    foreach (var distinctCard in distinctCardsWithoutJ)
                    {
                        var cardCount = play.Cards.Count(c => c == distinctCard);
                        if (distinctCard == focusedCard)
                        {
                            cardCount += countOfJ;
                        }

                        cardsCount.Add(distinctCard, cardCount);
                    }
                    var newCardValue = int.Parse(string.Join(null, play.Cards.Select(c => c == 'J' ? focusedCard : c).Select(cardsString => this.cardStrenght[cardsString])));
                    cardsCounts.Add((cardsCount, newCardValue));
                }
            }

            var highestHand = cardsCounts.Select((c) => (this.GetHand(c.Item1), c.Item2)).OrderByDescending((hand) => (int)hand.Item1 * 10000000000 + hand.Item2).First().Item1;

            handsPerPlay.Add(play, highestHand);
        }

        return handsPerPlay;
    }

    private Hand GetHand(Dictionary<char, int> cardsCount)
    {
        switch (cardsCount.Count)
        {
            case 1:
                return Hand.FiveOfAKind;
            case 2:
                {
                    if (cardsCount.Any(c => c.Value == 4))
                    {
                        return Hand.FourOfAKind;
                    }
                    else
                    {
                        return Hand.FullHouse;
                    }
                }

            case 3:
                {
                    if (cardsCount.Any(c => c.Value == 3))
                    {
                        return Hand.ThreeOfAKind;
                    }
                    else
                    {
                        return Hand.TwoPair;
                    }
                }

            case 4:
                return Hand.OnePair;
            default:
                return Hand.HighCard;
        }
    }

    private IEnumerable<Play> GetPlays(string[] lines)
    {
        var regex = new Regex(@"^(?<cards>[AKQJT2-9]{5}) (?<bet>\d+)$");
        foreach (var line in lines)
        {
            var match = regex.Match(line);
            var cardsString = match.Groups["cards"].Value;
            var cardValue = int.Parse(string.Join(null, cardsString.Select(cardsString => this.cardStrenght[cardsString])));
            var cards = cardsString.ToCharArray();
            var bet = int.Parse(match.Groups["bet"].Value);
            yield return new Play(cards, cardValue, bet);
        }
    }
}