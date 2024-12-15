namespace AOC2024.Puzzels;

using System;
using System.Text.RegularExpressions;

public class Day13_ClawContraption : PuzzelBase
{
    private readonly Regex buttonBehaviourRegex = new Regex(@".*X(?<offsetX>[\+|\-]{1}\d+).*Y(?<offsetY>[\+|\-]{1}\d+)");
    private readonly Regex priceRegex = new Regex(@".*X=(?<X>\d+).*Y=(?<Y>\d+)");

    private List<ClawMachine> clawMachines = new();

    private record ButtonBehaviour(double X, double Y);

    public record Position(double X, double Y);

    private record ClawMachine(ButtonBehaviour buttonA, ButtonBehaviour buttonB, Position pricePosition);

    public Day13_ClawContraption()
    : base(13, "Claw Contraption")
    {
    }

    protected override void PreparePart1(string inputFile)
    {
        this.clawMachines.Clear();
        var lines = File.ReadLines(inputFile).ToArray();
        for (var i = 0; i < lines.Length; i += 4)
        {
            var buttonBehaviourAMacth = buttonBehaviourRegex.Match(lines[i]);
            var buttonBehaviourBMatch = buttonBehaviourRegex.Match(lines[i + 1]);
            var priceMatch = priceRegex.Match(lines[i + 2]);

            var buttonA = new ButtonBehaviour(int.Parse(buttonBehaviourAMacth.Groups["offsetX"].Value), int.Parse(buttonBehaviourAMacth.Groups["offsetY"].Value));
            var buttonB = new ButtonBehaviour(int.Parse(buttonBehaviourBMatch.Groups["offsetX"].Value), int.Parse(buttonBehaviourBMatch.Groups["offsetY"].Value));
            var pricePosition = new Position(int.Parse(priceMatch.Groups["X"].Value), int.Parse(priceMatch.Groups["Y"].Value));

            this.clawMachines.Add(new ClawMachine(buttonA, buttonB, pricePosition));
        }
    }

    protected override object Part1()
    {
        var count = 0m;
        foreach (var clawMachine in this.clawMachines)
        {
            var p = clawMachine.pricePosition;
            var a = clawMachine.buttonA;
            var b = clawMachine.buttonB;

            if (a.X * b.Y == b.X * a.Y)
            {
                continue;
            }

            var nA = (int)((p.X * b.Y - p.Y * b.X) / (a.X * b.Y - a.Y * b.X));
            var nB = (int)((p.X * a.Y - p.Y * a.X) / (b.X * a.Y - b.Y * a.X));

            if (nA + nB > 100)
            {
                continue;
            }

            if (nA * a.X + nB * b.X == p.X && nA * a.Y + nB * b.Y == p.Y)
            {
                count += (nA * 3 + nB);
            }
        }

        return count;
    }

    protected override object Part2()
    {
        UInt128 count = 0;
        foreach (var clawMachine in this.clawMachines)
        {
            var p = new Position(clawMachine.pricePosition.X + 10000000000000, clawMachine.pricePosition.Y + 10000000000000);
            var a = clawMachine.buttonA;
            var b = clawMachine.buttonB;

            if (a.X * b.Y == b.X * a.Y)
            {
                continue;
            }

            var nA = (UInt128)((p.X * b.Y - p.Y * b.X) / (a.X * b.Y - a.Y * b.X));
            var nB = (UInt128)((p.X * a.Y - p.Y * a.X) / (b.X * a.Y - b.Y * a.X));

            if (nA < 0 || nB < 0)
            {
                continue;
            }

            if (nA * (UInt128)a.X + nB * (UInt128)b.X == (UInt128)p.X && nA * (UInt128)a.Y + nB * (UInt128)b.Y == (UInt128)p.Y)
            {
                count += (nA * (UInt128)3 + nB);
            }
        }

        return count;
    }
}