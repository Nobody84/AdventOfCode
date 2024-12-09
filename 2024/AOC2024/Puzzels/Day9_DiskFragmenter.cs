namespace AOC2024.Puzzels;

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;

public class Day9_DiskFragmenter : PuzzelBase
{
    private int[] input = [];

    public Day9_DiskFragmenter()
        : base(9, "Disk Fragmenter")
    {
    }

    protected override void PreparePart1(string inputFilePath)
    {
        this.input = System.IO.File.ReadAllText(inputFilePath).Replace("\r\n", string.Empty).Select(c => int.Parse([c])).ToArray();
    }

    protected override object Part1()
    {
        var uncompressedSb = new StringBuilder(this.input.Length*10);
        var fileId = 0;
        for (var i = 0; i < this.input.Length; i++)
        {
            if (i % 2 == 0)
            {
                uncompressedSb.Append(ToString(fileId, this.input[i]));
                fileId++;
            }
            else
            {
                uncompressedSb.Append(new String('.', this.input[i]));
            }
        }

        var uncompressed = uncompressedSb.ToString().ToCharArray();
        var lastReplacementPos = uncompressed.Length;
        for (var i = 0; i < uncompressed.Length; i++)
        {
            if (uncompressed[i] == '+')
            {
                break;
            }

            //Console.WriteLine(string.Join("", uncompressed.Select(u => u.ToString())));
            if (uncompressed[i] == '.')
            {
                do
                {
                    lastReplacementPos--;
                } while (uncompressed[lastReplacementPos] == '.');

                uncompressed[i] = uncompressed[lastReplacementPos];
                uncompressed[lastReplacementPos] = '+';
            }
        }

        var compressedDataChar = uncompressed.Where(c => c != '.' && c != '+');
        //Console.WriteLine(string.Join("", compressedDataChar));
        var compressedData = string.Join("", compressedDataChar).Select(d => decimal.Parse(d.ToString()));

        var checksum = compressedData.Select((value, idx) => value * idx).Sum();
        return checksum;
    }

    protected override object Part2()
    {
        return 0;
    }

    private static string ToString(int value, int count)
    {
        var sb = new StringBuilder(count);
        for (var i = 0; i < count; i++)
        {
            sb.Append(value);
        }
        return sb.ToString();
    }
}