using System;
using lde_test.Infrastructure;

namespace lde_test
{
    public class ExtendSolarPanelsCommand : IRobotCommand
    {
        private readonly IConsumeBatteryCommand _consumeBatteryCommand;
        private readonly IChargeBatteryCommand _chargeBatteryCommand;

        public event EventHandler<RobotEventArgs> MoveOutOfRange;
        public event EventHandler<RobotEventArgs> MoveObstacle;
        public event EventHandler<RobotEventArgs> VisitedCell;

        public ExtendSolarPanelsCommand(
            IConsumeBatteryCommand consumeBatteryCommand,
            IChargeBatteryCommand chargeBatteryCommand)
        {
            _consumeBatteryCommand = consumeBatteryCommand;
            _chargeBatteryCommand = chargeBatteryCommand;

            _consumeBatteryCommand.NeededBatteryCommand = 1;
            _chargeBatteryCommand.Quantity = 10;
        }

        public void ExecuteCommand(Robot robot)
        {
            if (_consumeBatteryCommand.RobotHasEnoughBattery(robot))
            {
                _chargeBatteryCommand.ChargeBattery(robot);
                robot._robotMemento = new Robot.RobotMemento(robot.Position, robot.Battery, robot.Strategies, robot.FacingControl, robot.MovingControl);
            }
        }
    }   
}

