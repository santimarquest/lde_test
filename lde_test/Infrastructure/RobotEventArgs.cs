using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lde_test.Infrastructure
{
    public class RobotEventArgs : EventArgs
    {
        public Robot _robot { get; set; }

        public RobotEventArgs(Robot robot)
        {
            _robot = robot;
        }
    }
}
