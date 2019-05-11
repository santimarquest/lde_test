using System;
using lde_test.Infrastructure;

namespace lde_test
{
    public class TakeSampleCommand : ConsumeBatteryCommand, IRobotCommand
    {

        public TakeSampleCommand()
        {
            NeededBatteryCommand = 8;
        }

        public void ExecuteCommand(Robot robot)
        {
            if (RobotHasEnoughBattery(robot))        
            {
                robot.SamplesCollected.Add(robot.Terrain[robot.Position.Location.X, robot.Position.Location.Y]);
                robot._robotMemento = new Robot.RobotMemento(robot.Position, robot.Battery, robot.Strategies, robot.FacingControl, robot.MovingControl);
            }
        }
    }   
}

