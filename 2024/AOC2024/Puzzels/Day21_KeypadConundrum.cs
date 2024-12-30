using System.Linq;
using System.Text;

namespace AOC2024.Puzzels;

public class Day21_KeypadConundrum : PuzzelBase
{
    private List<CodeInfo> codeInfos = new();
    private Dictionary<KeyMovement, string> numberKayPadMovements = new();
    private Dictionary<KeyMovement, string> remoteKayPadMovements = new();

    private record CodeInfo(string Code, int Value);

    private record class Position(int Y, int X);

    private Dictionary<Char, Position> numberKeyPad = new()
    {
        {'7', new Position(0, 0)},
        {'8', new Position(0, 1)},
        {'9', new Position(0, 2)},
        {'4', new Position(1, 0)},
        {'5', new Position(1, 1)},
        {'6', new Position(1, 2)},
        {'1', new Position(2, 0)},
        {'2', new Position(2, 1)},
        {'3', new Position(2, 2)},
        {'0', new Position(3, 1)},
        {'A', new Position(3, 2)},
    };

    private Dictionary<Char, Position> remoteKeyPad = new()
    {
        {'^', new Position(0, 1)},
        {'A', new Position(0, 2)},
        {'<', new Position(1, 0)},
        {'v', new Position(1, 1)},
        {'>', new Position(1, 2)},
    };

    private record KeyMovement(char StartKey, char EndKey);

    public Day21_KeypadConundrum()
     : base(21, "Keypad Conundrumn")
    {
    }

    protected override void PreparePart1(string inputFile)
    {
        var lines = File.ReadAllLines(inputFile);
        foreach (var line in lines)
        {
            this.codeInfos.Add(new CodeInfo(line, int.Parse(line.Substring(0, 3))));
        }
    }

    protected override object Part1()
    {
        var complexitySum = 0;
        foreach (var codeInfo in codeInfos)
        {
            // Number KeyPad
            var numberKeyPadInput = new StringBuilder();

            var currentCodePart = 'A';
            foreach (var codePart in codeInfo.Code)
            {
                var keyMovement = new KeyMovement(currentCodePart, codePart);
                if (numberKayPadMovements.TryGetValue(keyMovement, out var movement))
                {
                    numberKeyPadInput.Append(movement); 
                    currentCodePart = codePart;
                    continue;
                }

                var codePartPosition = numberKeyPad[codePart];
                var currentPosition = numberKeyPad[currentCodePart];
                string? inputPart = null;

                // to avoid the empty space if the next key is below the current key move horizontally if the next key is to the right move vertically first
                if (codePartPosition.Y > currentPosition.Y)
                {
                    inputPart = $"{new string(codePartPosition.X < currentPosition.X ? '<' : '>', Math.Abs(codePartPosition.X - currentPosition.X))}{new string('v', codePartPosition.Y - currentPosition.Y)}A";
                }
                else
                {
                    inputPart = $"{new string('^', currentPosition.Y - codePartPosition.Y)}{new string(codePartPosition.X < currentPosition.X ? '<' : '>', Math.Abs(codePartPosition.X - currentPosition.X))}A";
                }

                numberKayPadMovements.Add(keyMovement, inputPart);
                numberKeyPadInput.Append(inputPart);
                currentCodePart = codePart;
            }

            Console.WriteLine($"Code: {codeInfo.Code}, Input: {numberKeyPadInput}");

            // First Remote
            currentCodePart = 'A';
            var firstRemoteInput = new StringBuilder();
            foreach (var codePart in numberKeyPadInput.ToString())
            {
                var keyMovement = new KeyMovement(currentCodePart, codePart);
                if (remoteKayPadMovements.TryGetValue(keyMovement, out var value))
                {
                    firstRemoteInput.Append(value);
                    currentCodePart = codePart;
                    continue;
                }

                var codePartPosition = remoteKeyPad[codePart];
                var currentPosition = remoteKeyPad[currentCodePart];
                string? inputPart = null;

                // to avoid the empty space if the next key is below the current key move verticaly if the next key is to the right move horizontaly first
                if (codePartPosition.Y > currentPosition.Y)
                {
                    inputPart = $"{new string('v', codePartPosition.Y - currentPosition.Y)}{new string(codePartPosition.X < currentPosition.X ? '<' : '>', Math.Abs(codePartPosition.X - currentPosition.X))}A";
                }
                else
                {
                    inputPart = $"{new string(codePartPosition.X < currentPosition.X ? '<' : '>', Math.Abs(codePartPosition.X - currentPosition.X))}{new string('^', currentPosition.Y - codePartPosition.Y)}A";
                }

                remoteKayPadMovements.Add(keyMovement, inputPart);
                firstRemoteInput.Append(inputPart);
                currentCodePart = codePart;
            }

            Console.WriteLine($"First Remote: {codeInfo.Code}, Input: {firstRemoteInput}");

            // Second Remote
            currentCodePart = 'A';
            var secondRemoteInput = new StringBuilder();
            foreach (var codePart in firstRemoteInput.ToString())
            {
                var keyMovement = new KeyMovement(currentCodePart, codePart);
                if (remoteKayPadMovements.TryGetValue(keyMovement, out var value))
                {
                    secondRemoteInput.Append(value);
                    currentCodePart = codePart;
                    continue;
                }

                var codePartPosition = remoteKeyPad[codePart];
                var currentPosition = remoteKeyPad[currentCodePart];
                string? inputPart = null;

                // to avoid the empty space if the next key is below the current key move verticaly if the next key is to the right move horizontaly first
                if (codePartPosition.Y > currentPosition.Y)
                {
                    inputPart = $"{new string('v', codePartPosition.Y - currentPosition.Y)}{new string(codePartPosition.X < currentPosition.X ? '<' : '>', Math.Abs(codePartPosition.X - currentPosition.X))}A";
                }
                else
                {
                    inputPart = $"{new string(codePartPosition.X < currentPosition.X ? '<' : '>', Math.Abs(codePartPosition.X - currentPosition.X))}{new string('^', currentPosition.Y - codePartPosition.Y)}A";
                }

                remoteKayPadMovements.Add(keyMovement, inputPart);
                secondRemoteInput.Append(inputPart);
                currentCodePart = codePart;
            }

            var secondRemoteInoputString = secondRemoteInput.ToString();
            Console.WriteLine($"Second Remote: {codeInfo.Code}, Input: {secondRemoteInoputString}");
            var complexity = secondRemoteInput.ToString().Length * codeInfo.Value;
            ConsoleExtensions.WriteLine($"Complexity: {secondRemoteInoputString.Length} * {codeInfo.Value} = {complexity}", ConsoleColor.Green);
            complexitySum += complexity;
        }

        return complexitySum == 176870;
    }

    protected override object Part2()
    {
        return 0;
    }
}