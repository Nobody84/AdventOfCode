namespace AOC2023.Puzzels;

using AOC2023.Puzzels.Day10;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;

public class Day10_PipeMaze
{

    public long Part1()
    {
        var lines = File.ReadAllLines("Inputs/Day10.txt");

        var startPosition = new Point(0, 0);
        Dictionary<Point, Pipe> maze = this.GetMaze(lines, ref startPosition);
        var borderPipes = GetBorderPipes(startPosition, maze);

        return borderPipes.Count() / 2;
    }

    public long Part2()
    {
        var lines = File.ReadAllLines("Inputs/Day10.txt");

        var startPosition = new Point(0, 0);
        Dictionary<Point, Pipe> maze = this.GetMaze(lines, ref startPosition);
        var borderPipes = GetBorderPipes(startPosition, maze);
        var borderPipesPositions = borderPipes.Select(p => p.Position).ToList();


        var enclosedPipes = new List<Pipe>();
        foreach (var pipe in maze.Values)
        {
            // Points that are on the border are not enclosed
            if (borderPipes.Any(p => p.Position == pipe.Position))
            {
                continue;
            }

            if (this.IsPointInsidBorder(borderPipesPositions, pipe.Position))
            {
                enclosedPipes.Add(pipe);
            }
        }

        this.PrintBorder(lines, borderPipes, enclosedPipes);

        return enclosedPipes.Count;
    }

    private IEnumerable<Pipe> GetBorderPipes(Point startPosition, Dictionary<Point, Pipe> maze)
    {
        var startPipe = maze[startPosition];
        var previousPipe = startPipe;
        var currentPipe = maze[startPipe.Connections.First()];
        yield return currentPipe;
        do
        {
            var nexPipe = maze[currentPipe.GetNextPosition(previousPipe.Position)];
            previousPipe = currentPipe;
            currentPipe = nexPipe;
            yield return currentPipe;
        } while (currentPipe != startPipe);
    }

    private Dictionary<Point, Pipe> GetMaze(string[] lines, ref Point startPosition)
    {
        var maze = new Dictionary<Point, Pipe>();
        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                var position = new Point(x, y);
                var c = lines[y][x];
                if (c == 'S')
                {
                    startPosition = position;
                }

                var connection = new List<Point>();
                switch (c)
                {
                    case '|':
                        connection.Add(new Point(x, y - 1));
                        connection.Add(new Point(x, y + 1));
                        break;
                    case '-':
                        connection.Add(new Point(x - 1, y));
                        connection.Add(new Point(x + 1, y));
                        break;
                    case 'L':
                        connection.Add(new Point(x, y - 1));
                        connection.Add(new Point(x + 1, y));
                        break;
                    case 'J':
                        connection.Add(new Point(x, y - 1));
                        connection.Add(new Point(x - 1, y));
                        break;
                    case '7':
                        connection.Add(new Point(x - 1, y));
                        connection.Add(new Point(x, y + 1));
                        break;
                    case 'F':
                        connection.Add(new Point(x + 1, y));
                        connection.Add(new Point(x, y + 1));
                        break;
                    case 'S':
                        if (y > 0 && (lines[y - 1][x] == '7' || lines[y - 1][x] == 'F' || lines[y - 1][x] == '|'))
                        {
                            connection.Add(new Point(x, y - 1));
                        }
                        if (y < lines.Length - 1 && (lines[y + 1][x] == 'L' || lines[y + 1][x] == 'J' || lines[y + 1][x] == '|'))
                        {
                            connection.Add(new Point(x, y + 1));
                        }
                        if (x > 0 && (lines[y][x - 1] == 'L' || lines[y][x - 1] == 'F' || lines[y][x - 1] == '-'))
                        {
                            connection.Add(new Point(x - 1, y));
                        }
                        if (x < lines[y].Length - 1 && (lines[y][x + 1] == '7' || lines[y][x + 1] == 'J' || lines[y][x + 1] == '-'))
                        {
                            connection.Add(new Point(x + 1, y));
                        }
                        break;
                    default:
                        break;
                }

                maze.Add(position, new Pipe(position, c, connection));

            }
        }

        return maze;
    }

    public bool IsPointInsidBorder(List<Point> borderPoints, Point p)
    {
        if (borderPoints.Contains(p))
        {
            // Points on the border are not inside
            return false;
        }

        int intersections = 0;
        int n = borderPoints.Count;

        for (int i = 0; i < n; i++)
        {
            var p1 = borderPoints[i];
            var p2 = borderPoints[(i + 1) % n];
            if ((p1.Y > p.Y) != (p2.Y > p.Y))
            {
                int intersectX = p1.X + (p.Y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y);

                if (intersectX > p.X)
                {
                    intersections++;
                }
            }
        }

        return intersections % 2 != 0;
    }

    private void PrintBorder(string[] lines, IEnumerable<Pipe> borderPipes, IEnumerable<Pipe> enclosedPipes)
    {
        for (var y = 0; y < lines.Length; y++)
        {
            for (var x = 0; x < lines[y].Length; x++)
            {
                if (enclosedPipes.Any(p => p.Position == new Point(x, y)))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (borderPipes.Any(p => p.Position == new Point(x, y)))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.Write(borderPipes.Any(p => p.Position == new Point(x, y)) ? borderPipes.First(p => p.Position == new Point(x, y)).Char : ".");
            }
            Console.WriteLine();
        }
    }
}