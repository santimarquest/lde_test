using System.Collections.Generic;
using System.Linq;

namespace lde_test
{
    public class Result
    {
        public Result(List<Location> visitedCells, List<ElementType> samplesCollected, Location location, Facing facing, int battery)
        {
            VisitedCells = visitedCells;
            SamplesCollected = string.Join(" - ", samplesCollected.Select(s => s.ToString()).ToArray());
            FinalPosition = new Position(location,facing);
            Battery = battery;

        }

        public List<Location> VisitedCells { get; set; }
        public string SamplesCollected { get; set; }
        public int Battery { get; set; }
        public Position FinalPosition { get; set; }               
    }
}