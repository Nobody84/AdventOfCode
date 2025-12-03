namespace AOC2025.Puzzels;

using System.Collections.Generic;
using System.Net.Sockets;

public class Day03_Lobby : PuzzelBase
{
    private string line = string.Empty;
    private IEnumerable<int[]> batteryBanks;

    public Day03_Lobby()
        : base(3, "Lobby")
    {
    }

    protected override void PreparePart1(string inputPath)
    {
        var lines = File.ReadLines(inputPath);
        this.batteryBanks = lines.Select(line => line.Select(d => int.Parse($"{d}")).ToArray());
    }

    protected override object Part1()
    {
        var sum = 0m;
        foreach (var bank in this.batteryBanks)
        {           
            var firstDigit = bank.Take(bank.Length - 1).Max();
            var firstDigitIndex = Array.IndexOf(bank, firstDigit);

            var secondDigit = bank.Skip(firstDigitIndex + 1).Max();

            var joltage = firstDigit * 10 + secondDigit;
            sum += joltage;
        }

        return sum;
    }

    protected override object Part2()
    {
        var numberOfDigits = 12;
        var digits = new int[numberOfDigits];
        var sum = 0m;
        foreach (var bank in this.batteryBanks)
        {
            var lastDigitIndex = -1;
            for (var i = 0; i < numberOfDigits; i++)
            {
                var offset = lastDigitIndex + 1;
                var gabToEnd = numberOfDigits - i - 1;
                digits[i] = bank.Skip(offset).Take(bank.Length - offset - gabToEnd).Max();
                lastDigitIndex = offset + Array.IndexOf(bank.Skip(offset).ToArray(), digits[i]);
            }

            var joltage = decimal.Parse(string.Join("", digits));
            sum += joltage;
        }

        return sum;
    }
}