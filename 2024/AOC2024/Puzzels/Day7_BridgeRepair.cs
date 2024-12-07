namespace AOC2024.Puzzels;

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

public class Day7_BridgeRepair : PuzzelBase
{
    private readonly Regex numberRegex = new(@"(\d+)");
    private record Equation(ulong ExpectedResult, ulong[] Terms);

    private List<Equation> equations = new();

    public Day7_BridgeRepair()
        : base(7, "Bridge Repair")
    {
    }

    protected override void PreparePart1(string inputFilePath)
    {
        equations.Clear();
        var inputLines = File.ReadLines(inputFilePath).ToList();
        for (var i = 0; i < inputLines.Count; i++)
        {
            var matches = numberRegex.Matches(inputLines[i]);

            var expectedResult = ulong.Parse(matches[0].Value);
            var terms = matches.Skip(1).Select(m => ulong.Parse(m.Value)).ToArray();

            equations.Add(new Equation(expectedResult, terms));
        }
    }

    protected override object Part1()
    {
        char[] operators = [ '*', '+' ];
        var combinationDict = new Dictionary<int, List<char[]>>();

        ulong totalCalibrationResult = 0u;

        foreach (var equation in equations)
        {
            if (Check(equation.ExpectedResult, equation.Terms[0], equation.Terms, operators, 1))
            {
                totalCalibrationResult += equation.ExpectedResult;
            }
        }

        return totalCalibrationResult;
    }

    protected override void PreparePart2(string inputFilePath)
    {
        PreparePart1(inputFilePath);
    }

    protected override object Part2()
    {
        char[] operators = [ '*', '+', '|' ]; 
        var combinationDict = new Dictionary<int, List<char[]>>();

        ulong totalCalibrationResult = 0u;

        foreach (var equation in equations)
        {
            if (Check(equation.ExpectedResult, equation.Terms[0], equation.Terms, operators, 1))
            {
               totalCalibrationResult+=equation.ExpectedResult;
            }
        }

        return totalCalibrationResult;
    }

    private static bool Check(ulong expectedResult, ulong currentResult, ulong[] terms, char[] operators, int pos)
    {
        var correctResult = false;
        foreach (var op in operators)
        {
            var newResult = op switch
            {
                '+' => currentResult + terms[pos],
                '*' => currentResult * terms[pos],
                '|' => ulong.Parse($"{currentResult}{terms[pos]}"),
                _ => throw new InvalidOperationException()
            };

            if (newResult > expectedResult)
            {
                continue;
            }

            if (pos == terms.Length - 1)
            {
                if (newResult == expectedResult)
                {
                    return true;
                }

                continue;
            }

            correctResult = Check(expectedResult, newResult, terms, operators, pos + 1);
            if (correctResult)
            {
                break;
            }
        }

        return correctResult;
    }
}