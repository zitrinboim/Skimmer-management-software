using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class DroneInParcel : DroneInCargeing
    {
        public Location Location { get; set; }
        public override string ToString()
        {
            return string.Format(base.ToString() + "Location {0}:", Location);
        }

    }
}

