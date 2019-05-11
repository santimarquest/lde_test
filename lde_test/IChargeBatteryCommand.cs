namespace lde_test
{
    public interface IChargeBatteryCommand
    {
        int Quantity { get; set; }
        void ChargeBattery(Robot robot);
    }
}