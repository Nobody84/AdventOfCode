namespace AOC2024.Puzzels;

using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

public class Day6_GuardGallivant
{
    private readonly Regex opsticalRegex = new Regex(@"#{1}");
    private readonly Regex guardRegex = new Regex(@"[<|^|>|v]{1}");
    private enum Orientation
    {
        Up,
        Right,
        Down,
        Left,
    }

    record Position(int Y, int X);

    record PositionWithOrientation(int Y, int X, Orientation Orientation);

    public int Part1()
    {
        var inputLines = File.ReadLines("Inputs/Day6.txt").ToList();

        var obstaclePositions = new List<Position>();
        var startPosition = new Position(0, 0);
        var startOrientation = Orientation.Up;
        var yMax = inputLines.Count;
        var xMax = inputLines[0].Length;

        for (var y = 0; y < inputLines.Count; y++)
        {
            var line = inputLines[y];
            var obstacleMatches = opsticalRegex.Matches(line);
            foreach (Match match in obstacleMatches)
            {
                obstaclePositions.Add(new Position(y, match.Index));
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

        var visitedPositions = TracePath(obstaclePositions.OrderBy(p => p.Y).ThenBy(p => p.X).ToList(), yMax, xMax, startPosition, startOrientation);

        //PrintMaze(inputLines, visitedPositions, startPosition);
        return visitedPositions.Distinct().Count();
    }

    public int Part2()
    {
        var inputLines = File.ReadLines("Inputs/Day6.txt").ToList();

        var obstaclePositions = new List<Position>();
        var startPosition = new Position(0, 0);
        var startOrientation = Orientation.Up;
        var yMax = inputLines.Count;
        var xMax = inputLines[0].Length;

        for (var y = 0; y < inputLines.Count; y++)
        {
            var line = inputLines[y];
            var obstacleMatches = opsticalRegex.Matches(line);
            foreach (Match match in obstacleMatches)
            {
                obstaclePositions.Add(new Position(y, match.Index));
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

        var visitedPositions = TracePath(obstaclePositions, yMax, xMax, startPosition, startOrientation);

        var count = 0;
        var loopCount = 1;
        var posibleObstructionPositions = visitedPositions.Distinct().ToList();
        posibleObstructionPositions.RemoveAll(p => p.X == startPosition.X && p.Y == startPosition.Y);

        var maxLoops = posibleObstructionPositions.Count;
        Parallel.ForEach(posibleObstructionPositions, visitedPositon =>
        {
            var alteredObstaclePositions = obstaclePositions.ToList();
            alteredObstaclePositions.Add(visitedPositon);
            alteredObstaclePositions = alteredObstaclePositions.OrderBy(p => p.Y).ThenBy(p => p.X).ToList();
            var looped = FindLoop(alteredObstaclePositions, yMax, xMax, startPosition, startOrientation, out List<PositionWithOrientation> visitedPositions2);
            if (looped)
            {                
                count++;
            }
        });

        return count;
    }

    private static List<Position> TracePath(List<Position> obstaclePositions, int yMax, int xMax, Position startPosition, Orientation startOrientation)
    {
        var guardPosition = startPosition;
        var guardOrientation = startOrientation;
        var visitedPositions = new List<Position> { new Position(guardPosition.Y, guardPosition.X) };
        while (true)
        {
            Position? nextObstaclePosition = null;
            switch (guardOrientation)
            {
                case Orientation.Up:
                    nextObstaclePosition = obstaclePositions.LastOrDefault(p => p.X == guardPosition.X && p.Y < guardPosition.Y);
                    if (nextObstaclePosition != null)
                    {
                        for (var y = guardPosition.Y - 1; y > nextObstaclePosition.Y; y--)
                        {
                            visitedPositions.Add(new Position(y, guardPosition.X));
                        }

                        guardPosition = new Position(nextObstaclePosition.Y + 1, nextObstaclePosition.X);
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
                    nextObstaclePosition = obstaclePositions.FirstOrDefault(p => p.X > guardPosition.X && p.Y == guardPosition.Y);
                    if (nextObstaclePosition != null)
                    {
                        for (var x = guardPosition.X + 1; x < nextObstaclePosition.X; x++)
                        {
                            visitedPositions.Add(new Position(guardPosition.Y, x));
                        }

                        guardPosition = new Position(nextObstaclePosition.Y, nextObstaclePosition.X - 1);
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
                    nextObstaclePosition = obstaclePositions.FirstOrDefault(p => p.X == guardPosition.X && p.Y > guardPosition.Y);
                    if (nextObstaclePosition != null)
                    {
                        for (var y = guardPosition.Y + 1; y < nextObstaclePosition.Y; y++)
                        {
                            visitedPositions.Add(new Position(y, guardPosition.X));
                        }

                        guardPosition = new Position(nextObstaclePosition.Y - 1, nextObstaclePosition.X);
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
                    nextObstaclePosition = obstaclePositions.LastOrDefault(p => p.X < guardPosition.X && p.Y == guardPosition.Y);
                    if (nextObstaclePosition != null)
                    {
                        for (var x = guardPosition.X - 1; x > nextObstaclePosition.X; x--)
                        {
                            visitedPositions.Add(new Position(guardPosition.Y, x));
                        }

                        guardPosition = new Position(nextObstaclePosition.Y, nextObstaclePosition.X + 1);
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

            if (nextObstaclePosition == null)
            {
                break;
            }
        }

        return visitedPositions;
    }
    private static bool FindLoop(List<Position> obstaclePositions, int yMax, int xMax, Position startPosition, Orientation startOrientation, out List<PositionWithOrientation> outVisitedPositions)
    {
        var guardPosition = startPosition;
        var guardOrientation = startOrientation;
        var visitedPositions = new List<PositionWithOrientation> { new PositionWithOrientation(guardPosition.Y, guardPosition.X, startOrientation) };
        while (true)
        {
            Position? nextObstaclePosition = null;
            switch (guardOrientation)
            {
                case Orientation.Up:
                    nextObstaclePosition = obstaclePositions.LastOrDefault(p => p.X == guardPosition.X && p.Y < guardPosition.Y);
                    if (nextObstaclePosition != null)
                    {
                        for (var y = guardPosition.Y - 1; y > nextObstaclePosition.Y; y--)
                        {
                            var newVisitedPosition = new PositionWithOrientation(y, guardPosition.X, guardOrientation);
                            if (visitedPositions.Any(p => p.X == newVisitedPosition.X && p.Y == newVisitedPosition.Y && p.Orientation == newVisitedPosition.Orientation))
                            {
                                outVisitedPositions = visitedPositions;
                                return true;
                            }
                            visitedPositions.Add(newVisitedPosition);
                        }

                        guardPosition = new Position(nextObstaclePosition.Y + 1, nextObstaclePosition.X);
                        guardOrientation = Orientation.Right;
                    }
                    else
                    {
                        for (var y = guardPosition.Y - 1; y >= 0; y--)
                        {
                            var newVisitedPosition = new PositionWithOrientation(y, guardPosition.X, guardOrientation);
                            if (visitedPositions.Any(p => p.X == newVisitedPosition.X && p.Y == newVisitedPosition.Y && p.Orientation == newVisitedPosition.Orientation))
                            {
                                outVisitedPositions = visitedPositions;
                                return true;
                            }
                            visitedPositions.Add(newVisitedPosition);
                        }
                    }
                    break;

                case Orientation.Right:
                    nextObstaclePosition = obstaclePositions.FirstOrDefault(p => p.X > guardPosition.X && p.Y == guardPosition.Y);
                    if (nextObstaclePosition != null)
                    {
                        for (var x = guardPosition.X + 1; x < nextObstaclePosition.X; x++)
                        {
                            var newVisitedPosition = new PositionWithOrientation(guardPosition.Y, x, guardOrientation);
                            if (visitedPositions.Any(p => p.X == newVisitedPosition.X && p.Y == newVisitedPosition.Y && p.Orientation == newVisitedPosition.Orientation))
                            {
                                outVisitedPositions = visitedPositions;
                                return true;
                            }
                            visitedPositions.Add(newVisitedPosition);
                        }

                        guardPosition = new Position(nextObstaclePosition.Y, nextObstaclePosition.X - 1);
                        guardOrientation = Orientation.Down;
                    }
                    else
                    {
                        for (var x = guardPosition.X + 1; x < xMax; x++)
                        {
                            var newVisitedPosition = new PositionWithOrientation(guardPosition.Y, x, guardOrientation);
                            if (visitedPositions.Any(p => p.X == newVisitedPosition.X && p.Y == newVisitedPosition.Y && p.Orientation == newVisitedPosition.Orientation))
                            {
                                outVisitedPositions = visitedPositions;
                                return true;
                            }
                            visitedPositions.Add(newVisitedPosition);
                        }
                    }
                    break;

                case Orientation.Down:
                    nextObstaclePosition = obstaclePositions.FirstOrDefault(p => p.X == guardPosition.X && p.Y > guardPosition.Y);
                    if (nextObstaclePosition != null)
                    {
                        for (var y = guardPosition.Y + 1; y < nextObstaclePosition.Y; y++)
                        {
                            var newVisitedPosition = new PositionWithOrientation(y, guardPosition.X, guardOrientation);
                            if (visitedPositions.Any(p => p.X == newVisitedPosition.X && p.Y == newVisitedPosition.Y && p.Orientation == newVisitedPosition.Orientation))
                            {
                                outVisitedPositions = visitedPositions;
                                return true;
                            }
                            visitedPositions.Add(newVisitedPosition);
                        }

                        guardPosition = new Position(nextObstaclePosition.Y - 1, nextObstaclePosition.X);
                        guardOrientation = Orientation.Left;
                    }
                    else
                    {
                        for (var y = guardPosition.Y + 1; y < yMax; y++)
                        {
                            var newVisitedPosition = new PositionWithOrientation(y, guardPosition.X, guardOrientation);
                            if (visitedPositions.Any(p => p.X == newVisitedPosition.X && p.Y == newVisitedPosition.Y && p.Orientation == newVisitedPosition.Orientation))
                            {
                                outVisitedPositions = visitedPositions;
                                return true;
                            }
                            visitedPositions.Add(newVisitedPosition);
                        }
                    }
                    break;

                case Orientation.Left:
                    nextObstaclePosition = obstaclePositions.LastOrDefault(p => p.X < guardPosition.X && p.Y == guardPosition.Y);
                    if (nextObstaclePosition != null)
                    {
                        for (var x = guardPosition.X - 1; x > nextObstaclePosition.X; x--)
                        {
                            var newVisitedPosition = new PositionWithOrientation(guardPosition.Y, x, guardOrientation);
                            if (visitedPositions.Any(p => p.X == newVisitedPosition.X && p.Y == newVisitedPosition.Y && p.Orientation == newVisitedPosition.Orientation))
                            {
                                outVisitedPositions = visitedPositions;
                                return true;
                            }
                            visitedPositions.Add(newVisitedPosition);
                        }

                        guardPosition = new Position(nextObstaclePosition.Y, nextObstaclePosition.X + 1);
                        guardOrientation = Orientation.Up;
                    }
                    else
                    {
                        for (var x = guardPosition.X - 1; x >= 0; x--)
                        {
                            var newVisitedPosition = new PositionWithOrientation(guardPosition.Y, x, guardOrientation);
                            if (visitedPositions.Any(p => p.X == newVisitedPosition.X && p.Y == newVisitedPosition.Y && p.Orientation == newVisitedPosition.Orientation))
                            {
                                outVisitedPositions = visitedPositions;
                                return true;
                            }
                            visitedPositions.Add(newVisitedPosition);
                        }
                    }
                    break;
            }

            if (nextObstaclePosition == null)
            {
                break;
            }
        }

        outVisitedPositions = visitedPositions;
        return false;
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
                var character = inputLine[x] != '#' ? $"{count}"[0] : '#';
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

    private static void PrintMaze2(List<string> inputLines, List<PositionWithOrientation> visitedPositions, Position startPosition, Position newObstacle)
    {
        for (var y = 0; y < inputLines.Count; y++)
        {
            var inputLine = inputLines[y];
            for (var x = 0; x < inputLine.Length; x++)
            {
                var count = visitedPositions.Where(p => p.X == x && p.Y == y).Count();
                var color = ConsoleColor.White;

                var character = '#';
                if (newObstacle.X == x && newObstacle.Y == y)
                {
                    character = 'O';
                    color = ConsoleColor.DarkRed;
                }
                else if (startPosition.X == x && startPosition.Y == y)
                {
                    character = 'K';
                    color = ConsoleColor.Magenta;
                }
                else
                {
                    var pos = visitedPositions.FirstOrDefault(p => p.X == x && p.Y == y);
                    if (pos != null)
                    {
                        character = pos.Orientation switch
                        {
                            Orientation.Up => '^',
                            Orientation.Right => '>',
                            Orientation.Down => 'v',
                            Orientation.Left => '<',
                            _ => throw new Exception("Invalid orientation"),
                        };
                    }
                    else
                    {
                        character = inputLine[x];
                    }
                }

                ConsoleExtensions.Write(character, color);
            }

            Console.WriteLine();
        }
    }
}