namespace AOC2022.Puzzels
{
    using Microsoft.VisualBasic;
    using System;

    public class Day2
    {
        private readonly string[] inputLine;
        private Dictionary<string, int> scorePerShape = new Dictionary<string, int>
        {
            { "R", 1 }, // Rock
            { "P", 2 }, // Paper
            { "S", 3 }, // Scissor
        };

        private Dictionary<string, string> winningMap = new Dictionary<string, string>
        {
            { "S", "R" },
            { "P", "S" },
            { "R", "P" },
        };

        public Day2()
        {
            inputLine = File.ReadAllLines(@"./Inputs/day2.txt");
        }

        public int PartOne()
        {
            var score = 0;
            foreach (var line in inputLine)
            {
                var hands = line.Split(' ');
                score += this.GetScorePartOne(this.ToGeneralShape(hands[0]), this.ToGeneralShape(hands[1]));
            }

            return score;
        }

        public int PartTwo()
        {
            var score = 0;
            foreach (var line in inputLine)
            {
                var hands = line.Split(' ');
                score += this.GetScorePartTwo(this.ToGeneralShape(hands[0]), hands[1]);
            }

            return score;
        }

        public int GetScorePartOne(string opponentHand, string myHand)
        {
            var score = scorePerShape[myHand];
            if (opponentHand == myHand) // Draw
            {
                score += 3;
            }
            else if (myHand == winningMap[opponentHand]) // Win
            {
                score += 6;
            }

            return score;
        }

        public int GetScorePartTwo(string opponentHand, string targetOutcome)
        {
            var score = 0;
            switch (targetOutcome)
            {
                case "X": // Lose
                    score += 0;
                    score += this.scorePerShape[winningMap.First(i => i.Value == opponentHand).Key];
                    break;
                case "Y": // Draw
                    score += 3;
                    score += this.scorePerShape[opponentHand];
                    break;
                case "Z": // Win
                    score += 6;
                    score += this.scorePerShape[winningMap[opponentHand]];
                    break;
            }

            return score;
        }

        private string ToGeneralShape(string hand)
        {
            switch (hand)
            {
                case "A":
                case "X":
                    return "R";
                case "B":
                case "Y":
                    return "P";
                case "C":
                case "Z":
                    return "S";
                default: return hand;
            }
        }


    }
}