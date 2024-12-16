using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AOC2024.Puzzels;

public class Day15_WarehouseWoes : PuzzelBase
{
    private Type[][] warehouse;
    private Position robotPosition;
    private List<Direction> movements = new();

    private enum Type
    {
        Wall,
        Box,
        Free,
    }

    private enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    private record Position(int Y, int X);

    public Day15_WarehouseWoes()
        : base(15, "Warehouse Woes")
    {
    }

    protected override void PreparePart1(string inputFile)
    {
        var lines = File.ReadLines(inputFile);

        var warehouseRows = new List<Type[]>();
        this.robotPosition = new Position(0, 0);

        for (var y = 0; y < lines.Count(); y++)
        {
            var line = lines.ElementAt(y);
            var warehouseRow = new Type[line.Length];
            if (line.StartsWith('#'))
            {
                for (var x = 0; x < line.Length; x++)
                {
                    var c = line[x];
                    switch (c)
                    {
                        case '#':
                            warehouseRow[x] = Type.Wall;
                            break;
                        case '.':
                            warehouseRow[x] = Type.Free;
                            break;
                        case 'O':
                            warehouseRow[x] = Type.Box;
                            break;
                        case '@':
                            warehouseRow[x] = Type.Free;
                            this.robotPosition = new Position(y, x);
                            break;
                    }
                }

                warehouseRows.Add(warehouseRow);

                continue;
            }

            if (string.IsNullOrEmpty(line))
            {
                this.warehouse = warehouseRows.ToArray();
                continue;
            }

            for (var x = 0; x < line.Length; x++)
            {
                var c = line[x];
                switch (c)
                {
                    case '<':
                        this.movements.Add(Direction.Left);
                        break;
                    case '^':
                        this.movements.Add(Direction.Up);
                        break;
                    case '>':
                        this.movements.Add(Direction.Right);
                        break;
                    case 'v':
                        this.movements.Add(Direction.Down);
                        break;
                }
            }
        }
    }    

    protected override object Part1()
    {
        foreach (var movement in this.movements)
        {
            try
            {
                var nextPosition = GetNextPosition(this.robotPosition, movement);
                var nextType = this.warehouse[nextPosition.Y][nextPosition.X];
                if (nextType == Type.Wall)
                {
                    continue;
                }

                if (nextType == Type.Free)
                {
                    this.robotPosition = nextPosition;
                    continue;
                }

                if (nextType == Type.Box)
                {
                    var boxesInLine = 1;
                    var blocked = false;
                    var nextRobotPosition = nextPosition;
                    while (true)
                    {
                        nextPosition = GetNextPosition(nextPosition, movement);
                        nextType = this.warehouse[nextPosition.Y][nextPosition.X];
                        if (nextType == Type.Box)
                        {
                            boxesInLine++;
                        }

                        if (nextType == Type.Wall)
                        {
                            blocked = true;
                            break;
                        }

                        if (nextType == Type.Free)
                        {
                            break;
                        }
                    }

                    if (blocked)
                    {
                        continue;
                    }

                    this.robotPosition = nextRobotPosition;
                    this.warehouse[nextRobotPosition.Y][nextRobotPosition.X] = Type.Free;
                    this.warehouse[nextPosition.Y][nextPosition.X] = Type.Box;
                }
            }
            finally
            {
                //PrintWarehouse(this.warehouse, this.robotPosition, movement);
            }

        }

        var sumOfGpsCoordinates = GetSumOfGpsCoordinates(this.warehouse);

        PrintWarehouse(this.warehouse, this.robotPosition);
        return sumOfGpsCoordinates;
    }

    protected override object Part2()
    {
        return 0;
    }

    private static void PrintWarehouse(Type[][] warehouse, Position roboterPosition, Direction? movement = null)
    {
        for (var y = 0; y < warehouse.Count(); y++)
        {
            var row = warehouse[y];
            for (var x = 0; x < row.Length; x++)
            {
                var value = row[x];
                switch (value)
                {
                    case Type.Wall:
                        Console.Write("#");
                        break;

                    case Type.Box:
                        ConsoleExtensions.Write("O", ConsoleColor.Cyan);
                        break;
                    case Type.Free:
                        if (roboterPosition.X == x && roboterPosition.Y == y)
                        {
                            ConsoleExtensions.Write("@", ConsoleColor.Green);
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                        break;
                }
            }

            if (movement != null && y == 0)
            {
                ConsoleExtensions.Write($" {movement}", ConsoleColor.DarkYellow);
            }
            Console.WriteLine();
        }
    }

    private object GetSumOfGpsCoordinates(Type[][] warehouse)
    {
        var sum = 0;
        for (var y = 0; y < warehouse.Count(); y++)
        {
            var row = warehouse[y];
            for (var x = 0; x < row.Length; x++)
            {
                var value = row[x];
                if (value == Type.Box)
                {
                    sum += y * 100 + x;
                }
            }
        }

        return sum;
    }

    private Position GetNextPosition(Position robotPosition, Direction movement)
    {
        return movement switch
        {
            Direction.Up => new Position(robotPosition.Y - 1, robotPosition.X),
            Direction.Right => new Position(robotPosition.Y, robotPosition.X + 1),
            Direction.Down => new Position(robotPosition.Y + 1, robotPosition.X),
            Direction.Left => new Position(robotPosition.Y, robotPosition.X - 1),
            _ => throw new NotImplementedException()
        };
    }
}