namespace lde_test
{
    public class TurnRightCommand : TurnCommand
    {
        public TurnRightCommand()
        {
            NeededBatteryCommand = 2;
        }

        public override void ExecuteCommand(Robot robot)
        {
            if (RobotHasEnoughBattery(robot))
            {
                robot.Position.Facing = robot.FacingControl.GetNextDirection(robot.Position.Facing,
                    CommandType.TurnRight);
                robot._robotMemento = new Robot.RobotMemento(robot.Position, robot.Battery, robot.Strategies, robot.FacingControl, robot.MovingControl);
            }
        }
    }   
}

