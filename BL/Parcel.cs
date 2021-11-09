using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Parcel:abstractParcel
        {
            public CustomerInParcel Sender { get; set; }
            public CustomerInParcel Target { get; set; }
            public DroneInParcel droneInParcel { get; set; }
            public DateTime Requested { get; set; }
            public DateTime Scheduled { get; set; }
            public DateTime PickedUp { get; set; }
            public DateTime Delivered { get; set; }
        }
    }
}
