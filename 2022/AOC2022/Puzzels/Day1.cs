namespace AOC2022.Puzzels
{
    using System;

    public class Day1
    {
        private readonly string[] inputLine;

        public Day1()
        {
            inputLine = File.ReadAllLines(@"./Inputs/day1.txt");
        }

        public int PartOne()
        {
            var caloriesPerElf = GetCaloriesPerElf();
            return caloriesPerElf.Max();
        }

        public int PartTwo()
        {
            var caloriesPerElf = GetCaloriesPerElf();
            return caloriesPerElf.OrderDescending().Take(3).Sum();
        }

        private List<int> GetCaloriesPerElf() {
            var caloriesPerElf = new List<int>();

            var calories = 0;
            foreach (var line in inputLine)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    caloriesPerElf.Add(calories);
                    calories = 0;
                    continue;
                }

                calories += int.Parse(line);
            }

            caloriesPerElf.Add(calories);

            return caloriesPerElf;
        }
    }
}