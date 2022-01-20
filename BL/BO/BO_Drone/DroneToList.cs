using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Class for DroneToList.
    /// </summary>
    public class DroneToList : abstractDrone
    {
        public int parcelNumber { get; set; }
        public override string ToString()
        {
            return string.Format(base.ToString() + "parcelNumber {0}\n\n", parcelNumber);
        }
    }
}

