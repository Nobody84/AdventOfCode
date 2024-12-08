using System.Diagnostics;
using System.Globalization;

namespace AOC2024
{
    public abstract class PuzzelBase
    {

        protected PuzzelBase(int day, string name)
        {
            this.Day = day;
            this.Name = name;
        }

        public int Day { get; private set; }

        public string Name { get; private set; }

        protected virtual void PreparePart1(string inputPath) { }
        protected abstract object Part1();

        protected virtual void PreparePart2(string inputPath)
        {
            this.PreparePart1(inputPath);
        }
        protected abstract object Part2();

        public void Run()
        {
            Console.WriteLine($"Day {this.Day} - {this.Name}");
            this.PreparePart1($"Inputs/Day{Day}.txt");
            var sw = new Stopwatch();
            sw.Restart();
            var result1 = Part1();
            sw.Stop();
            Console.WriteLine($"Part 1: {result1}, Time={FormatTime(sw.Elapsed.TotalMicroseconds)}");

            this.PreparePart2($"Inputs/Day{Day}.txt");
            sw.Restart();
            var result2 = Part2();
            sw.Stop();
            Console.WriteLine($"Part 2: {result2}, Time={FormatTime(sw.Elapsed.TotalMicroseconds)}");
        }

        private static string FormatTime(double microseconds)
        {
            if (microseconds < 1000)
            {
                return $"{microseconds:F3} µs";
            }
            else if (microseconds < 1_000_000)
            {
                double milliseconds = microseconds / 1000.0;
                return $"{milliseconds:F3} ms";
            }
            else
            {
                double seconds = microseconds / 1_000_000.0;
                return $"{seconds:F3} s";
            }
        }
    }
}
