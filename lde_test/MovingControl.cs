using System;
using System.Collections.Generic;

namespace lde_test
{
    public class MovingControl
    {
        public Dictionary<Facing, Func<Location, Location>> MoveForwardFunctions =
                                new Dictionary<Facing, Func<Location, Location>>
        {
        {Facing.North, MoveNorth},
        {Facing.West, MoveWest},
        {Facing.South, MoveSouth},
        {Facing.East, MoveEast}
        };

        public Dictionary<Facing, Func<Location, Location>> MoveBackwardFunctions =
                               new Dictionary<Facing, Func<Location, Location>>
       {
        {Facing.North, MoveSouth},
        {Facing.West, MoveEast},
        {Facing.South, MoveNorth},
        {Facing.East, MoveWest}
       };

        public Location Move(CommandType commandType, Facing currentDirection, Location currentCoordinates)
        {
            if (commandType == CommandType.MoveForward)
            {
                return MoveForwardFunctions[currentDirection](currentCoordinates);
            }

            if (commandType == CommandType.MoveBackwards)
            {
                return MoveBackwardFunctions[currentDirection](currentCoordinates);
            }

            return currentCoordinates;
        }

        private static Location MoveEast(Location coordinates)
        {
            return new Location(coordinates.X + 1, coordinates.Y);
        }

        private static Location MoveSouth(Location coordinates)
        {
            return new Location(coordinates.X, coordinates.Y - 1);
        }

        private static Location MoveWest(Location coordinates)
        {
            return new Location(coordinates.X - 1, coordinates.Y);
        }

        private static Location MoveNorth(Location coordinates)
        {
            return new Location(coordinates.X, coordinates.Y + 1);
        }
    }
}
