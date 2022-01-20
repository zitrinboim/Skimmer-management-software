using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Class for customer.
    /// </summary>
    public class Customer : CustomerInParcel
    {
        public string phone { get; set; }
        public Location location { get; set; }
        public List<ParcelInCustomer> fromCustomer;
        public List<ParcelInCustomer> toCustomer;
        public override string ToString()
        {
            return string.Format(base.ToString() + "phone {0}\tlocation {1}", phone, location + "\nfromCustomer\n" +
                string.Join(" ", fromCustomer) + "toCustomer\n" +
                string.Join(" ", toCustomer));
        }
    }
}

