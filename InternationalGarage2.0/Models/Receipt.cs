using System;
using System.ComponentModel.DataAnnotations;

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
        [DataType(DataType.Currency)]
        public double Cash;
    }
}
