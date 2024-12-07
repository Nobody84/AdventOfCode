using System.Globalization;

namespace AOC2024
{
    public abstract class PuzzelBase
    {
        private readonly int day;
        private readonly string name;

        protected PuzzelBase(int day, string name)
        {
            this.day = day;
            this.name = name;
        }

        protected abstract void PreparePart1(string inputPath);
        protected abstract object Part1();

        protected abstract void PreparePart2(string inputPath);
        protected abstract object Part2();

        public void Run()
        {
            Console.WriteLine($"Day {this.day} - {this.name}");
            this.PreparePart1($"Inputs/Day{day}.txt");
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var result1 = Part1();
            sw.Stop();
            Console.WriteLine($"Part 1: {result1}, Time={FormatTime(sw.Elapsed.TotalMicroseconds)}");

            this.PreparePart2($"Inputs/Day{day}.txt");
            sw.Restart();
            var result2 = Part2();
            sw.Stop();
            Console.WriteLine($"Part 2: {result2}, Time={FormatTime(sw.Elapsed.TotalMicroseconds)}");
        }

        private static string FormatTime(double microseconds)
        {
            if (microseconds < 1000)
            {
                return $"{microseconds} µs";
            }
            else if (microseconds < 1_000_000)
            {
                double milliseconds = microseconds / 1000.0;
                return $"{milliseconds.ToString("F4", CultureInfo.InvariantCulture)} ms";
            }
            else
            {
                double seconds = microseconds / 1_000_000.0;
                return $"{seconds.ToString("F4", CultureInfo.InvariantCulture)} s";
            }
        }
    }
}
