namespace AOC2023.Puzzels.Day3;

internal class Number
{
    public int X { get; init; }
    public int Y { get; init; }
    public string Value { get; init; } = string.Empty;

    public bool IsAdjacent(Symbol symbol)
    {
        var isAdjacent = false;
        for (var offsetX = 0; offsetX < this.Value.Length; offsetX++)
        {
            isAdjacent |= Math.Abs((X + offsetX) - symbol.X) <= 1 && Math.Abs(Y - symbol.Y) <= 1;
        }

        return isAdjacent;
    }
}