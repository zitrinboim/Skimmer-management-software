using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Class for DroneInCargeing.
    /// </summary>
    public class DroneInCargeing
    {
        public int Id { get; set; }
        public double battery { get; set; }
        public override string ToString()
        {
            return string.Format("Id {0}\tbattery {1}\t", (float)Id, (float)battery);
        }
    }
}
