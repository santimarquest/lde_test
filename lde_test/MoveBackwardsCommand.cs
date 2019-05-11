using System;
using System.Linq;
using lde_test.Infrastructure;

namespace lde_test
{
    public class MoveBackwardsCommand : MoveCommand
    {
        public MoveBackwardsCommand()
        {
            NeededBatteryCommand = 3;
            Command = CommandType.MoveBackwards;
        }
    }   
}

