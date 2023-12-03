namespace AOC2023.Puzzels;

public class Day1_Terbuchet
{
    public int Part1()
    {
        var numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        var lines = File.ReadLines("Inputs/Day1_Part1.txt");
        var calibrationsNumber = 0;
        foreach (var line in lines)
        {
            var fistIndex = line.IndexOfAny(numbers);
            var lastIndex = line.LastIndexOfAny(numbers);
            calibrationsNumber += int.Parse($"{line[fistIndex]}{line[lastIndex]}");
        }

        return calibrationsNumber;
    }

    public int Part2()
    {
        var replacements = new Dictionary<string, string>
        {
            { "seven", "7" },
            { "nine", "9" },
            { "three", "3" },
            { "one", "1" },
            { "eight", "8" },
            { "two", "2" },
            { "four", "4" },
            { "five", "5" },
            { "six", "6" },
        };

        var numbers = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        var lines = File.ReadLines("Inputs/Day1_Part2.txt");
        var calibrationsNumber = 0;
        foreach (var line in lines)
        {
            var cleandLine = string.Empty;
            var currentLine = line;
            var i = 1;
            do
            {
                foreach (var replacement in replacements)
                {
                    var reaplaced = currentLine.Substring(0, i).Replace(replacement.Key, replacement.Value);
                    if (reaplaced.Length < i)
                    {
                        cleandLine += reaplaced;
                        currentLine = currentLine.Substring(i);
                        i = 0;
                        break;
                    }
                }

                if (currentLine.Length == i)
                {
                    cleandLine += currentLine;
                    break;
                }

                i++;
            } while (currentLine.Length != 0);

            var fistIndex = cleandLine.IndexOfAny(numbers);
            var lastIndex = cleandLine.LastIndexOfAny(numbers);
            var currenCalibarionNumber = int.Parse($"{cleandLine[fistIndex]}{cleandLine[lastIndex]}");
            calibrationsNumber += currenCalibarionNumber;
        }

        return calibrationsNumber;
    }
}