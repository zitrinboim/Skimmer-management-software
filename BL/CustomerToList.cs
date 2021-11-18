using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
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
            return string.Format(base.ToString() + "phone {0} : packagesProvided {1} :" +
                " packagesNotYetDelivered {2} :receivedPackages {3} : PackagesOnTheWay {4} :"
                , phone, packagesProvided, packagesNotYetDelivered, receivedPackages, PackagesOnTheWay);
        }
    }
}

