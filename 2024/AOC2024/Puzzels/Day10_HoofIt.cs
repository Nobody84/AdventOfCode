﻿namespace AOC2024.Puzzels;

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

public class Day10_HoofIt : PuzzelBase
{
    private int[][] map = new int[0][];

    private record Position(int Y, int X);

    private record HikingTrail(List<Position> Path);
    record Offset(int X, int Y);

    private List<Offset> directionOffsets = new()
    {
        new Offset(-1, 0), // Up
        new Offset(0, 1), // Right
        new Offset(1, 0), // Down
        new Offset(0, -1), // Left
    };


    public Day10_HoofIt()
    : base(10, "Hoof It")
    {
    }

    protected override void PreparePart1(string inputFilePath)
    {
        var lines = File.ReadAllLines(inputFilePath);
        this.map = lines.Select(l => l.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
    }

    protected override object Part1()
    {
        var trailheads = this.map
            .SelectMany((row, y) => row.Select((value, x) => new { value, position = new Position(y, x) }))
            .Where(item => item.value == 9)
            .Select(item => item.position)
            .ToList();

        var scores = new List<int>();
        foreach (var trailhead in trailheads)
        {
            var path = new List<Position> { trailhead };
            var trails = FindHikingTrail(path, trailhead, map[trailhead.Y][trailhead.X]);
           
            scores.Add(trails.DistinctBy(t => t.Path.Last()).Count());
        }

        return scores.Sum();
    }

    protected override object Part2()
    {
        var trailheads = this.map
            .SelectMany((row, y) => row.Select((value, x) => new { value, position = new Position(y, x) }))
            .Where(item => item.value == 9)
            .Select(item => item.position)
            .ToList();

        var scores = new List<int>();
        foreach (var trailhead in trailheads)
        {
            var path = new List<Position> { trailhead };
            var trails = FindHikingTrail(path, trailhead, map[trailhead.Y][trailhead.X]);

            scores.Add(trails.Count());
        }

        return scores.Sum();
    }

    private List<HikingTrail> FindHikingTrail(List<Position> path, Position currentPosition, int currentHeight)
    {
        var hikingTrails = new List<HikingTrail>();
        foreach (var directionOffset in this.directionOffsets)
        {
            var nextPosition = new Position(currentPosition.Y + directionOffset.Y, currentPosition.X + directionOffset.X);
            if (nextPosition.Y < 0 || nextPosition.Y >= this.map.Length || nextPosition.X < 0 || nextPosition.X >= this.map[nextPosition.Y].Length)
            {
                continue;
            }

            var nextHeight = this.map[nextPosition.Y][nextPosition.X];

            if (nextHeight != currentHeight - 1)
            {
                continue;
            }

            if (nextHeight == 0)
            {
                var foundPath = new List<Position>(path);
                foundPath.Add(nextPosition);
                hikingTrails.Add(new HikingTrail(foundPath));
                continue;
            }

            var newPath = new List<Position>(path);
            newPath.Add(nextPosition);
            foreach (var trail in FindHikingTrail(newPath, nextPosition, nextHeight))
            {
                hikingTrails.Add(trail);
            }
        }

        return hikingTrails;
    }

    private static void PrintPath(int[][] map, List<Position> path)
    {
        Console.WriteLine($"Path: {(string.Join(",", path.Select(p => $"({p.Y},{p.X})")))}");
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if(path.Any(p => p.Y == y && p.X == x))
                {
                    ConsoleExtensions.Write(map[y][x], ConsoleColor.Red);
                }
                else
                {
                    Console.Write(map[y][x]);
                }
            }

            Console.WriteLine();
        }
    }
}