﻿ using System;

namespace BO
{
    public class Location
    {
        public double longitude { get; set; }
        public double latitude { get; set; }
        public override string ToString()
        {
            return string.Format("{0}/{1}", longitude, latitude);
        }
    }
}
