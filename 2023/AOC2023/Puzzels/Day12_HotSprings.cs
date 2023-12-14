namespace AOC2023.Puzzels;

using System.Text.RegularExpressions;

public class Day12_HotSprings
{
    private enum Status
    {
        Unknown,
        Operational,
        Broken
    }

    private record DocumentationRow(List<Status> SpringStatus, List<int> DamageSpringGroups);

    public long Part1()
    {
        var lines = File.ReadAllLines("Inputs/Day12.txt");
        var documentationRows = this.GetDocumentationRows(lines).ToList();
        
        foreach(var row in documentationRows)
        {

        }

        return 0;
    }

    public long Part2()
    {

        var lines = File.ReadAllLines("Inputs/Day12.txt");
        return 0;
    }

    private IEnumerable<DocumentationRow> GetDocumentationRows(string[] lines)
    {
        var regex = new Regex(@"(?<springs>[.#?]*) (?<damagedSpringsGroups>[\d,]+)");
        foreach (var line in lines)
        {
            var match = regex.Match(line);
            var springStatus = match.Groups["springs"].Value.Select(c => c switch
            {
                '#' => Status.Broken,
                '.' => Status.Operational,
                _ => Status.Unknown
            }).ToList();

            var damagedSpringGroups = match.Groups["damagedSpringsGroups"].Value.Split(',').Select(int.Parse).ToList();

            yield return new DocumentationRow(springStatus, damagedSpringGroups);
        }
    }
}