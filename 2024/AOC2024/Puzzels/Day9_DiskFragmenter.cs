namespace AOC2024.Puzzels;

using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Day9_DiskFragmenter : PuzzelBase
{
    private int[] input = [];

    private record File
    {
        public int StartIdx { get; set; }
        public int Id { get; init; }
        public int Length { get; set; }
    };

    private record FreeSpace
    {
        public int StartIdx { get; set; }
        public int Length { get; set; }
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
        var uncompressedFiles = new List<int>();

        var fileId = 0;
        for (int i = 0; i < input.Length; i++)
        {
            var value = input[i];
            if (i % 2 == 0)
            {
                // value is file length
                uncompressedFiles.AddRange(Enumerable.Repeat(fileId, value));
                fileId++;
            }
            else
            {
                // value is freespace length, freespace is represented by -1
                uncompressedFiles.AddRange(Enumerable.Repeat(-1, value));
            }
        }

        var frontIdx = 0;
        var backIdx = uncompressedFiles.Count - 1;
        do
        {
            while (uncompressedFiles[backIdx] == -1)
            {
                backIdx--;
            }

            while (uncompressedFiles[frontIdx] != -1)
            {
                frontIdx++;
                if (frontIdx >= backIdx)
                {
                    break;
                }
            }

            if (frontIdx >= backIdx)
            {
                break;
            }

            uncompressedFiles[frontIdx] = uncompressedFiles[backIdx];
            uncompressedFiles[backIdx] = -1;

            frontIdx++;
            backIdx--;

        } while (frontIdx <= backIdx);

        var checksum = uncompressedFiles.Select((d, i) => d == -1 ? 0 : i * (decimal)d).Sum();
        return checksum;
    }

    protected override object Part2()
    {
        var files = new List<File>();
        var freeSpaces = new List<FreeSpace>();

        // Find files and freespace
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


        // Rearrange files
        files.Reverse();
        foreach (var file in files)
        {
            var freeSpace = freeSpaces.FirstOrDefault(fs => fs.Length >= file.Length && fs.StartIdx < file.StartIdx);
            if (freeSpace != null)
            {
                // There is a freespace that fit the file
                file.StartIdx = freeSpace.StartIdx;
                freeSpace.StartIdx += file.Length;
                freeSpace.Length -= file.Length;
            }
        }


        // Calculate checksum
        files = files.OrderBy(f => f.StartIdx).ToList();
        var lastFile = files.Last();
        var uncompressedFiles = Enumerable.Repeat(-1, lastFile.StartIdx + lastFile.Length).ToArray();
        foreach (var file in files)
        {

            for (int i = 0; i < file.Length; i++)
            {
                uncompressedFiles[file.StartIdx + i] = file.Id;
            }
        }

        var checksum = uncompressedFiles.Select((d, i) => d == -1 ? 0 : i * (decimal)d).Sum();
        return checksum;
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

    private static void PrintFiles(List<File> files)
    {
        var uncompressedFiles = Enumerable.Repeat(-1, files.Last().StartIdx + files.Last().Length).ToArray();
        foreach (var file in files)
        {
            for (int i = 0; i < file.Length; i++)
            {
                uncompressedFiles[file.StartIdx + i] = file.Id;
            }
        }

        Console.WriteLine($"1 {string.Join("", uncompressedFiles.Select(d => d == -1 ? " ." : d.ToString().PadLeft(2, ' ')))}");
    }
}