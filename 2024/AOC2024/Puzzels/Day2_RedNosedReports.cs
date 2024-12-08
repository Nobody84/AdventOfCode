
namespace AOC2024.Puzzels;

public class Day2_RedNosedReports : PuzzelBase
{
    private IEnumerable<int[]> levelsList = new List<int[]>();

    public Day2_RedNosedReports() 
        : base(2, "Red-Nosed Reports")
    {
    }

    protected override void PreparePart1(string inputPath)
    {
        var lines = File.ReadLines("inputPath");
        this.levelsList = lines.Select(l => l.Split(" ").Select(int.Parse).ToArray());
    }

    protected override object Part1()
    {
        var count = 0;
        foreach (var levels in levelsList)
        {
            var safe = true;
            var previous = levels[0];
            var increasing = levels[1] - levels[0] > 0;
            for (var i = 1; i < levels.Length; i++)
            {
                var current = levels[i];
                var difference = current - previous;
                if (increasing)
                {
                    if (difference < 1 || difference > 3)
                    {

                        safe = false;
                        break;
                    }
                }
                else
                {
                    if (difference > -1 || difference < -3)
                    {
                        safe = false;
                        break;
                    }
                }

                previous = current;
            }

            if (safe)
            {
                count++;
            }
        }

        return count;
    }

    protected override object Part2()
    {
        var count = 0;
        foreach (var levels in levelsList)
        {
            for (var m = 0; m < levels.Length; m++)
            {
                var modifiedLevels = levels.ToList();
                modifiedLevels.RemoveAt(m);

                var safe = true;
                var previous = modifiedLevels[0];
                var increasing = modifiedLevels[1] - modifiedLevels[0] > 0;
                for (var i = 1; i < modifiedLevels.Count; i++)
                {
                    var current = modifiedLevels[i];
                    var difference = current - previous;
                    if (increasing)
                    {
                        if (difference < 1 || difference > 3)
                        {
                            safe = false;
                            break;
                        }
                    }
                    else
                    {
                        if (difference > -1 || difference < -3)
                        {
                            safe = false;
                            break;
                        }
                    }
                    previous = current;
                }

                if (safe)
                {
                    count++;
                    break;
                }
            }
        }

        return count;
    }
}