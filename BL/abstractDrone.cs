using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IBL
{
    namespace BO
    {
        public class abstractDrone:DroneInParcel
        {
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatuses DroneStatuses { get; set; }
        }
    }
}
