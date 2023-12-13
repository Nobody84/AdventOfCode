namespace AOC2023.Puzzels;

using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;

public class Day13_PointOfIncidence
{
    record Mirror(int BlockId, Direction Direction, int Position);

    private enum Direction
    {
        Vertical,
        Horizontal,
        None
    }

    public long Part1()
    {
        var lines = System.IO.File.ReadAllLines(@"Inputs\Day13.txt");

        var blocks = new List<List<string>>();
        var currentBlock = new List<string>();
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                blocks.Add(currentBlock);
                currentBlock = new List<string>();
                continue;
            }

            currentBlock.Add(line);
        }

        blocks.Add(currentBlock);
        var mirrors = FindMirrors(blocks);

        return mirrors.Sum(m => m.Direction == Direction.Horizontal ? 100 * m.Position : m.Position);
    }

    private static List<Mirror> FindMirrors(List<List<string>> blocks)
    {
        var mirrors = new List<Mirror>();
        var blockNumber = 0;
        foreach (var rows in blocks)
        {
            blockNumber++;
            var mirrorPosition = FindMirrorPosition(rows);
            // If we found a vertical mirror, we don't need to check for a horizontal mirror
            if (mirrorPosition != 0)
            {
                var verticalMirror = new Mirror(blockNumber, Direction.Vertical, mirrorPosition);
                mirrors.Add(verticalMirror);
                continue;
            }

            var columnNumbers = Enumerable.Range(0, rows[0].Length);
            var columns = columnNumbers.Select((_, idx) => new string(rows.Select(row => row[idx]).ToArray())).ToList();
            mirrorPosition = FindMirrorPosition(columns);
            if (mirrorPosition != 0)
            {
                var horizontalMirror = new Mirror(blockNumber, Direction.Horizontal, mirrorPosition);
                mirrors.Add(horizontalMirror);
            }
        }

        return mirrors;
    }

    private static int FindMirrorPosition(List<string> lines)
    {
        var mirrorPosition = 0;
        var width = lines[0].Length;
        for (var x = 1; x < width; x++)
        {
            var equal = true;
            foreach (var line in lines)
            {
                var firstPart = line.Substring(0, x);
                var secondPart = line.Substring(x);
                var compairWidth = Math.Min(firstPart.Length, secondPart.Length);
                if (!firstPart.Skip(firstPart.Length - compairWidth).SequenceEqual(secondPart.Substring(0, compairWidth).Reverse()))
                {
                    equal = false;
                    break;
                }
            }

            if (equal)
            {
                mirrorPosition = x;
                break;
            }
        }

        return mirrorPosition;
    }

    private static void PrintBlock(List<string> block, Mirror mirror)
    {
        if (mirror.Direction == Direction.Horizontal)
        {
            for (var y = 0; y < block.Count; y++)
            {
                Console.WriteLine(block[y]);
                if (y == mirror.Position - 1)
                {
                    Console.WriteLine(new string('-', block[y].Length));
                }
            }
        }
        else if (mirror.Direction == Direction.Vertical)
        {
            foreach (var line in block)
            {
                Console.WriteLine(line.Insert(mirror.Position, "|"));
            }
        }
        else
        {
            foreach (var line in block)
            {
                Console.WriteLine(line);
            }
        }

        Console.WriteLine();
    }

    public ulong Part2()
    {
        return 0;
    }
}