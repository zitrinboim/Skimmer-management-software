using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// class for abstractStationBase.
    /// </summary>
    public class abstractStationBase
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int freeChargeSlots { get; set; }
        public override string ToString()
        {
            return string.Format("Id {0}\tname {1}\tfreeChargeSlots {2}\t", Id, name, freeChargeSlots);
        }
    }
}
