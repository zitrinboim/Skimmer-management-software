using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public enum WeightCategories { easy = 1, medium, heavy };
    public enum Priorities { normal = 1, fast, emergency };
    public enum DroneStatuses { available = 1, maintenance, busy };
    public enum parcelStatus { defined = 1, associated, collected, Provided };
    public enum PackageInTransferStatus { awaitingCollection = 0, OnTheWay };
}

