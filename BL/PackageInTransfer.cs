using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class PackageInTransfer:abstractParcel
        {
            public PackageInTransferStatus packageInTransferStatus { get; set; }
            public CustomerInParcel sander { get; set; }
            public CustomerInParcel target { get; set; }
            public Location startingPoint { get; set; }
            public Location targetPoint { get; set; }
            public double distance { get; set; }
        }
    }
}
