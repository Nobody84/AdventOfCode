namespace AOC2024.Puzzels;

using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Day5_PrintQueue
{

    record PageOrderRule(int Page1, int Page2);
    record Update(HashSet<int> PageHashSet, List<int> Pages);

    public int Part1()
    {
        var count = 0;
        var inputLines = File.ReadLines("Inputs/Day5.txt");

        var pageOrderRules = new List<PageOrderRule>();
        var updates = new List<Update>();

        var pageOrderRulesProcessed = false;
        foreach (var line in inputLines)
        {
            if (string.IsNullOrEmpty(line))
            {
                pageOrderRulesProcessed = true;
                continue;
            }

            if (!pageOrderRulesProcessed)
            {
                var pages = line.Split("|").Select(int.Parse).ToArray();
                pageOrderRules.Add(new PageOrderRule(pages[0], pages[1]));
            }
            else
            {
                var pages = line.Split(",").Select(int.Parse).ToList();
                updates.Add(new Update(pages.ToHashSet(), pages));
            }
        }

        var validUpdates = new List<Update>();
        foreach (var update in updates)
        {
            var updateOrderOk = true;
            var relevantOrderRules = pageOrderRules.Where(r => update.PageHashSet.Contains(r.Page1) && update.PageHashSet.Contains(r.Page2)).ToList();
            foreach (var orderRule in relevantOrderRules)
            {
                var posPage1 = update.Pages.IndexOf(orderRule.Page1);
                var posPage2 = update.Pages.IndexOf(orderRule.Page2);
                if (posPage1 > posPage2)
                {
                    updateOrderOk = false;
                    break;
                }
            }

            if (updateOrderOk)
            {
                validUpdates.Add(update);
            }
        }

        foreach (var validUpdate in validUpdates)
        {
            count += validUpdate.Pages[validUpdate.Pages.Count / 2];
        }

        return count;
    }

    public int Part2()
    {
        var count = 0;
        var inputLines = File.ReadLines("Inputs/Day5.txt").ToList();

        return count;
    }
}