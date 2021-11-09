using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class ParcelInCustomer:abstractParcel
        {
            public parcelStatus parcelStatus { get; set; }
            public CustomerInParcel CustomerInParcel { get; set; }
            
        }
    }
}
