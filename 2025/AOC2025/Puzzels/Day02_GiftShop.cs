namespace AOC2025.Puzzels;

using System.Collections.Generic;
using System.Net.Sockets;

public class Day02_GiftShop : PuzzelBase
{
    private string line = string.Empty;
    private IEnumerable<string[]> ranges;

    public Day02_GiftShop()
        : base(2, "Gift Shop")
    {
    }

    protected override void PreparePart1(string inputPath)
    {
        this.line = File.ReadLines(inputPath).First();
        this.ranges = this.line.Split(',') // Split into groups
            .Select(rangeString => rangeString.Split('-')); // Split into range numbers [start, end]
    }

    protected override object Part1()
    {
        var sum = 0m;
        foreach (var range in this.ranges)
        {
            var start = decimal.Parse(range[0]);
            var end = decimal.Parse(range[1]);
            for (var i = start; i < end; i++)
            {
                var numberString = i.ToString();
                if (numberString.Length % 2 != 0)
                {
                    continue;
                }

                var halfLength = numberString.Length / 2;
                if (numberString.Substring(0, halfLength) != numberString.Substring(halfLength))
                {
                    continue;
                }

                sum += i;
            }
        }

        return sum;
    }

    protected override object Part2()
    {
        var sum = 0m;
        foreach (var range in this.ranges)
        {
            var start = decimal.Parse(range[0]);
            var end = decimal.Parse(range[1]);
            // n: current number
            for (var n = start; n <= end; n++)
            {
                var numberString = n.ToString();
                // pl: pattern length
                for (var pl = 1; pl < numberString.Length; pl++)
                {
                    if (numberString.Length % pl != 0)
                    {
                        continue;
                    }

                    var currentPattern = numberString.Substring(0, pl);
                    var isMatch = true;
                    // po: pattern offset
                    for (var po = pl; po < numberString.Length; po += pl)
                    {
                        if (currentPattern != numberString.Substring(po, pl))
                        {
                            isMatch = false;
                            break;
                        }
                    }

                    if (isMatch)
                    {
                        sum += n;
                        break;
                    }
                }
            }
        }

        return sum;
    }
}