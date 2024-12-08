namespace AOC2024.Puzzels;

using System.Collections.Generic;

public class Day5_PrintQueue : PuzzelBase
{
    public Day5_PrintQueue()
        : base(5, "Print Queue")
    {
    }

    record PageOrderRule(int Page1, int Page2);
    record Update(HashSet<int> PageHashSet, List<int> Pages);

    protected override object Part1()
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

        foreach (var update in validUpdates)
        {
            count += update.Pages[update.Pages.Count / 2];
        }

        return count;
    }

    protected override object Part2()
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

        var invalidUpdates = new List<Update>();
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

            if (!updateOrderOk)
            {
                invalidUpdates.Add(update);
            }
        }

        foreach (var invalidUpdate in invalidUpdates)
        {
            var relevantOrderRules = pageOrderRules.Where(r => invalidUpdate.PageHashSet.Contains(r.Page1) && invalidUpdate.PageHashSet.Contains(r.Page2)).ToList();

            for(var i = 0; i < relevantOrderRules.Count; i++)
            {
                var orderRule = relevantOrderRules[i];
                var posPage1 = invalidUpdate.Pages.IndexOf(orderRule.Page1);
                var posPage2 = invalidUpdate.Pages.IndexOf(orderRule.Page2);
                if (posPage1 > posPage2)
                {
                    invalidUpdate.Pages[posPage1] = orderRule.Page2;
                    invalidUpdate.Pages[posPage2] = orderRule.Page1;
                    i = 0;
                    continue;
                }
            }
        }

        foreach (var update in invalidUpdates)
        {
            count += update.Pages[update.Pages.Count / 2];
        }

        return count;
    }
}