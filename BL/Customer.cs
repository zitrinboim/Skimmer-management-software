using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class Customer : CustomerInParcel
    {
        public string phone { get; set; }
        public Location location { get; set; }
        public List<ParcelInCustomer> fromCustomers;
        public List<ParcelInCustomer> toCustomer;
        public override string ToString()
        {
            return string.Format(base.ToString() +
                "phone {0}: location-{1} ", phone, location +
                string.Join(" ", fromCustomers) +
                string.Join(" ", toCustomer));
        }
    }
}

