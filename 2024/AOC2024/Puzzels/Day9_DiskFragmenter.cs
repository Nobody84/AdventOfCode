namespace AOC2024.Puzzels;

using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Day9_DiskFragmenter : PuzzelBase
{
    private int[] input = [];

    private record File
    {
        public int StartIdx { get; init; }
        public int Id { get; init; }
        public int Length { get; set; }
    };

    private record FreeSpace
    {
        public int StartIdx { get; init; }
        public int Length { get; init; }
    };

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
        var uncompressedFile = new List<int>();

        var fileId = 0;
        for (int i = 0; i < input.Length; i++)
        {
            var value = input[i];
            if (i % 2 == 0)
            {
                // value is file length
                uncompressedFile.AddRange(Enumerable.Repeat(fileId, value));
                fileId++;
            }
            else
            {
                // value is freespace length, freespace is represented by -1
                uncompressedFile.AddRange(Enumerable.Repeat(-1, value));
            }
        }

        var frontIdx = 0;
        var backIdx = uncompressedFile.Count - 1;
        do
        {
            while (uncompressedFile[backIdx] == -1)
            {
                backIdx--;
            }

            while (uncompressedFile[frontIdx] != -1)
            {
                frontIdx++;
            }

            uncompressedFile[frontIdx] = uncompressedFile[backIdx];
            uncompressedFile[backIdx] = -1;

            frontIdx++;
            backIdx--;

        } while (frontIdx < backIdx);

        var checksum = uncompressedFile.Where(d => d != -1).Select((d, i) => (decimal)d * i).Sum();

        return checksum;
    }

    protected override object Part2()
    {
        var uncompressedFile = new List<int>();

        var files = new List<File>();
        var freeSpaces = new List<FreeSpace>();
        var fileId = 0;
        var uncompressdIdx = 0;
        for (int i = 0; i < input.Length; i++)
        {
            var value = input[i];
            if (i % 2 == 0)
            {
                // value is file length
                files.Add(new File { StartIdx = uncompressdIdx, Id = fileId, Length = value });
                fileId++;
            }
            else
            {
                // value is freespace length, freespace is represented by -1
                freeSpaces.Add(new FreeSpace { StartIdx = uncompressdIdx, Length = value });
            }

            uncompressdIdx += value;
        }

        files.Reverse();
        foreach (var file in files)
        {
            file.Length = 23;
        }

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