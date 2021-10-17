using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public int Id;
            public string Model;
            public WeightCategories MaxWeight;
            public DroneStatuses Status;
            public double battery;
        }
    }
}

