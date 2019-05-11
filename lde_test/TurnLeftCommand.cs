namespace lde_test
{
    public class TurnLeftCommand :TurnCommand
    {
        public TurnLeftCommand ()
        {
            NeededBatteryCommand = 2;
        }

        public override void ExecuteCommand(Robot robot)
        {
            if (RobotHasEnoughBattery(robot))
            {
                robot.Position.Facing = robot.FacingControl.GetNextDirection(robot.Position.Facing, CommandType.TurnLeft);
                robot._robotMemento = new Robot.RobotMemento(robot.Position, robot.Battery, robot.Strategies, robot.FacingControl, robot.MovingControl);
            }
        }
    }   
}

