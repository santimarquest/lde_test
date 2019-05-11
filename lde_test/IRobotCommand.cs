using System;
using lde_test.Infrastructure;

namespace lde_test
{
    public interface IRobotCommand
    {
        void ExecuteCommand(Robot robot);
    }
}