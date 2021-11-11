using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public enum DroneStatuses { available = 1, maintenance, busy };
        public enum parcelStatus { defined = 1, associated, collected, Provided };
        public enum PackageInTransferStatus { awaitingCollection = 0, OnTheWay };
    }
}
    
