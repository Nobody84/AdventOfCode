using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023.Puzzels.Day3
{
    internal class Number
    {
        public int X { get; init; }
        public int Y { get; init; }
        public string Value { get; init; } = string.Empty;

        public bool IsAdjacent(Point symbolPosition)
        {
            var isAdjacent = false;
            for (var offsetX = 0; offsetX < this.Value.Length; offsetX++)
            {
                isAdjacent |= Math.Abs((X + offsetX) - symbolPosition.X) <= 1 && Math.Abs(Y - symbolPosition.Y) <= 1;
            }

            return isAdjacent;
        }
    }
}
    