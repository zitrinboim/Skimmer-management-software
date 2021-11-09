using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class CustomerToList:CustomerInParcel
        {
            public string phone { get; set; }
            public int packagesProvided { get; set; }
            public int packagesNotYetDelivered { get; set; }
            public int receivedPackages { get; set; }
            public int PackagesOnTheWay { get; set; }
        }
    }
}
