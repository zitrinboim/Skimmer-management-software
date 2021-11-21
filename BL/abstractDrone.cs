using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace IBL.BO
{
    public class abstractDrone : DroneInParcel
    {
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public DroneStatuses DroneStatuses { get; set; }
        public override string ToString()
        {
            return string.Format(base.ToString() + "Model {0}\tMaxWeight {1}\tDroneStatuses {2}\t", Model, MaxWeight, DroneStatuses);
        }
    }
}
