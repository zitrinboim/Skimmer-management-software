using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class abstractStationBase
    {
        public int Id { get; set; }
        public string name { get; set; }
        public int freeChargeSlots { get; set; }
        public override string ToString()
        {
            return string.Format("Station\nId {0}: name {1}: freeChargeSlots {2}: ", Id, name, freeChargeSlots);
        }
    }
}
