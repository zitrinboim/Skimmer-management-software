using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Parcel : abstractParcel
    {
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public DroneInParcel droneInParcel { get; set; }
        public DateTime Requested { get; set; }
        public DateTime Scheduled { get; set; }
        public DateTime PickedUp { get; set; }
        public DateTime Delivered { get; set; }
        public override string ToString()
        {
            return string.Format(base.ToString() + " Sender {0} : Target {1} : droneInParcel {2} :" +
                " Requested {3} : Scheduled {4} : PickedUp {5} : Delivered {6} :"
                , Sender, Target, droneInParcel, Requested, Scheduled, PickedUp, Delivered);
        }
    }

}
