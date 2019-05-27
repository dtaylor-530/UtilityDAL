using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityDAL.DemoApp
{
    public struct Price : IEquatable<Price>
    {
        public TeaTime.Time Date { get; set; }
        public double Bid { get; set; }
        public double Offer { get; set; }

        public bool Equals(Price other)
        {
            return Date == other.Date;
        }
    }


    public struct Pricecsv : IEquatable<Pricecsv>
    {
        public DateTime Date { get; set; }
        public double Bid { get; set; }
        public double Offer { get; set; }

        public bool Equals(Pricecsv other)
        {
            return Date == other.Date;
        }
    }

}
