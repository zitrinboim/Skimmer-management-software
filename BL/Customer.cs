using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Customer: CustomerInParcel
        {
            public string phone { get; set; }
            public Location location { get; set; }
            public List<ParcelInCustomer> fromCustomers;
            public List<ParcelInCustomer> toCustomer; 
        }
    }
}
