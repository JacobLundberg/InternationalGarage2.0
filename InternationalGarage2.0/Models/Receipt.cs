using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternationalGarage2_0.Models
{
    public class Receipt
    {
        public string LicenseNumber;
        public string Type;
        public string Color;
        public string Model;
        public int NumberOfWheels;
        public DateTime TimeStampCheckIn;
        public DateTime TimeStampCheckOut;
        public int Cash;
    }
}
