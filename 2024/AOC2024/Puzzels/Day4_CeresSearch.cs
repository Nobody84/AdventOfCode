namespace AOC2024.Puzzels;

using System.Text.RegularExpressions;

public class Day4_CeresSearch
{
    private const string mullRegexPattern = @"mul\s*\(\s*(?<X>[\d]{1,3})\s*,(?<Y>[\d]{1,3})\s*\)";
    private const string dontRegexPattern = @"don't\(\)(?:.|\n)*?do\(\)";
    private const string lastDontRegexPattern = @"don't\(\)(?:.|\n)*";

    record Position(int X, int Y);
    record Offset(int X, int Y);

    public int Part1()
    {
        var count = 0;
        var inputLines = File.ReadLines("Inputs/Day4.txt").ToList();

        char[][] inputMatrix = new char[inputLines.Count][];
        for (var i = 0; i < inputLines.Count; i++)
        {
            inputMatrix[i] = inputLines[i].ToCharArray();
        }

        var startPositions = new List<Position>();
        for (var y = 0; y < inputMatrix.Length; y++)
        {
            for (var x = 0; x < inputMatrix[y].Length; x++)
            {
                if (inputMatrix[y][x] == 'X')
                {
                    startPositions.Add(new Position(x, y));
                }
            }
        }

        var xmasOrder = new char[] { 'X', 'M', 'A', 'S' };
        var directionOffsets = new List<Offset>
        {
            new Offset(0, -1), // Up
            new Offset(1, -1), // Up Right
            new Offset(1, 0), // Right
            new Offset(1, 1), // Down Right
            new Offset(0, 1), // Down
            new Offset(-1, 1), // Down Left
            new Offset(-1, 0), // Left
            new Offset(-1, -1), // Up Left
        };

        foreach (var startPosition in startPositions)
        {
            // Search in all directions
            foreach (var directionOffset in directionOffsets)
            {
                for (var i = 0; i < xmasOrder.Length; i++)
                {
                    var x = startPosition.X + directionOffset.X * i;
                    var y = startPosition.Y + directionOffset.Y * i;
                    if (x < 0 || x >= inputMatrix[0].Length || y < 0 || y >= inputMatrix.Length)
                    {
                        break;
                    }

                    if (inputMatrix[y][x] != xmasOrder[i])
                    {
                        break;
                    }

                    if (i == xmasOrder.Length - 1)
                    {
                        count++;
                    }
                }
            }
        }

        return count;
    }

    public int Part2()
    {
        var count = 0;
        var inputLines = File.ReadLines("Inputs/Day4.txt").ToList();

        char[][] inputMatrix = new char[inputLines.Count][];
        for (var i = 0; i < inputLines.Count; i++)
        {
            inputMatrix[i] = inputLines[i].ToCharArray();
        }

        var startPositions = new List<Position>();
        for (var y = 0; y < inputMatrix.Length; y++)
        {
            for (var x = 0; x < inputMatrix[y].Length; x++)
            {
                if (inputMatrix[y][x] == 'M')
                {
                    startPositions.Add(new Position(x, y));
                }
            }
        }

        var xmasOrder = new char[] { 'M', 'A', 'S' };
        var directionOffsets = new List<Offset>
        {
            new Offset(1, -1), // Up Right
            new Offset(1, 1), // Down Right
            new Offset(-1, 1), // Down Left
            new Offset(-1, -1), // Up Left
        };

        var positionsOfTheAs = new List<Position>();
        foreach (var startPosition in startPositions)
        {
            // Search in all directions
            foreach (var directionOffset in directionOffsets)
            {
                for (var i = 0; i < xmasOrder.Length; i++)
                {
                    var x = startPosition.X + directionOffset.X * i;
                    var y = startPosition.Y + directionOffset.Y * i;
                    if (x < 0 || x >= inputMatrix[0].Length || y < 0 || y >= inputMatrix.Length)
                    {
                        break;
                    }

                    if (inputMatrix[y][x] != xmasOrder[i])
                    {
                        break;
                    }

                    if (i == xmasOrder.Length - 1)
                    {
                        positionsOfTheAs.Add(new Position(x - directionOffset.X, y - directionOffset.Y));
                    }
                }
            }
        }

        var xmasPostitions = positionsOfTheAs.GroupBy(p => p).Where(g => g.Count() == 2).Select(g => g.Key);

        //for (var y = 0; y < inputMatrix.Length; y++)
        //{
        //    for (var x = 0; x < inputMatrix[y].Length; x++)
        //    {
        //        if (xmasPostitions.Any(p => p.X == x && p.Y == y))
        //        {
        //            ConsoleExtensions.Write(inputMatrix[y][x], ConsoleColor.Red);
        //        }
        //        else if (positionsOfTheAs.Any(p => p.X == x && p.Y == y))
        //        {
        //            ConsoleExtensions.Write(inputMatrix[y][x], ConsoleColor.Green);
        //        }
        //        else
        //        {
        //            Console.Write(inputMatrix[y][x]);
        //        }
        //    }

        //    Console.WriteLine();
        //}

        return xmasPostitions.Count();
    }
}