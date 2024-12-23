﻿namespace AOC2024.Puzzels;

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

public class Day08_ResonantCollinearity : PuzzelBase
{
    private readonly Regex antennaRegex = new(@"([^.]{1})");
    private List<IGrouping<char, Antenna>> antennaGroups = new();
    private GridDimension gridDimension;

    private record Antenna(int Y, int X, char Type);
    private record GridDimension(int Height, int Width);
    private record ResonancePoint(int Y, int X);

    public Day08_ResonantCollinearity()
        : base(8, "Resonant Collinearity")
    {
    }

    protected override void PreparePart1(string inputFilePath)
    {
        this.antennaGroups.Clear();

        var inputLines = File.ReadLines(inputFilePath);
        this.antennaGroups = inputLines
            .SelectMany((line, y) => antennaRegex.Matches(line).Select(m => new Antenna(y, m.Index, m.Value[0])))
            .GroupBy(a => a.Type).ToList();

        this.gridDimension = new GridDimension(inputLines.Count(), inputLines.First().Length);
    }

    protected override object Part1()
    {
        var resonancePoints = FindResonancePoints(this.gridDimension, this.antennaGroups, 1, 1);

        //PrintGrid(gridDimension, resonancePoints, antennaGroups.SelectMany(g => g).ToList());
        return resonancePoints.Count();
    }

    protected override void PreparePart2(string inputFilePath)
    {
        this.PreparePart1(inputFilePath);
    }

    protected override object Part2()
    {
        var resonancePoints = FindResonancePoints(this.gridDimension, this.antennaGroups, 0, -1);

        //PrintGrid(gridDimension, resonancePoints, antennaGroups.SelectMany(g => g).ToList());
        return resonancePoints.Count();
    }

    private static void PrintGrid(GridDimension gridDimension, List<ResonancePoint> resonancePoints, List<Antenna> antennas)
    {
        for (var y = 0; y < gridDimension.Height; y++)
        {
            for (var x = 0; x < gridDimension.Width; x++)
            {
                var antenna = antennas.FirstOrDefault(a => a.Y == y && a.X == x);
                var resonancePoint = resonancePoints.FirstOrDefault(rp => rp.Y == y && rp.X == x);
                if (antenna != null)
                {
                    ConsoleExtensions.Write(antenna.Type, ConsoleColor.Green);
                }
                else if (resonancePoint != null)
                {
                    ConsoleExtensions.Write('#', ConsoleColor.Red);
                }
                else
                {
                    Console.Write('.');
                }
            }

            Console.WriteLine();
        }
    }

    private static void ExtractCoordinates((Antenna a1, Antenna a2) antennaCombination, out int x1, out int y1, out int x2, out int y2)
    {
        x1 = antennaCombination.a1.X;
        y1 = antennaCombination.a1.Y;
        x2 = antennaCombination.a2.X;
        y2 = antennaCombination.a2.Y;
    }

    private static List<ResonancePoint> FindResonancePoints(GridDimension gridDimension, List<IGrouping<char, Antenna>> antennaGroups, int startStep, int maxSteps)
    {
        List<ResonancePoint> resonancePoints = new();
        foreach (var antennaGroup in antennaGroups)
        {
            var antennaType = antennaGroup.Key;
            var antennaCombinations = antennaGroup.SelectMany((a, i) => antennaGroup.Skip(i + 1), (a1, a2) => (a1, a2));

            foreach (var antennaCombination in antennaCombinations)
            {
                ExtractCoordinates(antennaCombination, out int x1, out int y1, out int x2, out int y2);
                var step = startStep;
                var dY = y2 - y1;
                var dX = x2 - x1;
                while (true)
                {
                    var newResonancePoint1 = new ResonancePoint(y2 + step * dY, x2 + step * dX);

                    // Check if outside the grid
                    if (newResonancePoint1.Y < 0 || newResonancePoint1.Y >= gridDimension.Height ||
                        newResonancePoint1.X < 0 || newResonancePoint1.X >= gridDimension.Width)
                    {
                        break;
                    }

                    resonancePoints.Add(newResonancePoint1);
                    step++;

                    if (maxSteps > 0 && step - startStep >= maxSteps)
                    {
                        break;
                    }
                }

                step = startStep;
                while (true)
                {
                    var newResonancePoint1 = new ResonancePoint(y1 - step * dY, x1 - step * dX);

                    // Check if outside the grid
                    if (newResonancePoint1.Y < 0 || newResonancePoint1.Y >= gridDimension.Height ||
                        newResonancePoint1.X < 0 || newResonancePoint1.X >= gridDimension.Width)
                    {
                        break;
                    }

                    resonancePoints.Add(newResonancePoint1);
                    step++;

                    if (maxSteps > 0 && step - startStep >= maxSteps)
                    {
                        break;
                    }
                }
            }
        }

        return resonancePoints.Distinct().ToList();
    }
}