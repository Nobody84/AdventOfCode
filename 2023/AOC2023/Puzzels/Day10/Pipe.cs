using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2023.Puzzels.Day10
{
    public class Pipe
    {
        public Pipe(Point posisiton, char c, IEnumerable<Point> connections)
        {
            this.Position = posisiton;
            this.Char = c;
            this.Connections = connections.ToList();
        }

        public Point Position { get; }

        public char Char { get; }

        public List<Point> Connections { get; }

        public Point GetNextPosition(Point previousPosition)
        {
            return this.Connections.First(p => p != previousPosition);
        }
    }
}
