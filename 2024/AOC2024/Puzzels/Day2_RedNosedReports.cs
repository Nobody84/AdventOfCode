namespace AOC2024.Puzzels;

public class Day2_RedNosedReports
{
    public int Part1()
    {
        var lines = File.ReadLines("Inputs/Day2.txt");
        var levelsList = lines.Select(l => l.Split(" ").Select(int.Parse).ToArray());

        var count = 0;
        foreach (var levels in levelsList)
        {
            var safe = true;
            var previous = levels[0];
            var increasing = levels[1] - levels[0] > 0;
            var skipCount = 1;
            for (var i = 1; i < levels.Length; i++)
            {
                var current = levels[i];
                var difference = current - previous;
                if (increasing)
                {
                    if (difference < 1 || difference > 3)
                    {
                        if (skipCount == 0)
                        {
                            skipCount++;
                            continue;
                        }

                        safe = false;
                        break;
                    }
                }
                else
                {
                    if (difference > -1 || difference < -3)
                    {
                        if (skipCount == 0)
                        {
                            skipCount++;
                            continue;
                        }

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

    public int Part2()
    {
        return 0;
    }
}