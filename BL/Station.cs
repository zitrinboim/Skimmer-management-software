using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Station : abstractStationBase
        {
            public Location Location { get; set; }
            public List<DroneInCargeing> droneInCargeings;
        }
    }
}
