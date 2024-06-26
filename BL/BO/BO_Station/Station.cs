﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// class for station.
    /// </summary>
    public class Station : abstractStationBase
    {
        public Location location { get; set; }
        public List<DroneInCargeing> droneInCargeings;
        public override string ToString()
        {
            return string.Format(base.ToString() + "location {0}\t", location + "droneInCargeings\n"+ string.Join(" ", droneInCargeings));
        }
    }
}
