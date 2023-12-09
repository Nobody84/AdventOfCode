using System.Text.RegularExpressions;

namespace AOC2022.Puzzels
{
    public class Day4_CampCleanup
    {
        private record SectionsRange(int Start, int End);
        private record SectionsRagePair(SectionsRange SectionRange1, SectionsRange SectionRange2);

        public int PartOne()
        {
            var lines = File.ReadAllLines("Inputs/Day4.txt");
            var sectionsRagePairs = GetPairOfElves(lines);
            return sectionsRagePairs.Where(this.IsIncludeding).Count();
        }

        public int PartTwo()
        {
            var lines = File.ReadAllLines("Inputs/Day4.txt");
            var sectionsRagePairs = GetPairOfElves(lines);
            return sectionsRagePairs.Where(this.IsOverlapping).Count();
        }

        private List<SectionsRagePair> GetPairOfElves(string[] lines)
        {
            var sectionsRegex = new Regex(@"(?<start1>\d+)-(?<end1>\d+),(?<start2>\d+)-(?<end2>\d+)");
            return lines
                     .Select(lines => sectionsRegex.Match(lines))
                     .Select(match => new SectionsRagePair(
                                        new SectionsRange(int.Parse(match.Groups["start1"].Value), int.Parse(match.Groups["end1"].Value)),
                                        new SectionsRange(int.Parse(match.Groups["start2"].Value), int.Parse(match.Groups["end2"].Value))))
                                 .ToList();

        }

        private bool IsIncludeding(SectionsRagePair pair)
        {
            return (pair.SectionRange1.Start <= pair.SectionRange2.Start && pair.SectionRange1.End >= pair.SectionRange2.End) ||
                   (pair.SectionRange2.Start <= pair.SectionRange1.Start && pair.SectionRange2.End >= pair.SectionRange1.End);
        }

        private bool IsOverlapping(SectionsRagePair pair)
        {
            return (pair.SectionRange1.Start >= pair.SectionRange2.Start && pair.SectionRange1.Start <= pair.SectionRange2.End) ||
                   (pair.SectionRange2.Start >= pair.SectionRange1.Start && pair.SectionRange2.Start <= pair.SectionRange1.End);
        }
    }
}