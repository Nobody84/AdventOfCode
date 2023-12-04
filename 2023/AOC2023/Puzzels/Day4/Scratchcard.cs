namespace AOC2023.Puzzels.Day4;

internal class Scratchcard
{
    public Scratchcard(int id, List<int> scratchedNumbers, List<int> winningNumbers)
    {
        this.Id = id;
        this.ScratchedNumbers = scratchedNumbers;
        this.WinningNumbers = winningNumbers;
    }

    public int Id { get; }

    public List<int> ScratchedNumbers { get; }

    public List<int> WinningNumbers { get; }

    public int GetMatchingNumbers()
    {
        return this.ScratchedNumbers.Count(n => this.WinningNumbers.Contains(n));
    }

    public int GetScore()
    {
        var matchingNumbers = this.GetMatchingNumbers();

        if (matchingNumbers == 0)
        {
            return 0;
        }

        return (int)Math.Pow(2, matchingNumbers - 1);
    }
}