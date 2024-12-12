namespace AOC2024.Puzzels;

using System;

public class Day11_PlutoniuanPebbles : PuzzelBase
{
    private List<Stone> stoneList;

    private class Stone(ulong value)
    {
        public ulong Value { get; set; } = value;

        public decimal Count { get; set; } = 1;
    }

    public Day11_PlutoniuanPebbles()
        : base(11, "Plutoniuan Pebbles")
    {
    }

    protected override void PreparePart1(string inputFilePath)
    {
        var content = File.ReadAllText(inputFilePath);
        var numbers = content.Split(' ').ToList();

        this.stoneList = new List<Stone>();
        for (var i = 0; i < numbers.Count; i++)
        {
            stoneList.Add(new Stone(ulong.Parse(numbers[i])));
        }
    }

    protected override object Part1()
    {
        for (var i = 0; i < 25; i++)
        {
            BlinkeOnce(ref this.stoneList);
        }

        var numberOfStones = this.stoneList.Select(s => (decimal)s.Count).Sum();
        return numberOfStones;
    }

    protected override object Part2()
    {
        for (var i = 0; i < 75; i++)
        {
            BlinkeOnce(ref this.stoneList);
        }

        var numberOfStones = this.stoneList.Select(s => (decimal)s.Count).Sum();
        return numberOfStones;
    }

    private static void BlinkeOnce(ref List<Stone> stoneList)
    {
        var stoneCount = stoneList.Count;
        for (var j = 0; j < stoneCount; j++)
        {
            var currentStone = stoneList[j];

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

                var newStone = new Stone(ulong.Parse(secondHalf)) { Count = currentStone.Count };
                stoneList.Add(newStone);
            }
            // If none of the other rules apply, the stone is replaced by a new stone;
            // the old stone's number multiplied by 2024 is engraved on the new stone.
            else
            {
                currentStone.Value *= 2024;
            }
        }

        var stoneGroups = stoneList.GroupBy(s => s.Value).ToList();
        stoneList.Clear();
        foreach (var group in stoneGroups)
        {
            var count = group.Select(g => (decimal)g.Count).Sum();
            if (count != 0)
            {
                stoneList.Add(new Stone(group.Key) { Count = count });
            }
        }
    }
}