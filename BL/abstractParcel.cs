using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class abstractParcel
        {
            public int Id { get; set; }
            public WeightCategories weight { get; set; }
            public Priorities priority { get; set; }
        }
    }
}
