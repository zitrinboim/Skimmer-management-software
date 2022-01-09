using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
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
            return string.Format(base.ToString() + "Sender {0}\tTarget {1}\tdroneInParcel {2}\t" +
                "Requested {3}\tScheduled {4}\tPickedUp {5}\tDelivered {6}\n"
                , Sender, Target, droneInParcel, Requested, Scheduled, PickedUp, Delivered);
        }
    }

}
