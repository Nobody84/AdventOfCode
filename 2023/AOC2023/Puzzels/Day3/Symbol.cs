namespace AOC2023.Puzzels.Day3;

internal class Symbol
{
    public int X { get; init; }
    public int Y { get; init; }
    public string Value { get; init; }

    public List<Number> AdjacentNumbers { get; init; } = new List<Number>();

    public void AddToAdjacentNumberIfAdjacent(Number number)
    {
        if (number.IsAdjacent(this))
        {
            this.AdjacentNumbers.Add(number);
        }
    }

    public bool IsAdjacent(Number number)
    {
        return number.IsAdjacent(this);
    }
}