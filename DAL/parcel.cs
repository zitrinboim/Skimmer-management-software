using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




    namespace DO
    {
        public struct Parcel
        {
            public int Id { get; set; }
            public int SenderId { get; set; }
            public int TargetId { get; set; }
            public WeightCategories weight { get; set; }
            public Priorities priority { get; set; }
            public int DroneId { get; set; }
            public DateTime Requested { get; set; }
            public DateTime Scheduled { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }
            public override string ToString()
            {
                return string.Format("Parcel\nID {0}\t Sender Id {1}\tTarget Id {2}\t weight {3}\t priority {4}\t Drone Id {5}\n" +
                    "Time Requested {6}\t Time Scheduled {7}\t Time PickedUp {8}\t Time Delivered {9}\t"
                    , Id, SenderId, TargetId, weight, priority, DroneId, Requested, Scheduled, PickedUp, Delivered);
            }
        }
    }


