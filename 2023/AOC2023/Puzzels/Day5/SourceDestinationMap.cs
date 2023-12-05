namespace AOC2023.Puzzels.Day5
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    internal class SourceDestinationMap
    {
        private record MapValue(long SourceRangeStart, long DestinationRangeStart, long RangeLength, long RangeOffset);
        private List<MapValue> maps = new List<MapValue>();

        public SourceDestinationMap(string sourceType, string destinationType)
        {
            this.SourceType = sourceType;
            this.DestinationType = destinationType;
        }

        public string SourceType { get; }

        public string DestinationType { get; }

        public void AddMapValue(long sourceRangeStart, long destinationRangeStart, long rangeLength)
        {
            this.maps.Add(new MapValue(sourceRangeStart, destinationRangeStart, rangeLength, destinationRangeStart - sourceRangeStart));
        }

        public long Map(long source)
        {
            var destination = source;
            foreach (var map in this.maps)
            {
                if (source >= map.SourceRangeStart && source < map.SourceRangeStart + map.RangeLength)
                {
                    destination = source + map.RangeOffset;
                    break;
                }
            }

            return destination;
        }

        public long ReverseMap(long destination)
        {
            var source = destination;
            foreach (var map in this.maps)
            {
                if (destination >= map.DestinationRangeStart && destination < map.DestinationRangeStart + map.RangeLength)
                {
                    source = source - map.RangeOffset;
                    break;
                }
            }

            return source;
        }

        public long GetLowestMappedDestinationNumber()
        {
            return this.maps.Min(m => m.DestinationRangeStart);
        }
    }
}
