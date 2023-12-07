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
        { 'T', "10" },
        { '9', "09" },
        { '8', "08" },
        { '7', "07" },
        { '6', "06" },
        { '5', "05" },
        { '4', "04" },
        { '3', "03" },
        { '2', "02" },
        { 'J', "01" },
    };

    private enum HandType
    {
        HighCard = 0,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind,
    }

    private record Play(char[] Cards, int Bet);

    private record Hand(HandType Type, int HandValue);

    private record PlayResult(Play Play, Hand Hand);


    public long Part1()
    {
        var lines = File.ReadAllLines("Inputs/Day7.txt");
        var plays = this.GetPlays(lines);

        var playResults = this.GetPlayResults(plays).OrderBy((pr) => this.GetValue(pr.Hand));

        var result = 0;
        for (var i = 0; i < playResults.Count(); i++)
        {
            result += playResults.ElementAt(i).Play.Bet * (i + 1);
        }

        return result;
    }

    public long Part2()
    {
        var lines = File.ReadAllLines("Inputs/Day7.txt");
        var plays = this.GetPlays(lines);

        var playResults = this.GetPlayResults2(plays).OrderBy((pr) => this.GetValue(pr.Hand));
        foreach (var playResult in playResults)
        {
            Console.WriteLine($"{string.Join(null, playResult.Play.Cards)} - {playResult.Hand.Type} - {playResult.Hand.HandValue}");
        }

        var result = 0;
        for (var i = 0; i < playResults.Count(); i++)
        {
            result += playResults.ElementAt(i).Play.Bet * (i + 1);
        }

        return result;
    }

    private List<PlayResult> GetPlayResults(IEnumerable<Play> plays)
    {
        var playResults = new List<PlayResult>();
        foreach (var play in plays)
        {
            var cardsCount = new Dictionary<char, int>();
            foreach (var distinctCard in play.Cards.Distinct())
            {
                var cardCount = play.Cards.Count(c => c == distinctCard);
                cardsCount.Add(distinctCard, cardCount);
            }

            playResults.Add(new PlayResult(play, new Hand(this.GetHandType(cardsCount), this.CalcHandValue(play.Cards))));
        }

        return playResults;
    }

    private List<PlayResult> GetPlayResults2(IEnumerable<Play> plays)
    {
        var playResults = new List<PlayResult>();
        foreach (var play in plays)
        {
            var distinctCards = play.Cards.Distinct();

            Hand hand;

            if (!distinctCards.Contains('J'))
            {
                var cardsCount = new Dictionary<char, int>();
                foreach (var distinctCard in distinctCards)
                {
                    var cardCount = play.Cards.Count(c => c == distinctCard);
                    cardsCount.Add(distinctCard, cardCount);
                }

                hand = new Hand(this.GetHandType(cardsCount), this.CalcHandValue2(play.Cards));
            }
            else if (distinctCards.All(c => c == 'J'))
            {
                var cardsCount = new Dictionary<char, int>() { { 'A', 5 } };
                hand = new Hand(this.GetHandType(cardsCount), this.CalcHandValue2(play.Cards));
            }
            else
            {
                var possibleHands = new List<Hand>();
                foreach (var focusedCard in distinctCards.Where(c => c != 'J').ToList())
                {
                    var newCards = play.Cards.Select(c => c == 'J' ? focusedCard : c).ToArray();
                    var cardsCount = new Dictionary<char, int>();
                    foreach (var distinctCard in distinctCards.Where(c => c != 'J').ToList())
                    {
                        var cardCount = newCards.Count(c => c == distinctCard);
                        cardsCount.Add(distinctCard, cardCount);
                    }

                    possibleHands.Add(new Hand(this.GetHandType(cardsCount), this.CalcHandValue2(play.Cards)));
                }

                hand = possibleHands.OrderByDescending(h => this.GetValue(h)).First();
            }


            playResults.Add(new PlayResult(play, hand));
        }

        return playResults;
    }

    private HandType GetHandType(Dictionary<char, int> cardsCount)
    {
        switch (cardsCount.Count)
        {
            case 1:
                return HandType.FiveOfAKind;
            case 2:
                {
                    if (cardsCount.Any(c => c.Value == 4))
                    {
                        return HandType.FourOfAKind;
                    }
                    else
                    {
                        return HandType.FullHouse;
                    }
                }

            case 3:
                {
                    if (cardsCount.Any(c => c.Value == 3))
                    {
                        return HandType.ThreeOfAKind;
                    }
                    else
                    {
                        return HandType.TwoPair;
                    }
                }

            case 4:
                return HandType.OnePair;
            default:
                return HandType.HighCard;
        }
    }

    private IEnumerable<Play> GetPlays(string[] lines)
    {
        var regex = new Regex(@"^(?<cards>[AKQJT2-9]{5}) (?<bet>\d+)$");
        foreach (var line in lines)
        {
            var match = regex.Match(line);
            var cards = match.Groups["cards"].Value.ToCharArray();
            var bet = int.Parse(match.Groups["bet"].Value);
            yield return new Play(cards, bet);
        }
    }

    private int CalcHandValue(char[] cards)
    {
        return int.Parse(string.Join(null, cards.Select(cardsString => this.cardStrenght[cardsString])));
    }

    private int CalcHandValue2(char[] cards)
    {
        return int.Parse(string.Join(null, cards.Select(cardsString => this.cardStrenght2[cardsString])));
    }

    private long GetValue(Hand hand)
    {
        return (int)hand.Type * 10000000000 + hand.HandValue;
    }
}