namespace lde_test
{
    public class ChargeBatteryCommand : IChargeBatteryCommand
    {  
        public int Quantity { get; set; }

        public void ChargeBattery(Robot robot)
        {
            robot.Battery += Quantity;
        }   
    }   
}

