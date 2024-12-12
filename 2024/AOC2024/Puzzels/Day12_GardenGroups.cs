namespace AOC2024.Puzzels;

public class Day12_GardenGroups : PuzzelBase
{
    private Plant[][] garden;
    private static int MaxY;
    private static int MaxX;

    private class Plant(char plantType, int y, int x)
    {
        public char PlantType { get; init; } = plantType;

        public int Y { get; init; } = y;

        public int X { get; init; } = x;

        public bool Visited { get; set; } = false;

        public int Fences { get; set; } = 0;
    }

    record Offset(int X, int Y);

    private static List<Offset> DirectionOffsets = new()
    {
        new Offset(-1, 0), // Up
        new Offset(0, 1), // Right
        new Offset(1, 0), // Down
        new Offset(0, -1), // Left
    };


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

        return groups.Select(g => g.Select(p => p.Fences).Sum() * g.Count).Sum();
    }

    private static void VisitPlant(ref Plant[][] garden, ref List<Plant> plantGroup, Plant plant)
    {
        plantGroup.Add(plant);
        plant.Visited = true;
        foreach (var offset in DirectionOffsets)
        {
            if (plant.Y + offset.Y < 0 || plant.Y + offset.Y >= MaxY || plant.X + offset.X < 0 || plant.X + offset.X >= MaxX)
            {
                plant.Fences++;
                continue;
            }
            else
            {
                var nextPlant = garden[plant.Y + offset.Y][plant.X + offset.X];
                if (nextPlant.PlantType == plant.PlantType)
                {
                    if (!nextPlant.Visited)
                    {
                        VisitPlant(ref garden, ref plantGroup, nextPlant);
                    }
                }
                else
                {
                    plant.Fences++;
                }
            }
        }
    }

    protected override object Part2()
    {
        return 0;
    }
}