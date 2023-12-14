namespace AOC2023.Puzzels;

using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        //foreach(var mirror in mirrors)
        //{
        //    PrintBlock(blocks[mirror.BlockId - 1], mirror);
        //}

        return mirrors.Sum(m => m.Direction == Direction.Horizontal ? 100 * m.Position : m.Position);
    }

    private static List<Mirror> FindMirrors(List<List<string>> blocks)
    {
        var mirrors = new List<Mirror>();
        var blockNumber = 0;
        foreach (var rows in blocks)
        {
            blockNumber++;
            mirrors.AddRange(FindMirrors(rows, blockNumber));
        }

        return mirrors;
    }

    private static IEnumerable<Mirror> FindMirrors(List<string> rows, int blockNumber)
    {
        var mirrorPosition = FindMirrorPosition(rows);
        // If we found a vertical mirror, we don't need to check for a horizontal mirror
        if (mirrorPosition != 0)
        {
            var verticalMirror = new Mirror(blockNumber, Direction.Vertical, mirrorPosition);
            yield return verticalMirror;
        }

        var columnNumbers = Enumerable.Range(0, rows[0].Length);
        var columns = columnNumbers.Select((_, idx) => new string(rows.Select(row => row[idx]).ToArray())).ToList();
        mirrorPosition = FindMirrorPosition(columns);
        if (mirrorPosition != 0)
        {
            var horizontalMirror = new Mirror(blockNumber, Direction.Horizontal, mirrorPosition);
            yield return horizontalMirror;
        }

        yield return new Mirror(blockNumber, Direction.None, 0);
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

    public long Part2()
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
        var mirrors = new List<Mirror>();
        var origMirrors = FindMirrors(blocks);

        var blockNumber  = 0;
        foreach (var block in blocks)
        {
            var blockMirrors = new List<Mirror>();
            blockNumber++;
            Console.WriteLine($"Block {blockNumber}");
            var newBlock = block.Select(r => r.ToArray()).ToArray();
            for (var y = 0; y < block.Count; y++)
            {
                var line = new List<char>();
                for (var x = 0; x < block[0].Count(); x++)
                {
                    var original = newBlock[y][x];
                    newBlock[y][x] = original == '.' ? 'X' : '+';
                    PrintBlock(newBlock.Select(s => string.Join(null, s)).ToList(), new Mirror(0, Direction.None, 0));
                    newBlock[y][x] = original == '.' ? '#' : '.';
                    var newMirrors = FindMirrors(newBlock.Select(s => string.Join(null, s)).ToList(), blockNumber);

                    foreach (var mirror in newMirrors)
                    {
                        if (mirror.Direction != Direction.None)
                        {
                            if (!origMirrors.Contains(mirror) && !blockMirrors.Contains(mirror))
                            {
                                blockMirrors.Add(mirror);
                                newBlock[y][x] = original == '.' ? 'X' : '+';
                                //PrintBlock(newBlock.Select(s => string.Join(null, s)).ToList(), mirror);
                            }
                        }
                    }

                    newBlock[y][x] = original;
                }
            }

            mirrors.AddRange(blockMirrors);
            if (blockMirrors.Count == 0)
            { 
                Console.WriteLine($"{blockNumber} No mirrors found");
                PrintBlock(block, origMirrors.First());
            }
        }

        return mirrors.Sum(m => m.Direction == Direction.Horizontal ? 100 * m.Position : m.Position);
    }
}