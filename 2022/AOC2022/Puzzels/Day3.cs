namespace AOC2022.Puzzels
{
    public class Day3
    {
        private readonly string[] inputLine;
        private Dictionary<char, int> priorities = new Dictionary<char, int>
        {
            { 'a', 1 },
            { 'b', 2 },
            { 'c', 3 },
            { 'd', 4 },
            { 'e', 5 },
            { 'f', 6 },
            { 'g', 7 },
            { 'h', 8 },
            { 'i', 9 },
            { 'j', 10 },
            { 'k', 11 },
            { 'l', 12 },
            { 'm', 13 },
            { 'n', 14 },
            { 'o', 15 },
            { 'p', 16 },
            { 'q', 17 },
            { 'r', 18 },
            { 's', 19 },
            { 't', 20 },
            { 'u', 21 },
            { 'v', 22 },
            { 'w', 23 },
            { 'x', 24 },
            { 'y', 25 },
            { 'z', 26 },
            { 'A', 27 },
            { 'B', 28 },
            { 'C', 29 },
            { 'D', 30 },
            { 'E', 31 },
            { 'F', 32 },
            { 'G', 33 },
            { 'H', 34 },
            { 'I', 35 },
            { 'J', 36 },
            { 'K', 37 },
            { 'L', 38 },
            { 'M', 39 },
            { 'N', 40 },
            { 'O', 41 },
            { 'P', 42 },
            { 'Q', 43 },
            { 'R', 44 },
            { 'S', 45 },
            { 'T', 46 },
            { 'U', 47 },
            { 'V', 48 },
            { 'W', 49 },
            { 'X', 50 },
            { 'Y', 51 },
            { 'Z', 52 },
        };

        public Day3()
        {
            inputLine = File.ReadAllLines(@"./Inputs/day3.txt");
        }

        public int PartOne()
        {
            var score = 0;
            foreach (var line in inputLine)
            {
                var firstCompartment = line.Take(line.Length / 2);
                var secondCompartment = line.Skip(line.Length / 2);
                var itemInBothComparments = firstCompartment.First(i => secondCompartment.Contains(i));
                score += priorities[itemInBothComparments];
            }

            return score;
        }

        public int PartTwo()
        {
            var score = 0;
            for (var i = 0; i < inputLine.Length; i += 3)
            {
                var rucksack1 = inputLine[i];
                var rucksack2 = inputLine[i + 1];
                var rucksack3 = inputLine[i + 2];

                var badgeItem = rucksack1.First(i => rucksack2.Contains(i) && rucksack3.Contains(i));
                score += priorities[badgeItem];
            }

            return score;
        }
    }
}