namespace AOC2024.Puzzels;

public class Day1_HistorianHysteria
{
    public int Part1()
    {
        var lines = File.ReadLines("Inputs/Day1.txt");
        var splits = lines.Select(l => l.Split("   ").Select(int.Parse));
        var leftList = splits.Select(i => i.ElementAt(0)).Order().ToArray();
        var rightList = splits.Select(i => i.ElementAt(1)).Order().ToArray();

        var sum = 0;
        for (var i = 0; i < leftList.Count(); i++)
        {
            sum += Math.Abs(leftList[i] - rightList[i]);
        }

        return sum;
    }

    public int Part2()
    {
        var lines = File.ReadLines("Inputs/Day1.txt");
        var splits = lines.Select(l => l.Split("   ").Select(int.Parse));
        var leftList = splits.Select(i => i.ElementAt(0));
        var rightListGroups = splits.Select(i => i.ElementAt(1)).GroupBy(n => n).ToDictionary(g => g.Key, v => v.Count());

        var sum = 0;
        foreach (var number in leftList)
        {
            sum += number * (rightListGroups.ContainsKey(number) ? rightListGroups[number] : 0);
        }

        return sum;
    }
}