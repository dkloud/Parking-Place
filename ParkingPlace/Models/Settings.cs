using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlace.Models
{
    static class Settings
    {
        public static int Timeout { get; }//in milliseconds
        public static int LogTimeout { get; }//in milliseconds
        public static Dictionary<CarType, decimal> ParkingPrice { get; }
        public static int ParkingPlace { get; }
        public static decimal Fine { get;}

        static Settings()
        {
            Timeout = 3000; //in milliseconds
            LogTimeout = 60000; //in milliseconds
            ParkingPrice = new Dictionary<CarType, decimal>
            {
                {CarType.Motorcycle, 1},
                {CarType.Bus, 2},
                {CarType.Passenger, 3},
                {CarType.Truck, 5}
            };
            ParkingPlace = 10;
            Fine = 2;
        }
    }
}
