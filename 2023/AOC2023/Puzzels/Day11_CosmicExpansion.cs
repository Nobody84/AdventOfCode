namespace AOC2023.Puzzels;

using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;

public class Day11_CosmicExpansion
{
    private record Galaxy(int Id)
    {
        public int X { get; set; }
        public int Y { get; set; }
    };

    private record GalaxyPairs(Galaxy Galaxy1, Galaxy Galaxy2);

    public long Part1()
    {
        var lines = File.ReadAllLines("Inputs/Day11.txt");

        var galaxies = this.GetGalaxies(lines).ToList();

        this.ExpandUniverse(1, ref galaxies);

        var pairs = this.GetGalaxyPairs(galaxies);

        return pairs.Select(p => Math.Abs(p.Galaxy1.X - p.Galaxy2.X) + Math.Abs(p.Galaxy1.Y - p.Galaxy2.Y)).Sum();
    }

    public ulong Part2()
    {
        var lines = File.ReadAllLines("Inputs/Day11.txt");

        var galaxies = this.GetGalaxies(lines).ToList();

        this.ExpandUniverse(999999, ref galaxies);

        var pairs = this.GetGalaxyPairs(galaxies);

        var distances = pairs.Select(p => Math.Abs(p.Galaxy1.X - p.Galaxy2.X) + Math.Abs(p.Galaxy1.Y - p.Galaxy2.Y));

        ulong sum = 0;
        foreach (var distance in distances)
        {
            sum += (ulong)distance;
        }

        return sum;
    }

    private IEnumerable<Galaxy> GetGalaxies(string[] lines)
    {
        var galaxyRegex = new Regex(@"(#)");

        var galaxyId = 1;
        var y = 0;
        foreach (var line in lines)
        {
            var matches = galaxyRegex.Matches(line);
            foreach (Match match in matches)
            {
                yield return new Galaxy(galaxyId++) { X = match.Index, Y = y };
            }

            y++;
        }
    }

    private void ExpandUniverse(int expansionWidth, ref List<Galaxy> galaxies)
    {
        var maxX = galaxies.Max(g => g.X);
        var maxY = galaxies.Max(g => g.Y);
        for (var i = 0; i < maxX; i++)
        {
            // Ignore columns that have a galaxy
            if (galaxies.Any(g => g.X == i))
            {
                continue;
            }

            galaxies.Where(galaxies => galaxies.X > i).ToList().ForEach(g => g.X += expansionWidth);
            maxX += expansionWidth;
            i += expansionWidth;
        }

        for (var i = 0; i < maxY; i++)
        {
            // Ignore rows that have a galaxy
            if (galaxies.Any(g => g.Y == i))
            {
                continue;
            }

            galaxies.Where(galaxies => galaxies.Y > i).ToList().ForEach(g => g.Y += expansionWidth);
            maxY += expansionWidth;
            i += expansionWidth;
        }
    }

    private List<GalaxyPairs> GetGalaxyPairs(List<Galaxy> galaxies)
    {
        var galaxyPairs = new List<GalaxyPairs>();
        for (var i = 0; i < galaxies.Count; i++)
        {
            for (var j = i + 1; j < galaxies.Count; j++)
            {
                galaxyPairs.Add(new GalaxyPairs(galaxies[i], galaxies[j]));
            }
        }

        return galaxyPairs;
    }
}