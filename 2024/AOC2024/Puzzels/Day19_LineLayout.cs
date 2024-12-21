using System.Collections.Concurrent;
using System.Reflection.Metadata.Ecma335;

namespace AOC2024.Puzzels;

public class Day19_LineLayout : PuzzelBase
{
    private List<string> designs;
    private Dictionary<char, Dictionary<int, List<string>>> availableTowels;

    private readonly object hashSetLock = new();
    private HashSet<string> impossiblePartialDesigns = new();

    private List<List<string>> possiblePatterns = new();

    public Day19_LineLayout()
     : base(19, "Line Layout")
    {
    }

    protected override void PreparePart1(string inputFile)
    {
        this.availableTowels = new();
        this.designs = new();
        this.impossiblePartialDesigns = new();

        var lines = System.IO.File.ReadAllLines(inputFile);
        var firstCharGroups = lines[0].Split(",").Select(v => v.Trim()).GroupBy(s => s[0]);
        foreach (var group in firstCharGroups)
        {
            var lengthGroups = group.GroupBy(s => s.Length);
            var lengthDict = new Dictionary<int, List<string>>();
            foreach (var lengthGroup in lengthGroups)
            {
                if (!availableTowels.ContainsKey(group.Key))
                {
                    availableTowels.Add(group.Key, new());
                }

                this.availableTowels[group.Key].Add(lengthGroup.Key, lengthGroup.ToList());
            }
        }

        foreach (var line in lines.Skip(2))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            this.designs.Add(line);
        }
    }

    protected override object Part1()
    {
        var count = 0;
        foreach (var design in this.designs)
        {
            if (CheckTowel(design))
            {
                count++;
            }
        };

        return count;
    }

    protected override object Part2()
    {
        foreach (var design in this.designs)
        {
            CheckTowel2(design, new());
        };

        foreach (var pattern in this.possiblePatterns)
        {
            Console.WriteLine(string.Join(",", pattern));
        }

        return this.possiblePatterns.Count;
    }

    private bool CheckTowel(string partialDesign)
    {
        if (this.impossiblePartialDesigns.Contains(partialDesign))
        {            
            return false;
        }

        if (!this.availableTowels.TryGetValue(partialDesign[0], out Dictionary<int, List<string>> lengthDict))
        {            
            return false;
        }

        var towels = lengthDict.Where(e => e.Key <= partialDesign.Length).SelectMany(e => e.Value);
        if (!towels.Any())
        {            
            return false;
        }

        foreach (var towel in towels)
        {
            if (partialDesign == towel)
            {
                return true;
            }
            else
            {
                if (partialDesign.StartsWith(towel))
                {
                    var newPartialDesign = partialDesign.Substring(towel.Length);
                    if (CheckTowel(newPartialDesign))
                    {
                        return true;
                    }
                }
            }
        }

        lock (hashSetLock)
        {
            this.impossiblePartialDesigns.Add(partialDesign);
        }
        return false;
    }

    private bool CheckTowel2(string partialDesign, List<string> currentTowels)
    {
        if (this.impossiblePartialDesigns.Contains(partialDesign))
        {
            return false;
        }

        if (!this.availableTowels.TryGetValue(partialDesign[0], out Dictionary<int, List<string>> lengthDict))
        {
            return false;
        }

        var towels = lengthDict.Where(e => e.Key <= partialDesign.Length).SelectMany(e => e.Value);
        if (!towels.Any())
        {
            return false;
        }

        var found = false;
        foreach (var towel in towels)
        {
            if (partialDesign == towel)
            {
                currentTowels.Add(towel);
                possiblePatterns.Add(currentTowels);
                return true;
            }
            else
            {
                if (partialDesign.StartsWith(towel))
                {
                    var newPartialDesign = partialDesign.Substring(towel.Length);
                    if (CheckTowel2(newPartialDesign, new List<string>(currentTowels) { towel }))
                    {
                        found = true;
                    }
                }
            }
        }

        if (!found)
        {
            lock (hashSetLock)
            {
                this.impossiblePartialDesigns.Add(partialDesign);
            }
        }

        return false;
    }
}