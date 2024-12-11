namespace AOC2024.Puzzels;

using System;

public class Day11_PlutoniuanPebbles : PuzzelBase
{
    private Stone firstStone;

    private class Stone(ulong value, Stone? previous, Stone? next)
    {
        public ulong Value { get; set; } = value;

        public Stone? Previous { get; set; } = previous;

        public Stone? Next { get; set; } = next;
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

    protected override object Part2()
    {
        return 0;
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
}