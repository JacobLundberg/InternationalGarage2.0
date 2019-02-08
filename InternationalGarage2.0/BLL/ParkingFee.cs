using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternationalGarage2_0.BLL
{
    public class ParkingFee
    {

        public static double Calculate(double minutes)
        {
            return Math.Round(minutes);
        }

        public static double Calculate(TimeSpan span)
        {
            return Calculate(span.TotalMinutes);
        }

        public static double Calculate(DateTime checkIn, DateTime checkOut)
        {
            return Calculate(checkOut - checkIn);
        }

        public static string DisplayAsCurrency(double minutes)
        {
            var fee = Calculate(minutes);
            return fee.ToString("C");
        }

        public static string DisplayAsCurrency(DateTime checkIn, DateTime checkOut)
        {
            var fee = Calculate(checkIn, checkOut);
            return fee.ToString("C0");
        }
    }
}
