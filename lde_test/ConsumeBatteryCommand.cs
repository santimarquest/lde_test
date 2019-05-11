using lde_test.Infrastructure;
using System;

namespace lde_test
{
    public class ConsumeBatteryCommand : IConsumeBatteryCommand
    {
        public int NeededBatteryCommand { get; set; }

        public event EventHandler<RobotEventArgs> LowBattery;

        public bool RobotHasEnoughBattery(Robot robot)
        {
            //var location = robot.Position.Location;
            LowBattery += OnLowBattery;

            if (robot.Battery - NeededBatteryCommand < 0)
            {
                //robot.Position.Location = location;
                LowBattery?.Invoke(this, new RobotEventArgs(robot));
                return false;
            }

            robot.Battery -= NeededBatteryCommand;
            return true;
        }

        private void OnLowBattery(object sender, RobotEventArgs robotEventArgs)
        {
            robotEventArgs._robot.ResetRobot();
        }
    }
}