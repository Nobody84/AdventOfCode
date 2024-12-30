namespace AOC2024.Puzzels;

public class Day18_RamRun : PuzzelBase
{
    private HashSet<Position> allCorruptions;
    private HashSet<Position> corropitions;
    private record Position(int Y, int X);

    private int mazeSize;
    private Position startPosition;
    private Position endPosition;
    private HashSet<Position> visited = new();
    private int shortesDistance = int.MaxValue;
    private List<Position> shortestPath = new();

    private readonly List<(int X, int Y)> directionOffsets = new()
    {
        (0,1),
        (1,0),
        (0,-1),
        (-1,0)
    };

    public Day18_RamRun()
     : base(18, "RAM Run")
    {
    }

    protected override void PreparePart1(string inputFile)
    {
        this.allCorruptions = new();
        foreach (var line in File.ReadLines(inputFile))
        {
            var splits = line.Split(",");
            this.allCorruptions.Add(new Position(int.Parse(splits[1]), int.Parse(splits[0])));
        }
    }

    protected override object Part1()
    {
        this.mazeSize = 71;
        this.startPosition = new Position(0, 0);
        this.endPosition = new Position(this.mazeSize - 1, this.mazeSize - 1);
        this.corropitions = this.allCorruptions.Take(1024).ToHashSet();

        var visited = new HashSet<Position>(); 
        var queue = new PriorityQueue<Position, int>();
        var distance = new Dictionary<Position, int>();

        distance[this.startPosition] = 0;
        queue.Enqueue(this.startPosition, 0);

        while(queue.Count > 0)
        {
            var currentPosition = queue.Dequeue();

            foreach (var direction in directionOffsets)
            {
                var newPosition = new Position(currentPosition.Y + direction.Y, currentPosition.X + direction.X);
                if (newPosition.Y < 0 || newPosition.Y >= this.mazeSize || newPosition.X < 0 || newPosition.X >= this.mazeSize)
                {
                    continue;
                }

                if (this.corropitions.Contains(newPosition))
                {
                    continue;
                }

                var newDistance = distance[currentPosition] + 1;
                if (distance.ContainsKey(newPosition) && newDistance >= distance[newPosition])
                {
                    continue;
                }

                distance[newPosition] = newDistance;
                queue.Enqueue(newPosition, newDistance);
            }
        }

        return distance[this.endPosition];
    }

    protected override object Part2()
    {
        this.mazeSize = 71;
        this.startPosition = new Position(0, 0);
        this.endPosition = new Position(this.mazeSize - 1, this.mazeSize - 1);
        this.corropitions = this.allCorruptions.ToHashSet();

        var queue = new PriorityQueue<Position, int>();
        var distance = new Dictionary<Position, int>();


        // Devide and conquer
        var intervalStart = 1024;
        var intervalEnd = this.allCorruptions.Count;
        do
        {
            var currentValue = intervalStart + (intervalEnd - intervalStart) / 2;
            this.corropitions = this.allCorruptions.Take(currentValue).ToHashSet();

            queue.Clear();
            distance.Clear();
            distance[this.startPosition] = 0;
            queue.Enqueue(this.startPosition, 0);
            while (queue.Count > 0)
            {
                var currentPosition = queue.Dequeue();

                foreach (var direction in directionOffsets)
                {
                    var newPosition = new Position(currentPosition.Y + direction.Y, currentPosition.X + direction.X);
                    if (newPosition.Y < 0 || newPosition.Y >= this.mazeSize || newPosition.X < 0 || newPosition.X >= this.mazeSize)
                    {
                        continue;
                    }

                    if (this.corropitions.Contains(newPosition))
                    {
                        continue;
                    }

                    var newDistance = distance[currentPosition] + 1;
                    if (distance.ContainsKey(newPosition) && newDistance >= distance[newPosition])
                    {
                        continue;
                    }

                    distance[newPosition] = newDistance;
                    queue.Enqueue(newPosition, newDistance);
                }
            }

            if (distance.ContainsKey(this.endPosition))
            {
                intervalStart = currentValue;
            }
            else
            {
                intervalEnd = currentValue;
            }
        }
        while (intervalEnd - intervalStart > 1);

        var corruption = this.allCorruptions.ElementAt(intervalStart);
        return $"{corruption.X},{corruption.Y}";
        
    }

    private void Move(Position currentPosition, int currentDistance, HashSet<Position> currentPath)
    {
        //for (var y = 0; y < this.mazeSize; y++)
        //{
        //    for (var x = 0; x < this.mazeSize; x++)
        //    {
        //        var pos = new Position(y, x);
        //        if (this.corropitions.Contains(pos))
        //        {
        //            Console.Write("#");
        //        }
        //        else if (currentPath.Contains(pos))
        //        {
        //            ConsoleExtensions.Write("O", ConsoleColor.Green);
        //        }
        //        else
        //        {
        //            Console.Write(".");
        //        }
        //    }

        //    Console.WriteLine();
        //}
        //Console.WriteLine();
        //Console.ReadKey();

        if (currentPosition == this.endPosition)
        {
            if (currentDistance < this.shortesDistance)
            {
                this.shortesDistance = currentDistance;
            }

            return;
        }

        foreach (var direction in directionOffsets)
        {
            var newPosition = new Position(currentPosition.Y + direction.Y, currentPosition.X + direction.X);
            if (newPosition.Y < 0 || newPosition.Y >= this.mazeSize || newPosition.X < 0 || newPosition.X >= this.mazeSize)
            {
                continue;
            }

            if (currentPath.Contains(newPosition))
            {
                continue;
            }

            if (this.corropitions.Contains(newPosition))
            {
                continue;
            }

            Move(newPosition, currentDistance  + 1, new HashSet<Position>(currentPath) { newPosition });
        }
    }
}
