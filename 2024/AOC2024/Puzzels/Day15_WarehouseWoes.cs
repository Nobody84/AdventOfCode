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
        BoxLeft,
        BoxRight,
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

        //PrintWarehouse(this.warehouse, this.robotPosition);
        return sumOfGpsCoordinates;
    }

    protected override void PreparePart2(string inputFile)
    {
        this.movements.Clear();
        var lines = File.ReadLines(inputFile);

        var warehouseRows = new List<Type[]>();
        this.robotPosition = new Position(0, 0);

        for (var y = 0; y < lines.Count(); y++)
        {
            var line = lines.ElementAt(y);
            var warehouseRow = new Type[line.Length * 2];
            if (line.StartsWith('#'))
            {
                for (int x = 0, k = 0; x < line.Length; x++, k += 2)
                {
                    var c = line[x];
                    switch (c)
                    {
                        case '#':
                            warehouseRow[k] = Type.Wall;
                            warehouseRow[k + 1] = Type.Wall;
                            break;
                        case '.':
                            warehouseRow[k] = Type.Free;
                            warehouseRow[k + 1] = Type.Free;
                            break;
                        case 'O':
                            warehouseRow[k] = Type.BoxLeft;
                            warehouseRow[k + 1] = Type.BoxRight;
                            break;
                        case '@':
                            warehouseRow[k] = Type.Free;
                            warehouseRow[k + 1] = Type.Free;
                            this.robotPosition = new Position(y, k);
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

    protected override object Part2()
    {
        PrintWarehouse(this.warehouse, this.robotPosition, null);

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

                var nextRobotPosition = nextPosition;
                if (nextType == Type.BoxLeft || nextType == Type.BoxRight)
                {
                    var blocked = false;
                    if (movement == Direction.Down || movement == Direction.Up)
                    {
                        // Up/Down
                        var nextLeftPosition = nextType == Type.BoxLeft ? new Position(nextPosition.Y, nextPosition.X) : new Position(nextPosition.Y, nextPosition.X - 1);
                        var nextRightPosition = nextType == Type.BoxLeft ? new Position(nextPosition.Y, nextPosition.X + 1) : new Position(nextPosition.Y, nextPosition.X);
                        blocked = IsBlocked(nextLeftPosition, nextRightPosition, movement);
                        if (!blocked)
                        {
                            Move(nextLeftPosition, nextRightPosition, movement);
                            this.robotPosition = nextPosition;
                        }

                    }
                    else
                    {
                        // Left/Right
                        while (true)
                        {
                            nextPosition = GetNextPosition(nextPosition, movement);
                            nextType = this.warehouse[nextPosition.Y][nextPosition.X];

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

                        if (!blocked)
                        {
                            this.robotPosition = nextRobotPosition;
                            var start = Math.Min(nextRobotPosition.X, nextPosition.X);
                            var end = Math.Max(nextRobotPosition.X, nextPosition.X);
                            if (movement == Direction.Left)
                            {
                                for (var i = start; i < end; i++)
                                {
                                    this.warehouse[nextRobotPosition.Y][i] = this.warehouse[nextRobotPosition.Y][i + 1];
                                }

                                this.warehouse[nextRobotPosition.Y][end] = Type.Free;
                            }
                            else
                            {
                                for (var i = end; i > start; i--)
                                {
                                    this.warehouse[nextRobotPosition.Y][i] = this.warehouse[nextRobotPosition.Y][i - 1];
                                }

                                this.warehouse[nextRobotPosition.Y][start] = Type.Free;
                            }
                        }
                    }
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
                    case Type.BoxLeft:
                        ConsoleExtensions.Write("[", ConsoleColor.Cyan);
                        break;
                    case Type.BoxRight:
                        ConsoleExtensions.Write("]", ConsoleColor.Cyan);
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
                if (value == Type.Box || value == Type.BoxLeft)
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

    private bool IsBlocked(Position leftBoxPart, Position rightBoxPart, Direction movement)
    {
        var nextLeftPosition = GetNextPosition(leftBoxPart, movement);
        var nextRightPosition = GetNextPosition(rightBoxPart, movement);
        var nextLeftType = this.warehouse[nextLeftPosition.Y][nextLeftPosition.X];
        var nextRightType = this.warehouse[nextRightPosition.Y][nextRightPosition.X];


        if (nextLeftType == Type.Wall || nextRightType == Type.Wall)
        {
            return true;
        }

        if (nextLeftType == Type.Free && nextRightType == Type.Free)
        {
            return false;
        }

        if (nextLeftType == Type.BoxLeft && nextRightType == Type.BoxRight)
        {
            return IsBlocked(nextLeftPosition, nextRightPosition, movement);
        }

        var blocked = false;
        if (nextLeftType == Type.BoxRight)
        {
            blocked |= IsBlocked(new Position(nextLeftPosition.Y, nextLeftPosition.X - 1), nextLeftPosition, movement);
        }

        if (nextRightType == Type.BoxLeft)
        {
            blocked |= IsBlocked(nextRightPosition, new Position(nextRightPosition.Y, nextRightPosition.X + 1), movement);
        }

        return blocked;
    }

    private void Move(Position leftBoxPart, Position rightBoxPart, Direction movement)
    {
        var nextLeftPosition = GetNextPosition(leftBoxPart, movement);
        var nextRightPosition = GetNextPosition(rightBoxPart, movement);
        var nextLeftType = this.warehouse[nextLeftPosition.Y][nextLeftPosition.X];
        var nextRightType = this.warehouse[nextRightPosition.Y][nextRightPosition.X];

        if (nextLeftType == Type.Free && nextRightType == Type.Free)
        {
            this.warehouse[nextLeftPosition.Y][nextLeftPosition.X] = Type.BoxLeft;
            this.warehouse[nextRightPosition.Y][nextRightPosition.X] = Type.BoxRight;
            this.warehouse[leftBoxPart.Y][leftBoxPart.X] = Type.Free;
            this.warehouse[rightBoxPart.Y][rightBoxPart.X] = Type.Free;
            return;
        }

        if (nextLeftType == Type.BoxLeft && nextRightType == Type.BoxRight)
        {
           Move(nextLeftPosition, nextRightPosition, movement);
        }

        if (nextLeftType == Type.BoxRight)
        {
            Move(new Position(nextLeftPosition.Y, nextLeftPosition.X - 1), nextLeftPosition, movement);
        }

        if (nextRightType == Type.BoxLeft)
        {
            Move(nextRightPosition, new Position(nextRightPosition.Y, nextRightPosition.X + 1), movement);
        }

        this.warehouse[nextLeftPosition.Y][nextLeftPosition.X] = Type.BoxLeft;
        this.warehouse[nextRightPosition.Y][nextRightPosition.X] = Type.BoxRight;
        this.warehouse[leftBoxPart.Y][leftBoxPart.X] = Type.Free;
        this.warehouse[rightBoxPart.Y][rightBoxPart.X] = Type.Free;
    }
}