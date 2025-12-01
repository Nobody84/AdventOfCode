namespace AOC2025.Puzzels;

using System.Collections.Generic;

public class Day01_SecretEntrance : PuzzelBase
{
    private IEnumerable<string> lines = Array.Empty<string>();
    private int dialStartPos;

    public Day01_SecretEntrance()
        : base(1, "Secret Entrance")
    {
    }

    protected override void PreparePart1(string inputPath)
    {
        this.lines = File.ReadLines(inputPath);
        this.dialStartPos = 50;

    }

    protected override object Part1()
    {
        var zeroPosCount = 0;
        var pos = this.dialStartPos;
        foreach (var l in lines)
        {
            pos = (100 + pos + (l[0] == 'R' ? 1 : -1) * int.Parse(l.Substring(1))) % 100;
            zeroPosCount += pos == 0 ? 1 : 0;
        }

        return zeroPosCount;
    }

    protected override object Part2()
    {
        var zeroPosCount = 0;
        var pos = this.dialStartPos;
        foreach (var l in lines)
        {
            var clicksToMove = int.Parse(l.Substring(1));
            zeroPosCount += clicksToMove / 100;
            clicksToMove = clicksToMove % 100;

            var newPos = pos + (l[0] == 'R' ? 1 : -1) * clicksToMove;
            if (pos != 0 && (newPos <= 0 || newPos >= 100))
            {
                zeroPosCount += 1;
            }

            pos = (100 + newPos) % 100;
        }

        return zeroPosCount;
    }
}