using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AOC2024.Puzzels;

public class Day16_ReindeerMaze : PuzzelBase
{
    private Node startNode;
    private Node endNode;
    private Dictionary<Position, Node> nodePositionDict;

    private enum Direction
    {
        North = 0,
        East,
        South,
        West
    }

    private record Node(Dictionary<Direction, Node> NeighborNodes)
    {
        public void AddNeighbor(Direction direction, Node neighborNode)
        {
            if (!NeighborNodes.ContainsKey(direction))
            {
                NeighborNodes[direction] = neighborNode;
            }
        }

        public int Distance { get; set; } = int.MaxValue;
    };

    private record Position(int Y, int X);

    public Day16_ReindeerMaze()
        : base(16, "Reindeer Maze")
    {
    }

    protected override void PreparePart1(string inputFile)
    {
        this.nodePositionDict = new Dictionary<Position, Node>();
        var lines = File.ReadLines(inputFile);
        for (var y = 0; y < lines.Count(); y++)
        {
            var line = lines.ElementAt(y);
            for (var x = 0; x < line.Length; x++)
            {
                var c = line[x];
                if (c == '#')
                {
                    continue;
                }

                var position = new Position(y, x);
                var node = new Node(new Dictionary<Direction, Node>());
                nodePositionDict[position] = node;

                // Check Neighbors
                foreach (var direction in Enum.GetValues<Direction>())
                {
                    var neighborPosition = GetNeighborPosition(position, direction);
                    if (nodePositionDict.TryGetValue(neighborPosition, out var neighborNode))
                    {
                        node.AddNeighbor(direction, neighborNode);
                        neighborNode.AddNeighbor(direction switch
                        {
                            Direction.North => Direction.South,
                            Direction.East => Direction.West,
                            Direction.South => Direction.North,
                            Direction.West => Direction.East,
                            _ => throw new InvalidOperationException()
                        }, node);
                    }
                }

                if (c == 'S')
                {
                    startNode = node;
                    continue;
                }
                if (c == 'E')
                {
                    endNode = node;
                    continue;
                }
            }
        }
    }

    protected override object Part1()
    {
        var nodes = this.nodePositionDict.Values.ToList();

        var visited = new HashSet<Node>();
        var queue = new PriorityQueue<Node, int>();
        var distance = new Dictionary<Node, (int Distance, Direction Direction)>();

        distance[startNode] = (0, Direction.East);
        queue.Enqueue(startNode, 0);

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            if (visited.Contains(currentNode))
            {
                continue;
            }

            visited.Add(currentNode);

            foreach (var neighborNode in currentNode.NeighborNodes)
            {
                var neighbor = neighborNode.Value;
                var newDistance = distance[currentNode].Distance + CalcDistance(distance[currentNode].Direction, neighborNode.Key);
                if (!distance.ContainsKey(neighbor) || newDistance < distance[neighbor].Distance)
                {
                    distance[neighbor] = (newDistance, neighborNode.Key);
                    queue.Enqueue(neighbor, newDistance);
                }
            }
        }

        return distance[endNode].Distance;
    }

    protected override object Part2()
    {
        return 0;
    }

    private static Position GetNeighborPosition(Position position, Direction direction)
    {
        return direction switch
        {
            Direction.North => new Position(position.Y - 1, position.X),
            Direction.East => new Position(position.Y, position.X + 1),
            Direction.South => new Position(position.Y + 1, position.X),
            Direction.West => new Position(position.Y, position.X - 1),
            _ => throw new InvalidOperationException()
        };
    }

    private int CalcDistance(Direction currentDirection, Direction newDirection)
    {
        var rotations = Math.Abs(currentDirection - newDirection);
        rotations = rotations > 2 ? 4 - rotations : rotations;
        return 1 + rotations * 1000;
    }
}