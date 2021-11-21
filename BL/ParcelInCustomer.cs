using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    public class ParcelInCustomer : abstractParcel
    {
        public parcelStatus parcelStatus { get; set; }
        public CustomerInParcel CustomerInParcel { get; set; }
        public override string ToString()
        {
            return string.Format(base.ToString() + "parcelStatus {0}\tCustomerInParcel {1}\n", parcelStatus, CustomerInParcel);

        }
    }
}
