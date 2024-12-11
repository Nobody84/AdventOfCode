namespace AOC2024.Puzzels;

using System;

public class Day11_PlutoniuanPebbles : PuzzelBase
{
    private Stone firstStone;
    private Stone2 firstStone2;
    private List<Stone2> stoneList;

    private class Stone(ulong value, Stone? previous, Stone? next)
    {
        public ulong Value { get; set; } = value;

        public Stone? Previous { get; set; } = previous;

        public Stone? Next { get; set; } = next;
    }

    private class Stone2(ulong value)
    {
        public ulong Value { get; set; } = value;

        public int Count { get; set; } = 1;
    }

    public Day11_PlutoniuanPebbles()
        : base(11, "Plutoniuan Pebbles")
    {
    }

    protected override void PreparePart1(string inputFilePath)
    {
        var content = File.ReadAllText(inputFilePath);
        var numbers = content.Split(' ').ToList();
        var initialStoneList = new List<Stone>();
        for (var i = 0; i < numbers.Count; i++)
        {
            var value = ulong.Parse(numbers[i]);
            var newStone = new Stone(value, null, null);
            if (i != 0)
            {
                var prevStone = initialStoneList[i - 1];
                newStone.Previous = prevStone;
                prevStone.Next = newStone;
            }

            initialStoneList.Add(newStone);
        }

        this.firstStone = initialStoneList.First();
    }

    protected override object Part1()
    {
        for (var i = 0; i < 25; i++)
        {
            var currentStone = this.firstStone;
            while (true)
            {
                var numberLength = currentStone.Value.ToString().Length;

                // If the stone is engraved with the number 0, it is replaced by a stone engraved with the number 1
                if (currentStone.Value == 0)
                {
                    currentStone.Value = 1;
                }
                // If the stone is engraved with a number that has an even number of digits, it is replaced by two stones.
                // The left half of the digits are engraved on the new left stone, and the right half of the digits are engraved on the new right stone.
                // (The new numbers don't keep extra leading zeroes: 1000 would become stones 10 and 0.
                else if (numberLength % 2 == 0)
                {

                    var firstHalf = currentStone.Value.ToString().Substring(0, numberLength / 2);
                    var secondHalf = currentStone.Value.ToString().Substring(numberLength / 2);
                    currentStone.Value = ulong.Parse(firstHalf);

                    var newStone = new Stone(ulong.Parse(secondHalf), currentStone, currentStone.Next);

                    if (currentStone.Next != null)
                    {
                        currentStone.Next.Previous = newStone;
                    }

                    currentStone.Next = newStone;
                    currentStone = newStone;
                }
                // If none of the other rules apply, the stone is replaced by a new stone;
                // the old stone's number multiplied by 2024 is engraved on the new stone.
                else
                {
                    currentStone.Value *= 2024;
                }

                if (currentStone.Next == null)
                {
                    break;
                }

                currentStone = currentStone.Next;
            }

            PrintStoneChainSortedAndCompressed(this.firstStone);
        }

        var count = 0;
        var currentStoneCount = this.firstStone;
        while (true)
        {
            count++;
            if (currentStoneCount.Next == null)
            {
                break;
            }

            currentStoneCount = currentStoneCount.Next;
        }

        return count;
    }

    protected override void PreparePart2(string inputFilePath)
    {
        var content = File.ReadAllText(inputFilePath);
        var numbers = content.Split(' ').ToList();
        this.stoneList = new List<Stone2>();
        for (var i = 0; i < numbers.Count; i++)
        {
            stoneList.Add(new Stone2(ulong.Parse(numbers[i])));
        }

        this.firstStone2 = stoneList.First();
    }

    protected override object Part2()
    {
        for (var i = 0; i < 25; i++)
        {
            var stoneCount = this.stoneList.Count;
            var currentStoneList = new List<Stone2>(this.stoneList);
            for (var j = 0; j < stoneCount; j++)
            {
                var currentStone = currentStoneList[j];

                var numberLength = currentStone.Value.ToString().Length;

                // If the stone is engraved with the number 0, it is replaced by a stone engraved with the number 1
                if (currentStone.Value == 0)
                {
                    currentStone.Value = 1;
                }
                // If the stone is engraved with a number that has an even number of digits, it is replaced by two stones.
                // The left half of the digits are engraved on the new left stone, and the right half of the digits are engraved on the new right stone.
                // (The new numbers don't keep extra leading zeroes: 1000 would become stones 10 and 0.
                else if (numberLength % 2 == 0)
                {

                    var firstHalf = currentStone.Value.ToString().Substring(0, numberLength / 2);
                    var secondHalf = currentStone.Value.ToString().Substring(numberLength / 2);
                    currentStone.Value = ulong.Parse(firstHalf);

                    var newStone = new Stone2(ulong.Parse(secondHalf)) { Count = currentStone.Count };
                    currentStoneList.Add(newStone);
                }
                // If none of the other rules apply, the stone is replaced by a new stone;
                // the old stone's number multiplied by 2024 is engraved on the new stone.
                else
                {
                    currentStone.Value *= 2024;
                }
            }

            this.stoneList = this.stoneList.Where(s => s.Count != 0).ToList();

            Console.WriteLine(string.Join(", ", this.stoneList.OrderBy(s => s.Value).Select(s => $"{s.Value} ({s.Count})")));
        }

        var numberOfStones = this.stoneList.Select(s => (decimal)s.Count).Sum();

        return numberOfStones;
    }

    private static void PrintStoneChain(Stone firstStone)
    {
        var currentPrintStone = firstStone;
        while (true)
        {
            Console.Write($"{currentPrintStone.Value} ");
            if (currentPrintStone.Next == null)
            {
                Console.WriteLine();
                break;
            }

            currentPrintStone = currentPrintStone.Next;
        }
    }

    private static void PrintStoneChainSortedAndCompressed(Stone firstStone)
    {
        var stones = new SortedDictionary<ulong, int>();
        var currentStone = firstStone;
        while (true)
        {
           if (stones.ContainsKey(currentStone.Value))
            {
                stones[currentStone.Value]++;
            }
            else
            {
                stones.Add(currentStone.Value, 1);
            }

            if (currentStone.Next == null)
            {
                break;
            }

            currentStone = currentStone.Next;
        }

        Console.WriteLine(string.Join(", ", stones.Select(s => $"{s.Key} ({s.Value})")));
    }
}