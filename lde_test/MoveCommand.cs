using System;
using System.Linq;
using lde_test.Infrastructure;

namespace lde_test
{
    public abstract class MoveCommand : ConsumeBatteryCommand, IRobotCommand
    {
        public CommandType Command;

        protected MoveCommand()
        {
            MoveOutOfRange += OnMoveOutOfRange;
            MoveObstacle += OnMoveObstacle;
            VisitedCell += OnVisitedCell;
        }

        public event EventHandler<RobotEventArgs> MoveOutOfRange;
        public event EventHandler<RobotEventArgs> MoveObstacle;
        public event EventHandler<RobotEventArgs> VisitedCell;

        public void ExecuteCommand(Robot robot)
        {
            var location = robot.Position.Location;
            var continueExploringMove = true;

            if (RobotHasEnoughBattery(robot))
            {
                robot.Position.Location = robot.MovingControl.Move(Command,
                    robot.Position.Facing, robot.Position.Location);              
            }
            else
            {
                continueExploringMove = false;
            }

            if (continueExploringMove && robot.IsRobotLocationOutOfRange(robot))
            {
                robot.Position.Location = location;
                MoveOutOfRange?.Invoke(this, new RobotEventArgs(robot));
                continueExploringMove = false;
            }

            if (continueExploringMove && robot.IsRobotLocationObstacle(robot))
            {
                robot.Position.Location = location;
                MoveObstacle?.Invoke(this, new RobotEventArgs(robot));
                continueExploringMove = false;
            }

            if (continueExploringMove && robot.IsRobotLocationVisitedCell(robot))
            {
                robot.Position.Location = location;
                VisitedCell?.Invoke(this, new RobotEventArgs(robot));
                continueExploringMove = false;
            }

            if (continueExploringMove)
            {
                robot.VisitedCells.Add(robot.Position.Location);
                robot._robotMemento = new Robot.RobotMemento(robot.Position, robot.Battery, robot.Strategies,
                    robot.FacingControl, robot.MovingControl);
            }
        }

        public void OnVisitedCell(object sender, RobotEventArgs robotEventArgs)
        {
            ResetRobot(robotEventArgs);
        }

        public void OnMoveOutOfRange(object sender, RobotEventArgs robotEventArgs)
        {
            ResetRobot(robotEventArgs);
        }

        public void OnMoveObstacle(object sender, RobotEventArgs robotEventArgs)
        {
            ResetRobot(robotEventArgs);
        }

        private static void ResetRobot(RobotEventArgs robotEventArgs)
        {
            robotEventArgs._robot.ResetRobot();
        }
    }   
}

