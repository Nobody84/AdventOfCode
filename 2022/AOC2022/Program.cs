using AOC2022.Puzzels;

namespace AOC2022
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var day1 = new Day1();
            Console.WriteLine($"Day 1 Part 1: Answer={day1.PartOne()}");
            Console.WriteLine($"Day 1 Part 2: Answer={day1.PartTwo()}");
            var day2 = new Day2();
            Console.WriteLine($"Day 2 Part 1: Answer={day2.PartOne()}");
            Console.WriteLine($"Day 2 Part 2: Answer={day2.PartTwo()}");
            var day3 = new Day3();
            Console.WriteLine($"Day 3 Part 1: Answer={day3.PartOne()}");
            Console.WriteLine($"Day 3 Part 2: Answer={day3.PartTwo()}");

            Console.ReadKey();
        }
    }
}