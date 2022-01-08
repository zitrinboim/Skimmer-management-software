using System;



    namespace DO
    {

        public struct Customer
        {
            public int Id { get; set; }
            public string name { get; set; }
            public string phone { get; set; }
            public double longitude { get; set; }
            public double lattitude { get; set; }
            public override string ToString()
            {
                return string.Format("Customer\nID {0}\tname {1}\tphone {2}\tlongitude {3}\tlattitude {4}",
                    Id, name, phone, longitude, lattitude);
            }
        }

    }



