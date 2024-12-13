namespace AOC2024.Puzzels;

using System.Formats.Asn1;
using System.Threading.Tasks.Dataflow;
using System.Xml;

public class Day12_GardenGroups : PuzzelBase
{
    private Plant[][] garden;
    private static int MaxY;
    private static int MaxX;

    private class Plant(char plantType, int y, int x)
    {
        public char PlantType { get; init; } = plantType;

        public Position Position { get; init; } = new(y, x);

        public bool Visited { get; set; } = false;

        public List<Fence> Fences { get; set; } = new();
    }

    private class FenceSegment(Direction direction, int start, int level)
    {
        public Direction Direction { get; init; } = direction;

        public int Start { get; set; } = start;

        public int End { get; set; } = start;

        public int Level { get; init; } = level;
    }

    private record Fence(Direction Direction);

    private record Position(int Y, int X);

    record Offset(int Y, int X);

    private enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }


    public Day12_GardenGroups()
        : base(12, "Garden Groups")
    {
    }

    protected override void PreparePart1(string inputFile)
    {
        var lines = File.ReadLines(inputFile).ToArray();

        MaxY = lines.Count();
        MaxX = lines.First().Length;

        this.garden = new Plant[MaxY][];
        for (var y = 0; y < lines.Count(); y++)
        {
            var line = lines[y];
            this.garden[y] = new Plant[MaxX];
            for (var x = 0; x < line.Length; x++)
            {
                this.garden[y][x] = new Plant(line[x], y, x);
            }
        }
    }

    protected override object Part1()
    {
        var groups = new List<List<Plant>>();
        for (var y = 0; y < MaxY; y++)
        {
            for (var x = 0; x < MaxX; x++)
            {
                var plant = this.garden[y][x];
                if (plant.Visited)
                {
                    continue;
                }

                var plantGroup = new List<Plant>();
                VisitPlant(ref this.garden, ref plantGroup, plant);
                groups.Add(plantGroup);
            }
        }

        return groups.Select(g => g.Select(p => p.Fences.Count).Sum() * g.Count).Sum();
    }

    protected override object Part2()
    {
        var colors = new List<ConsoleColor> { ConsoleColor.Red, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Yellow, ConsoleColor.Magenta, ConsoleColor.Cyan, ConsoleColor.White, ConsoleColor.Gray, ConsoleColor.DarkRed, ConsoleColor.DarkGreen, ConsoleColor.DarkBlue, ConsoleColor.DarkYellow, ConsoleColor.DarkMagenta, ConsoleColor.DarkCyan, ConsoleColor.DarkGray };

        var groups = new List<List<Plant>>();
        for (var y = 0; y < MaxY; y++)
        {
            for (var x = 0; x < MaxX; x++)
            {
                var plant = this.garden[y][x];
                if (plant.Visited)
                {
                    continue;
                }

                var plantGroup = new List<Plant>();
                VisitPlant(ref this.garden, ref plantGroup, plant);
                groups.Add(plantGroup);
            }
        }

        //for (var y = 0; y < MaxY; y++)
        //{
        //    for (var x = 0; x < MaxX; x++)
        //    {
        //        var group = groups.FirstOrDefault(g => g.Any(p => p.Position.Y == y && p.Position.X == x));
        //        if (group == null)
        //        {
        //            Console.Write(this.garden[y][x].PlantType);
        //        }
        //        else
        //        {
        //            ConsoleExtensions.Write(this.garden[y][x].PlantType, colors[groups.IndexOf(group) % colors.Count]);
        //        }
        //    }

        //    Console.WriteLine();
        //}


        var totalCost = 0;
        foreach (var group in groups)
        {
            var minX = group.Min(p => p.Position.X);
            var maxX = group.Max(p => p.Position.X);
            var minY = group.Min(p => p.Position.Y);
            var maxY = group.Max(p => p.Position.Y);

            var fenceSegments = new List<FenceSegment>();
            foreach (var plant in group)
            {
                foreach (var fence in plant.Fences)
                {
                    var position = fence.Direction switch
                    {
                        Direction.Up => plant.Position.X,
                        Direction.Down => plant.Position.X,
                        Direction.Right => plant.Position.Y,
                        Direction.Left => plant.Position.Y,
                        _ => throw new NotImplementedException(),
                    };

                    var level = fence.Direction switch
                    {
                        Direction.Up => plant.Position.Y,
                        Direction.Down => plant.Position.Y,
                        Direction.Right => plant.Position.X,
                        Direction.Left => plant.Position.X,
                        _ => throw new NotImplementedException(),
                    };
                    // find a neighbor fence segment with the same direction
                    var neighborFenceSegment = fenceSegments.FirstOrDefault(fs =>
                            fs.Direction == fence.Direction &&
                            fs.Level == level &&
                            (fs.Start - 1 == position || fs.End + 1 == position));
                    if (neighborFenceSegment != null)
                    {
                        if (neighborFenceSegment.Start - 1 == position)
                        {
                            neighborFenceSegment.Start = position;
                        }
                        else
                        {
                            neighborFenceSegment.End = position;
                        }
                    }
                    else
                    {
                        fenceSegments.Add(new FenceSegment(fence.Direction, position, level));
                    }
                }
            }

            // Consolidate fence segments
            var fencesToRemoveIndexes = new List<int>();
            var tempFenceSegments = new List<FenceSegment>(fenceSegments);
            for (var i = 0; i < tempFenceSegments.Count; i++)
            {
                var fenceSegment = tempFenceSegments[i];
                var neighbourFenceSegment = fenceSegments.FirstOrDefault(fs =>
                    fs != fenceSegment &&
                    fs.Direction == fenceSegment.Direction &&
                    fs.Level == fenceSegment.Level &&
                    (fs.Start - 1 == fenceSegment.End || fs.End + 1 == fenceSegment.Start));

                if (neighbourFenceSegment != null)
                {
                    fenceSegments.Remove(fenceSegment);
                }
            }

            totalCost += group.Count * fenceSegments.Count;
        }

        return totalCost;
    }

    private static void VisitPlant(ref Plant[][] garden, ref List<Plant> plantGroup, Plant plant)
    {
        plantGroup.Add(plant);
        plant.Visited = true;
        foreach (Direction direction in Enum.GetValues(typeof(Direction)))
        {
            var offset = GetOffset(direction);
            if (plant.Position.Y + offset.Y < 0 || plant.Position.Y + offset.Y >= MaxY || plant.Position.X + offset.X < 0 || plant.Position.X + offset.X >= MaxX)
            {
                plant.Fences.Add(new Fence(direction));
                continue;
            }
            else
            {
                var nextPlant = garden[plant.Position.Y + offset.Y][plant.Position.X + offset.X];
                if (nextPlant.PlantType == plant.PlantType)
                {
                    if (!nextPlant.Visited)
                    {
                        VisitPlant(ref garden, ref plantGroup, nextPlant);
                    }
                }
                else
                {
                    plant.Fences.Add(new Fence(direction));
                }
            }
        }
    }

    private static Offset GetOffset(Direction direction)
    {
        return direction switch
        {
            Direction.Up => new Offset(-1, 0),
            Direction.Right => new Offset(0, 1),
            Direction.Down => new Offset(1, 0),
            Direction.Left => new Offset(0, -1),
            _ => throw new NotImplementedException(),
        };
    }
}