using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public static class DalFactory
    {
        public static IDal GetDal(string str)
        {
            if (str == "object")
                return Dal.DalObject.instatnce;
            //else if (str == "xml")
            //    /////
            //else
            //            throw;
            return default;
        }
    }
}
