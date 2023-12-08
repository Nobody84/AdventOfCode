namespace AOC2023.Puzzels;

using System;
using System.Text.RegularExpressions;

public class Day8_HauntedWasteland
{
    private record Node(string Name, string LeftNode, string RightNode);

    public long Part1()
    {
        var lines = File.ReadAllLines("Inputs/Day8.txt");
        var instructions = lines[0];
        var nodes = this.GetNodes(lines.Skip(2).ToArray());

        var currentNode = nodes.First(n => n.Name == "AAA");
        var steps = 0;
        do
        {
            if (instructions[steps % instructions.Length] == 'L')
            {
                currentNode = nodes.First(n => n.Name == currentNode.LeftNode);
            }
            else
            {
                currentNode = nodes.First(n => n.Name == currentNode.RightNode);
            }

            steps++;
        } while (currentNode.Name != "ZZZ");

        return steps;
    }


    public long Part2()
    {
        var lines = File.ReadAllLines("Inputs/Day8.txt");
        var instructions = lines[0];
        var nodes = this.GetNodes(lines.Skip(2).ToArray());

        var currentNodes = nodes.Where(n => n.Name.EndsWith("A"));
        var steps = 0;
        do
        {
            if (instructions[steps % instructions.Length] == 'L')
            {
                currentNodes = currentNodes.Select(cn => nodes.First(n => n.Name == cn.LeftNode));
            }
            else
            {
                currentNodes = currentNodes.Select(cn => nodes.First(n => n.Name == cn.RightNode));
            }

            steps++;
        } while (currentNodes.Any(n => !n.Name.EndsWith("Z")));

        return steps;
    }
    private List<Node> GetNodes(string[] lines)
    {
        var nodes = new List<Node>();
        var nodeRegex = new Regex(@"^(?<name>[\dA-Z]{3})\s+=\s+\((?<left>[\dA-Z]{3}),\s+(?<right>[\dA-Z]{3})\)$");
        foreach (var line in lines)
        {
            var match = nodeRegex.Match(line);
            nodes.Add(new Node(match.Groups["name"].Value, match.Groups["left"].Value, match.Groups["right"].Value));
        }

        return nodes;
    }
}