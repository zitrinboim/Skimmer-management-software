using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    /// <summary>
    /// Class for CustomerInParcel.
    /// </summary>
    public class CustomerInParcel
    {
        public int Id { get; set; }
        public string name { get; set; }
        public override string ToString()
        {
            return string.Format("Id {0}\t mane {1}\t", Id, name);
        }
    }
}

