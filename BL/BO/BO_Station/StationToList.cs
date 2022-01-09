using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class StationToList : abstractStationBase
    {
        public int busyChargeSlots { get; set; }
        public override string ToString()
        {
            return string.Format(base.ToString() +"busyChargeSlots {0}\n\n", busyChargeSlots);
        }
    }
}

