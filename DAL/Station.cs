using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IDAL
{
    namespace DO
    {
        public struct Station
        {
            public int Id { get; set; }
            public string name { get; set; }
            public double longitude { get; set; }
            public double lattitude { get; set; }
            public int ChargeSlots { get; set; }
            public override string ToString()
            {
                return string.Format("Station\nID {0}\t Name {1}\t longitude {2}\tlattitude {3}\t Charge slots {4} "
                    , Id, name, longitude, lattitude, ChargeSlots);
            }
        }
    }
}

