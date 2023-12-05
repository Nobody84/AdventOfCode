namespace AOC2023.Puzzels;

using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using AOC2023.Puzzels.Day5;
using ConsoleTables;

public class Day5_IfYouGiveASeedAFertilizer
{
    private static Regex seedRegex = new Regex(@"^seeds: (?<seeds>\d+(?: *\d+)*)$");
    private static Regex mapNameRegex = new Regex(@"^(?<sourceType>[a-z]+)-to-(?<destinationType>[a-z]+) map:$");
    private static Regex mapValueRegex = new Regex(@"^(?<map>\d+(?: *\d+)*)$");

    private record PackOfSeeds(long FirstSeed, long NumberOfSeeds);

    public long Part1()
    {
        var lines = File.ReadAllLines("Inputs/Day5.txt");

        var seeds = seedRegex.Match(lines[0]).Groups["seeds"].Value.Split(" ").Select(long.Parse).ToList();
        var maps = GetMaps(lines);
        var locationNumbers = this.GetLocationNumbers(seeds, maps);

        return locationNumbers.Min();
    }

    public long Part2()
    {
        var lines = File.ReadAllLines("Inputs/Day5.txt");

        var seedPairs = seedRegex.Match(lines[0]).Groups["seeds"].Value.Split(" ").Select(long.Parse).ToList();
        var packsOfSeeds = Enumerable.Range(0, seedPairs.Count / 2).Select(i => new PackOfSeeds(seedPairs[2 * i], seedPairs[2 * i + 1])).ToList();

        Console.WriteLine(packsOfSeeds.Sum(p => p.NumberOfSeeds));
        var maps = GetMaps(lines);

        var locationNumbers = this.GetLocationNumbers(packsOfSeeds, maps);

        return locationNumbers.Min();
    }

    private List<SourceDestinationMap> GetMaps(string[] lines)
    {
        var maps = new List<SourceDestinationMap>();
        SourceDestinationMap? currentDetectedMap = null;
        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            var mapNameMatch = mapNameRegex.Match(line);
            if (mapNameMatch.Success)
            {
                if (currentDetectedMap != null)
                {
                    maps.Add(currentDetectedMap);
                }

                currentDetectedMap = new SourceDestinationMap(mapNameMatch.Groups["sourceType"].Value, mapNameMatch.Groups["destinationType"].Value);
                continue;
            }

            var mapValueMatch = mapValueRegex.Match(line);
            if (mapValueMatch.Success)
            {
                var mapValues = mapValueMatch.Groups["map"].Value.Split(" ").Select(long.Parse).ToList();
                currentDetectedMap!.AddMapValue(mapValues[1], mapValues[0], mapValues[2]);
            }
        }

        if (currentDetectedMap != null)
        {
            maps.Add(currentDetectedMap);
        }

        return maps;
    }

    private List<long> GetLocationNumbers(List<long> seeds, List<SourceDestinationMap> maps)
    {
        var locationNumbers = new List<long>();
        foreach (var seed in seeds)
        {
            var currentMap = maps.First(m => m.SourceType == "seed");
            var currentValue = seed;
            do
            {
                currentValue = currentMap.Map(currentValue);
                currentMap = maps.FirstOrDefault(m => m.SourceType == currentMap.DestinationType);
            }
            while (currentMap != null);
            locationNumbers.Add(currentValue);
        }

        return locationNumbers;
    }

    private List<long> GetLocationNumbers(List<PackOfSeeds> packsOfSeeds, List<SourceDestinationMap> maps)
    {
        var locationNumbers = new ConcurrentBag<long>();
        var count = 1;
        Parallel.ForEach(packsOfSeeds, (packOfSeeds) =>
        {
            Console.WriteLine($"Pack of Seeds {count}: NumberOfSeeds {packOfSeeds.NumberOfSeeds}");
            long seedCount = 0;
            for (var seed = packOfSeeds.FirstSeed; seed < packOfSeeds.FirstSeed + packOfSeeds.NumberOfSeeds; seed++)
            {
                if (seedCount++ % 1000000 == 0)
                {
                    Console.WriteLine($"{seedCount} / {packOfSeeds.NumberOfSeeds} | {seedCount * 100.0 / packOfSeeds.NumberOfSeeds:00.##}%");
                }

                var currentMap = maps.First(m => m.SourceType == "seed");
                var currentValue = seed;
                do
                {
                    currentValue = currentMap.Map(currentValue);
                    currentMap = maps.FirstOrDefault(m => m.SourceType == currentMap.DestinationType);
                }
                while (currentMap != null);
                locationNumbers.Add(currentValue);
            }
        });

        return locationNumbers.ToList();
    }
}