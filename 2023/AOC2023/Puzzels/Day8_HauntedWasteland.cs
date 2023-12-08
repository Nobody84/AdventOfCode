namespace AOC2023.Puzzels;

using System;
using System.Security;
using System.Text.RegularExpressions;

public class Day8_HauntedWasteland
{
    private record Node(string Name, string LeftNode, string RightNode);
    private record Loop(Node Node, int InitialSteps)
    {
        public int LoopSteps { get; set; }
    }

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

        var loops = new List<Loop>();

        var startNodes = nodes.Where(n => n.Name.EndsWith("A"));

        foreach (var node in startNodes)
        {
            var steps = 0;
            var currentLoops = new List<Loop>();
            var currentNode = node;
            do
            {
                currentNode = nodes.First(n => n.Name == this.GetNextNode(currentNode, instructions, steps));
                steps++;

                if (currentNode.Name.EndsWith("Z") && !currentLoops.Any(l => l.Node == currentNode))
                {
                    currentLoops.Add(new Loop(currentNode, steps));
                }
                else if (currentNode.Name.EndsWith("Z") && currentLoops.Any(l => l.Node == currentNode))
                {
                    var currentLoop = currentLoops.First(l => l.Node == currentNode);
                    currentLoop.LoopSteps = steps - currentLoop.InitialSteps;
                    loops.Add(currentLoop);
                    break;
                }

            } while (true);
        }

        long result = loops[0].LoopSteps;
        for (int i = 1; i < loops.Count; i++)
        {
            result = CalcKGV(result, (long)loops[i].LoopSteps);
        }


        return result;
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

    private string GetNextNode(Node node, string instructions, int steps)
    {
        if (instructions[steps % instructions.Length] == 'L')
        {
            return node.LeftNode;
        }
        else
        {
            return node.RightNode;
        }
    }

    private long CalcGGT(long a, long b)
    {
        while (b != 0)
        {
            var rest = a % b;
            a = b;
            b = rest;
        }
        return a;
    }

    private long CalcKGV(long a, long b)
    {
        var result =  (a * b) / this.CalcGGT(a, b);
        return result;
    }
}