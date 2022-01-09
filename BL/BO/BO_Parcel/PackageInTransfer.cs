using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class PackageInTransfer : abstractParcel
    {
        public PackageInTransferStatus packageInTransferStatus { get; set; }
        public CustomerInParcel sander { get; set; }
        public CustomerInParcel target { get; set; }
        public Location startingPoint { get; set; }
        public Location targetPoint { get; set; }
        public double distance { get; set; }
        public override string ToString()
        {
            return string.Format(base.ToString() + "packageInTransferStatus {0} : " +
                "sander {1}: target {2} : startingPoint {3} : targetPoint {4} : distance {5} :"
                , packageInTransferStatus, sander, target, startingPoint, targetPoint, distance);
        }
    }
}

