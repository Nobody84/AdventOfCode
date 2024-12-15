using System.Text.RegularExpressions;

namespace AOC2024.Puzzels;

public class Day14_RestroomRedoubt : PuzzelBase
{
    private readonly Regex robotRegex = new Regex(@"p=(?<x>\d+),(?<y>\d+).*v=(?<vx>-*\d+),(?<vy>-*\d+)");

    private List<Robot> robots = new();

    private class Robot(int y, int x, int vY, int vX)
    {
        public int Y { get; set; } = y;
        public int X { get; set; } = x;
        public int VY { get; set; } = vY;
        public int VX { get; set; } = vX;
    }


    private record Position(int Y, int X);

    public Day14_RestroomRedoubt()
        : base(14, "Claw Restroom Redoubt")
    {
    }

    protected override void PreparePart1(string inputFile)
    {
        var lines = File.ReadLines(inputFile);
        this.robots = lines.Select(line => robotRegex.Match(line)).Select(match => new Robot(
                int.Parse(match.Groups["y"].Value),
                int.Parse(match.Groups["x"].Value),
                int.Parse(match.Groups["vy"].Value),
                int.Parse(match.Groups["vx"].Value))).ToList();
    }

    protected override object Part1()
    {
        var maxY = 103;
        var maxX = 101;
        var seconds = 100;

        var q1 = 0;
        var q2 = 0;
        var q3 = 0;
        var q4 = 0;
        foreach (var robot in this.robots)
        {
            var x = robot.X + robot.VX * seconds;
            var y = robot.Y + robot.VY * seconds;

            x = x < 0 ? maxX + (x % maxX) : x % maxX;
            y = y < 0 ? maxY + (y % maxY) : y % maxY;
            x = x == maxX ? 0 : x;
            y = y == maxY ? 0 : y;

            robot.X = x;
            robot.Y = y;

            var xCenter = maxX / 2;
            var yCenter = maxY / 2;
            if (x < xCenter && y < yCenter)
            {
                q1++;
            }
            else if (x > xCenter && y < yCenter)
            {
                q2++;
            }
            else if (x < xCenter && y > yCenter)
            {
                q3++;
            }
            else if (x > xCenter && y > yCenter)
            {
                q4++;
            }
        }

        return q1 * q2 * q3 * q4;
    }

    protected override object Part2()
    {
        var maxY = 103;
        var maxX = 101;
        var seconds = 0;

        var postions = new HashSet<Position>();
        while (true)
        {
            seconds++;
            postions.Clear();
            var found = true;
            foreach (var robot in this.robots)
            {
                var x = robot.X + robot.VX * seconds;
                var y = robot.Y + robot.VY * seconds;

                x = x < 0 ? maxX + (x % maxX) : x % maxX;
                y = y < 0 ? maxY + (y % maxY) : y % maxY;
                x = x == maxX ? 0 : x;
                y = y == maxY ? 0 : y;


                var pos = new Position(y, x);
                if (postions.Contains(pos))
                {
                    found = false;
                    break;
                }

                postions.Add(pos);
            }

            if (found)
            {
                break;
            }
        }

        for (var y = 0; y < maxY; y++)
        {
            for (var x = 0; x < maxX; x++)
            {
                var p = new Position(y, x);
                if (postions.Contains(p))
                {
                    ConsoleExtensions.Write("#", ConsoleColor.DarkGreen);
                }
                else
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }
        return seconds;
    }
}