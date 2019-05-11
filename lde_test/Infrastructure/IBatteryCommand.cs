namespace lde_test
{
    // Change battery commands 
    public interface IBatteryCommand
    {
        void ConsumeBattery(Robot robot);
        void ChargeBattery(Robot robot);
    }
}
