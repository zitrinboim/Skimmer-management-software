using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    internal class DroneSimulator
    {
        enum Mintance {Starting, Going, charging };
        private const double VELOCITY = 0.1;
        private const int DELAY = 500;
        private const double TIME_STOP = DELAY/1000.0;
        private const double STOP = VELOCITY / TIME_STOP;

        public DroneSimulator(BL bL, int droneId, Action updateDrone, Func<bool> func)
        {
            var bl = bL;
           // var dl=bL.dal


        }
    }
}
