namespace lde_test
{
    public interface IConsumeBatteryCommand
    {
        int NeededBatteryCommand { get; set; }
        bool RobotHasEnoughBattery(Robot robot);
    }
}