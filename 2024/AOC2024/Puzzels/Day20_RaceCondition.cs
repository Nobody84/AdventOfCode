using System.Text.RegularExpressions;

namespace AOC2024.Puzzels;

public class Day20_RaceCondition : PuzzelBase
{
    private Tile[][] maze;
    private Position startPosition;
    private Position endPosition;

    private class Tile(bool isWall)
    {
        public bool IsWall { get; set; } = isWall;

        public bool IsVisited { get; set; } = false;

        public int Distance { get; set; } = 0;
    }

    private record Position(int Y, int X);
    private record Offset(int Y, int X);

    private record CheatResult(Position StartPosition, Position EndPosition, int Saving);

    private readonly List<Offset> directionOffsets = new()
    {
        new Offset(0, 1),
        new Offset(1, 0),
        new Offset(0, -1),
        new Offset(-1, 0)
    };

    public Day20_RaceCondition()
     : base(20, "Race Condition")
    {
    }

    protected override void PreparePart1(string inputFile)
    {
        var lines = File.ReadAllLines(inputFile);

        this.maze = new Tile[lines.Length][];
        for (int y = 0; y < lines.Length; y++)
        {
            this.maze[y] = new Tile[lines[y].Length];
            for (int x = 0; x < lines[y].Length; x++)
            {
                this.maze[y][x] = new Tile(lines[y][x] == '#');

                if (lines[y][x] == 'S')
                {
                    this.startPosition = new Position(y, x);
                }
                else if (lines[y][x] == 'E')
                {
                    this.endPosition = new Position(y, x);
                }
            }
        }
    }

    protected override object Part1()
    {
        List<Position> path = GetPath();
        List<CheatResult> cheatResults = SearchForPosisibleCheats(path, 2);

        return cheatResults.Count(cr => cr.Saving >= 100);
    }

    protected override object Part2()
    {
        List<Position> path = GetPath();
        List<CheatResult> cheatResults = SearchForPosisibleCheats(path, 20);

        foreach (var cheatResultGroup in cheatResults.GroupBy(cr => cr.Saving).OrderBy(g => g.Key).Where(g => g.Key >= 50))
        {
            Console.WriteLine($"There are {(cheatResultGroup.Count() == 1 ? "one cheat" : $"{cheatResultGroup.Count()} cheats")} that save {cheatResultGroup.Key} picoseconds.");
        }

        return cheatResults.Count(cr => cr.Saving >= 50);
    }

    private List<Position> GetPath()
    {
        var path = new List<Position>();

        // find the path
        var currentPosition = this.startPosition;
        this.maze[currentPosition.Y][currentPosition.X].IsVisited = true;
        path.Add(this.startPosition);
        while (true)
        {
            // There is only on path to the end, meaning there is only one tile that has not been visited from the current position
            foreach (var directionOffset in this.directionOffsets)
            {
                var newPosition = new Position(currentPosition.Y + directionOffset.Y, currentPosition.X + directionOffset.X);
                if (newPosition.Y < 0 || newPosition.Y >= this.maze.Length || newPosition.X < 0 || newPosition.X >= this.maze[0].Length)
                {
                    continue;
                }

                if (this.maze[newPosition.Y][newPosition.X].IsWall || this.maze[newPosition.Y][newPosition.X].IsVisited)
                {
                    continue;
                }

                this.maze[newPosition.Y][newPosition.X].IsVisited = true;
                this.maze[newPosition.Y][newPosition.X].Distance = this.maze[currentPosition.Y][currentPosition.X].Distance + 1;
                path.Add(newPosition);
                currentPosition = newPosition;
                break;
            }

            if (currentPosition.Equals(this.endPosition))
            {
                break;
            }
        }

        return path;
    }

    private List<CheatResult> SearchForPosisibleCheats(List<Position> path, int cheatLength)
    {
        // Check for posible cheatings
        var cheatResults = new List<CheatResult>();
        foreach (var position in path)
        {

            foreach (var offset in this.directionOffsets)
            {
                for (var i = 1; i <= cheatLength; i++)
                {
                    var newPosition = new Position(position.Y + offset.Y * i, position.X + offset.X * i);
                    if (newPosition.Y < 0 || newPosition.Y >= this.maze.Length || newPosition.X < 0 || newPosition.X >= this.maze[0].Length)
                    {
                        continue;
                    }

                    if (this.maze[newPosition.Y][newPosition.X].IsWall)
                    {
                        continue;
                    }

                    if (this.maze[newPosition.Y][newPosition.X].Distance < this.maze[position.Y][position.X].Distance)
                    {
                        continue;
                    }

                    var saving = this.maze[newPosition.Y][newPosition.X].Distance - this.maze[position.Y][position.X].Distance - i;

                    if (saving > 0)
                    {
                        cheatResults.Add(new CheatResult(position, newPosition, saving));                        
                    }
                }
            }
        }

        return cheatResults;
    }

    private static void PrintMaze(Tile[][] maze, Position startPosition, Position endPosition, CheatResult cheatResult)
    {
        for (int y = 0; y < maze.Length; y++)
        {
            for (int x = 0; x < maze[y].Length; x++)
            {
                if (maze[y][x].IsWall)
                {
                    Console.Write("#");
                }
                else if (cheatResult.StartPosition.Y == y && cheatResult.StartPosition.X == x)
                {
                    ConsoleExtensions.Write("C", ConsoleColor.Green);
                }
                else if (cheatResult.EndPosition.Y == y && cheatResult.EndPosition.X == x)
                {
                    ConsoleExtensions.Write("C", ConsoleColor.Red);
                }
                else if (startPosition.Y == y && startPosition.X == x)
                {
                    Console.Write("S");
                }
                else if (endPosition.Y == y && endPosition.X == x)
                {
                    Console.Write("E");
                }
                else
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine($"Saving: {cheatResult.Saving}");
    }
}