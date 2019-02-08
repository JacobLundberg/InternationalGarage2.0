using System;
using System.ComponentModel.DataAnnotations;

namespace InternationalGarage2_0.Models
{
    public class Receipt
    {
        public string MemberName { get; set; }
        public string LicenseNumber { get; set; }
        public string Type { get; set; }
        public string Color { get; set; }
        public string Model { get; set; }
        public int NumberOfWheels { get; set; }
        public DateTime TimeStampCheckIn { get; set; }
        public DateTime TimeStampCheckOut { get; set; }
        //[DataType(DataType.Currency)]
        //public double Cash { get; set; }

        [DataType(DataType.Currency)]
        public string FeeDisplay { get; set; }
    }
}
