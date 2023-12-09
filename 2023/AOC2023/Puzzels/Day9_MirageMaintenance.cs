namespace AOC2023.Puzzels;

using AOC2023.Puzzels.Day9;
using System;
using System.Security;
using System.Text.RegularExpressions;

public class Day9_MirageMaintenance
{
    public long Part1()
    {
        var lines = File.ReadAllLines("Inputs/Day9.txt");
        var datasets = this.GetDatasets(lines);
        
        return datasets.Sum(d => d.GetPredicatedNumber(false));
    }


    public long Part2()
    {
        var lines = File.ReadAllLines("Inputs/Day9.txt");
        var datasets = this.GetDatasets(lines);

        return datasets.Sum(d => d.GetPredicatedNumber(true));
    }

    public List<Dataset> GetDatasets(string[] lines)
    {
        var numberRegex = new Regex(@"(-?\d+)");
        var datasets = lines.Select(l => new Dataset(numberRegex.Matches(l).Select(n => int.Parse(n.Value)))).ToList();
        return datasets;
    }
}