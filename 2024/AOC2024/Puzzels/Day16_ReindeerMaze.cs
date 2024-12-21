using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AOC2024.Puzzels;

public class Day16_ReindeerMaze : PuzzelBase
{
    private bool[][] maze;
    private Node startNode;
    private Node endNode;
    private Dictionary<Position, Node> nodePositionDict;

    private int shortestDistance = int.MaxValue;
    private List<Path> shortestPathes = new();

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

        public Position Position { get; set; } = new Position(0, 0);

        public int Distance { get; set; } = 0;
    };

    private record Path(int Distance, HashSet<Node> Nodes);

    private record Position(int Y, int X);

    public Day16_ReindeerMaze()
        : base(16, "Reindeer Maze")
    {
    }

    protected override void PreparePart1(string inputFile)
    {
        this.nodePositionDict = new Dictionary<Position, Node>();
        var lines = File.ReadLines(inputFile);
        this.maze = new bool[lines.Count()][];
        for (var y = 0; y < lines.Count(); y++)
        {
            this.maze[y] = Enumerable.Repeat(false, lines.ElementAt(y).Length).ToArray();
            var line = lines.ElementAt(y);
            for (var x = 0; x < line.Length; x++)
            {
                var c = line[x];
                if (c == '#')
                {
                    this.maze[y][x] = true;
                    continue;
                }

                var position = new Position(y, x);
                var node = new Node(new Dictionary<Direction, Node>());
                node.Position = position;
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
        var nodes = this.nodePositionDict.Values.ToList();
        this.shortestDistance = 65436; // From part one

        this.Move(new Path(0, new HashSet<Node> { this.startNode }), this.startNode, Direction.East);

        var distincedNodes = this.shortestPathes.SelectMany(n => n.Nodes).DistinctBy(n => n.Position).ToList();
        //for (var y = 0; y < this.maze.Length; y++)
        //{
        //    for (var x = 0; x < this.maze[y].Length; x++)
        //    {
        //        if (distincedNodes.Any(n => n.Position.Y == y && n.Position.X == x))
        //        {
        //            ConsoleExtensions.Write("O", ConsoleColor.Cyan);
        //        }
        //        else if (this.maze[y][x])
        //        {
        //            Console.Write("#");
        //        }
        //        else
        //        {
        //            Console.Write(".");
        //        }
        //    }
        //    Console.WriteLine();
        //}


        return distincedNodes.Count();
    }

    private void Move(Path currentPath, Node currentNode, Direction currentDirection)
    {
        foreach (var neigbor in currentNode.NeighborNodes.Where(n => !currentPath.Nodes.Contains(n.Value)))
        {
            var distanceToNeighbor = CalcDistance(currentDirection, neigbor.Key);
            if (currentPath.Distance + distanceToNeighbor > this.shortestDistance)
            {
                continue;
            }

            var newPath = new Path(currentPath.Distance + distanceToNeighbor, new HashSet<Node>(currentPath.Nodes) { neigbor.Value });
            if (neigbor.Value == this.endNode)
            {
                if (newPath.Distance < this.shortestDistance)
                {
                    this.shortestDistance = newPath.Distance;
                    this.shortestPathes.Clear();
                }

                if (newPath.Distance == this.shortestDistance)
                {
                    this.shortestPathes.Add(newPath);
                }
            }
            else
            {
                this.Move(newPath, neigbor.Value, neigbor.Key);
            }
        }
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