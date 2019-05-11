using System;
using lde_test.Infrastructure;

namespace lde_test
{
    public abstract class TurnCommand : ConsumeBatteryCommand, IRobotCommand
    {

        public abstract void ExecuteCommand(Robot robot);
    }   
}

