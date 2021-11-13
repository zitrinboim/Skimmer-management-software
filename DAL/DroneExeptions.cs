using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public class DroneExeptions : Exception
        {
            public DroneExeptions(string messge) : base(messge) { }
        }
    }
}
