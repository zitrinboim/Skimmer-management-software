using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public class IdExistExeptions : Exception
        {
            public IdExistExeptions(string messge) : base(messge) { }
        }
        public class IdNotExistExeptions : Exception
        {
            public IdNotExistExeptions(string messge) : base(messge) { }
        }
    }
}
