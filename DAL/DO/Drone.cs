using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    namespace DO
    {
        public struct Drone
        {
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public override string ToString()
            {
                return string.Format("Drone\nID {0}\tModel {1}\tMax Weight {2}\tStatus {3}\tbattery {4}"
                    , Id, Model, MaxWeight);
            }
        }
    }


