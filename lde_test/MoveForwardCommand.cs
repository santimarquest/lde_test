using System;
using System.Linq;
using lde_test.Infrastructure;

namespace lde_test
{
    public class MoveForwardCommand : MoveCommand
    {
        public MoveForwardCommand()
        {
            NeededBatteryCommand = 3;
            Command = CommandType.MoveForward;
        }
    }   
}

