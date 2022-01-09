using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{

    public class IdExistExeptions : Exception
    {

        public IdExistExeptions(string messge, Exception innerExeption) : base(messge) { }
        public IdExistExeptions(string messge) : base(messge) { }
    }
    public class IdNotExistExeptions : Exception
    {
        public IdNotExistExeptions(string messge, Exception innerExeption) : base(messge) { }
        public IdNotExistExeptions(string messge) : base(messge) { }
    }
    public class ThereIsNoSuitablePackage : Exception
    {
        public ThereIsNoSuitablePackage(string messge, Exception innerExeption) : base(messge) { }
        public ThereIsNoSuitablePackage(string messge) : base(messge) { }
    }
    public class invalidValueForChargeSlots : Exception
    {
        public invalidValueForChargeSlots(string messge) : base(messge) { }
    }
}
