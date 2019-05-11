using System;
using lde_test.Infrastructure;

namespace lde_test
{
    public abstract class Command : IRobotCommand
    {
        public abstract event EventHandler<RobotEventArgs> MoveObstacle;
        public abstract event EventHandler<RobotEventArgs> MoveOutOfRange;
        public abstract event EventHandler<RobotEventArgs> VisitedCell;

        public abstract void ExecuteCommand(Robot robot);
    }

    public enum CommandType
    {
        MoveForward,
        MoveBackwards,
        TurnLeft,
        TurnRight,
        TakeSample,
        ExtendSolarPanels
    }
}
