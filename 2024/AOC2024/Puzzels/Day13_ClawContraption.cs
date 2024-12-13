namespace AOC2024.Puzzels;

using System.Text.RegularExpressions;

public class Day13_ClawContraption : PuzzelBase
{
    private readonly Regex buttonBehaviourRegex = new Regex(@".*X(?<offsetX>[\+|\-]{1}\d+).*Y(?<offsetY>[\+|\-]{1}\d+)");
    private readonly Regex priceRegex = new Regex(@".*X=(?<X>\d+).*Y=(?<Y>\d+)");

    private List<ClawMachine> clawMachines = new();

    private record ButtonBehaviour(int offsetX, int offsetY);

    public record Position(int X, int Y);

    private record ClawMachine(ButtonBehaviour buttonA, ButtonBehaviour buttonB, Position pricePosition);

    public Day13_ClawContraption()
        : base(13, "Claw Contraption")
    {
    }

    protected override void PreparePart1(string inputFile)
    {
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
        foreach(var clawMachine in this.clawMachines)
        {
            var price = clawMachine.pricePosition;
            var buttonA = clawMachine.buttonA;
            var buttonB = clawMachine.buttonB;

            // Binary Tree Search
        }
        return 0;
    }

    protected override object Part2()
    {
        return 0;
    }
}