using DalApi;
using Dal;
using DO;
using DalXml;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalApi
{
    public static class DalFactory
    {
        public static IDal GetDal(string str)
        {
            if (str == "obj")
                return Dal.DalObject.instatnce;
            else if (str == "DalXml")
                return DalXml.DalXml.instatnce;
            else
                throw new ArgumentException($"{str}: isn't valid to \"DalFactory\"");
        }
    }
}
/*
 

namespace DalApi
{
    public static class DalFactory
    {
        public static IDal GetDal(string str)
        {
            if (str == "DalXml")
                return DalXml.DalXml.Instance;

            throw new ArgumentException($"{str}: isn't valid to \"DalFactory\"");
        }
    }
}
 */
