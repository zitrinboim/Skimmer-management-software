using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CustomerToList : CustomerInParcel
    {
        public string phone { get; set; }
        public int packagesProvided { get; set; }
        public int packagesNotYetDelivered { get; set; }
        public int receivedPackages { get; set; }
        public int PackagesOnTheWay { get; set; }
        public override string ToString()
        {
            return string.Format(base.ToString() + "phone {0}\tpackagesProvided {1}\t" +
                "packagesNotYetDelivered {2}\treceivedPackages {3}\tPackagesOnTheWay {4}\n\n"
                , phone, packagesProvided, packagesNotYetDelivered, receivedPackages, PackagesOnTheWay);
        }
    }
}

