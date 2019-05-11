using System;
using System.Collections.Generic;

namespace lde_test
{
    public class FacingControl
    {

        static readonly LinkedList<Facing> Directions =
                        new LinkedList<Facing>(new[] { Facing.North, Facing.West, Facing.South, Facing.East });

        public readonly Dictionary<CommandType, Func<Facing, Facing>> SpinningFunctions =
                                new Dictionary<CommandType, Func<Facing, Facing>>
        {
        {CommandType.TurnLeft, TurnLeft},
        {CommandType.TurnRight, TurnRight},
        {CommandType.TakeSample, Stay },
        {CommandType.ExtendSolarPanels, Stay},
        };

        public Facing GetNextDirection(Facing currentDirection, CommandType stepCommandType)
        {
            return SpinningFunctions[stepCommandType](currentDirection);
        }

        private static Facing TurnRight(Facing currentDirection)
        {
            LinkedListNode<Facing> currentIndex = Directions.Find(currentDirection);
            return currentIndex.PreviousOrLast().Value;
        }

        private static Facing TurnLeft(Facing currentDirection)
        {
            LinkedListNode<Facing> currentIndex = Directions.Find(currentDirection);
            return currentIndex.NextOrFirst().Value;
        }

        private static Facing Stay(Facing currentDirection)
        {
            return currentDirection;
        }

    }

    public static class CircularLinkedList
    {
        public static LinkedListNode<T> NextOrFirst<T>(this LinkedListNode<T> current)
        {
            return current.Next ?? current.List.First;
        }

        public static LinkedListNode<T> PreviousOrLast<T>(this LinkedListNode<T> current)
        {
            return current.Previous ?? current.List.Last;
        }
    }
}
