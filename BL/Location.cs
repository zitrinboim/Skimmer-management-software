 using System;

namespace IBL.BO
{
    public class Location
    {
        public double longitude { get; set; }
        public double latitude { get; set; }
        public override string ToString()
        {
            return string.Format("longitude {0}: latitude {1}:", longitude, latitude);
        }
    }
}
