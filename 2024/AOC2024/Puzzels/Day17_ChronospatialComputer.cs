using System.Text.RegularExpressions;

namespace AOC2024.Puzzels;

public class Day17_ChronospatialComputer : PuzzelBase
{
    private readonly Regex registerRegex = new(@"Register (?<register>\w): (?<value>\d+)");
    private readonly Regex instructionRegex = new(@"([\d])");

    private List<Instruction> instructionStack;
    private int instructionPointer = 0;
    private int regA;
    private int regB;
    private int regC;

    private List<int> output;

    private record Instruction(int Opcode, int Operant);

    public Day17_ChronospatialComputer()
     : base(17, "Chronospatial Computer")
    {
    }

    protected override void PreparePart1(string inputFile)
    {
        this.output = new();
        this.instructionStack = new();

        var lines = File.ReadLines(inputFile);
        foreach (var line in lines)
        {
            if (line.StartsWith("Register"))
            {
                var match = registerRegex.Match(line);
                var register = match.Groups["register"].Value;
                var value = int.Parse(match.Groups["value"].Value);
                if (register == "A")
                {
                    regA = value;
                }
                else if (register == "B")
                {
                    regB = value;
                }
                else if (register == "C")
                {
                    regC = value;
                }
            }

            if (line.StartsWith("Program"))
            {
                var matches = instructionRegex.Matches(line);
                for (var i = 0; i < matches.Count; i += 2)
                {
                    this.instructionStack.Add(new Instruction(int.Parse(matches[i].Value), int.Parse(matches[i + 1].Value)));
                }
            }
        }
    }

    protected override object Part1()
    {
        this.instructionPointer = 0;
        while(this.instructionPointer < this.instructionStack.Count)
        {
            PerformInstruction(this.instructionStack[this.instructionPointer]);
        }

        return string.Join(",", this.output);
    }

    protected override object Part2()
    {
        var expectedResult = string.Join(",", this.instructionStack.Select(i => $"{i.Opcode},{i.Operant}"));
        var startValue = 4;
        var loopCount = 0;
        do
        {
            startValue = loopCount * 64 + 4;
            this.regA = startValue;
            this.instructionPointer = 0;
            this.output.Clear();
            while (this.instructionPointer < this.instructionStack.Count)
            {
                PerformInstruction(this.instructionStack[this.instructionPointer]);
                if (!expectedResult.StartsWith(string.Join(",", this.output)))
                {
                    break;
                }
            }
            if (expectedResult.StartsWith(string.Join(",", this.output)))
            {
                Console.WriteLine(string.Join(",", this.output));
            }
            loopCount++;
        }
        while (expectedResult != string.Join(",", this.output));

        return startValue;
    }

    private int GetOperandValue(int operant)
    {
        switch (operant)
        {
            case 0:
            case 1:
            case 2:
            case 3:
                return operant;
            case 4:
                return regA;
            case 5:
                return regB;
            case 6:
                return regC;
            default:
                throw new InvalidOperationException($"Invalid Operand {operant}");
        }
    }

    private void PerformInstruction(Instruction instruction)
    {
        if (instruction.Opcode == 0) // adv
        {
            this.regA = (int) (this.regA / Math.Pow(2, GetOperandValue(instruction.Operant)));
        }
        else if (instruction.Opcode == 1) // bxl
        {
            this.regB = this.regB ^ instruction.Operant;
        }
        else if (instruction.Opcode == 2) // bst
        {
            this.regB = (GetOperandValue(instruction.Operant) % 8) & 0x07;
        }
        else if (instruction.Opcode == 3 && regA != 0) // jnz
        {
            this.instructionPointer = instruction.Operant / 2;
            return;
        }
        else if (instruction.Opcode == 4) // bxc
        {
            this.regB = this.regB ^ this.regC;
        }
        else if (instruction.Opcode == 5) // out
        {
            this.output.Add(GetOperandValue(instruction.Operant) % 8);
        }
        else if (instruction.Opcode == 6) // bdv
        {
            this.regB = (int)(this.regA / Math.Pow(2, GetOperandValue(instruction.Operant)));
        }
        else if (instruction.Opcode == 7) // cdv
        {
            this.regC = (int)(this.regA / Math.Pow(2, GetOperandValue(instruction.Operant)));
        }

        this.instructionPointer++;
    } 
}