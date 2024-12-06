namespace AOC2024.Puzzels;

using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

public class Day6_GuardGallivant
{
    private readonly Regex opsticalRegex = new Regex(@"#{1}");
    private readonly Regex guardRegex = new Regex(@"[<|^|>|v]{1}");

    record Position(int Y, int X);
    enum Orientation
    {
        Up,
        Right,
        Down,
        Left,
    }

    public int Part1()
    {
        var count = 1;
        var inputLines = File.ReadLines("Inputs/Day6.txt").ToList();

        var obsticalPositions = new List<Position>();
        var startPosition = new Position(0, 0);
        var startOrientation = Orientation.Up;
        var yMax = inputLines.Count;
        var xMax = inputLines[0].Length;


        for (var y = 0; y < inputLines.Count; y++)
        {
            var line = inputLines[y];
            var obsticalMatches = opsticalRegex.Matches(line);
            foreach (Match match in obsticalMatches)
            {
                obsticalPositions.Add(new Position(y, match.Index));
            }

            var guardMatch = guardRegex.Match(line);
            if (guardMatch.Success)
            {
                startPosition = new Position(y, guardMatch.Index);
                startOrientation = guardMatch.Value switch
                {
                    "<" => Orientation.Left,
                    "^" => Orientation.Up,
                    ">" => Orientation.Right,
                    "v" => Orientation.Down,
                    _ => throw new Exception("Invalid start orientation"),
                };
            }
        }

        var guardPosition = startPosition;
        var guardOrientation = startOrientation;
        var visitedPositions = new List<Position> { new Position(guardPosition.Y, guardPosition.X) };
        while (true)
        {
            Position? nextObsticalPosition = null;
            switch (guardOrientation)
            {
                case Orientation.Up:
                    nextObsticalPosition = obsticalPositions.FirstOrDefault(p => p.X == guardPosition.X && p.Y < guardPosition.Y);
                    if (nextObsticalPosition != null)
                    {
                        for (var y = guardPosition.Y - 1; y > nextObsticalPosition.Y; y--)
                        {
                            visitedPositions.Add(new Position(y, guardPosition.X));
                        }

                        guardPosition = new Position(nextObsticalPosition.Y + 1, nextObsticalPosition.X);
                        guardOrientation = Orientation.Right;
                    }
                    else
                    {
                        for (var y = guardPosition.Y - 1; y >= 0; y--)
                        {
                            visitedPositions.Add(new Position(y, guardPosition.X));
                        }
                    }
                    break;
                case Orientation.Right:
                    nextObsticalPosition = obsticalPositions.FirstOrDefault(p => p.X > guardPosition.X && p.Y == guardPosition.Y);
                    if (nextObsticalPosition != null)
                    {
                        for (var x = guardPosition.X + 1; x < nextObsticalPosition.X; x++)
                        {
                            visitedPositions.Add(new Position(guardPosition.Y, x));
                        }

                        guardPosition = new Position(nextObsticalPosition.Y, nextObsticalPosition.X - 1);
                        guardOrientation = Orientation.Down;
                    }
                    else
                    {
                        for (var x = guardPosition.X + 1; x < xMax; x++)
                        {
                            visitedPositions.Add(new Position(guardPosition.Y, x));
                        }
                    }
                    break;
                case Orientation.Down:
                    nextObsticalPosition = obsticalPositions.FirstOrDefault(p => p.X == guardPosition.X && p.Y > guardPosition.Y);
                    if (nextObsticalPosition != null)
                    {
                        for (var y = guardPosition.Y + 1; y < nextObsticalPosition.Y; y++)
                        {
                            visitedPositions.Add(new Position(y, guardPosition.X));
                        }

                        guardPosition = new Position(nextObsticalPosition.Y - 1, nextObsticalPosition.X);
                        guardOrientation = Orientation.Left;
                    }
                    else
                    {
                        for (var y = guardPosition.Y + 1; y < yMax; y++)
                        {
                            visitedPositions.Add(new Position(y, guardPosition.X));
                        }
                    }
                    break;
                case Orientation.Left:
                    nextObsticalPosition = obsticalPositions.FirstOrDefault(p => p.X < guardPosition.X && p.Y == guardPosition.Y);
                    if (nextObsticalPosition != null)
                    {
                        for (var x = guardPosition.X - 1; x > nextObsticalPosition.X; x--)
                        {
                            visitedPositions.Add(new Position(guardPosition.Y, x));
                        }

                        guardPosition = new Position(nextObsticalPosition.Y, nextObsticalPosition.X + 1);
                        guardOrientation = Orientation.Up;
                    }
                    else
                    {
                        for (var x = guardPosition.X - 1; x >= 0; x--)
                        {
                            visitedPositions.Add(new Position(guardPosition.Y, x));
                        }
                    }
                    break;
            }

            if (nextObsticalPosition == null)
            {
                break;
            }
        }

        PrintMaze(inputLines, visitedPositions, startPosition);
        count = visitedPositions.Distinct().Count();


        return count;
    }

    private static void PrintMaze(List<string> inputLines, List<Position> visitedPositions, Position startPosition)
    {
        Console.WriteLine($"Steps: {visitedPositions.Count} Distinct: {visitedPositions.Distinct().Count()}");
        for (var y = 0; y < inputLines.Count; y++)
        {
            var inputLine = inputLines[y];
            for (var x = 0; x < inputLine.Length; x++)
            {
                var count = visitedPositions.Where(p => p.X == x && p.Y == y).Count();
                var color = ConsoleColor.White;
                var character = $"{count}"[0];
                switch (count)
                {
                    case 0:
                        character = inputLine[x];
                        break;
                    case 1:
                        color = ConsoleColor.Green;
                        break;
                    case 2:
                        color = ConsoleColor.DarkYellow;
                        break;
                    case 3:
                        color = ConsoleColor.Cyan;
                        break;
                    default:
                        color = ConsoleColor.Red;
                        break;
                }

                if (startPosition.X == x && startPosition.Y == y)
                {
                    color = ConsoleColor.Magenta;
                }

                ConsoleExtensions.Write(character, color);
            }

            Console.WriteLine();
        }
    }

    public int Part2()
    {
        var count = 0;
        var inputLines = File.ReadLines("Inputs/Day6.txt");


        return count;
    }
}