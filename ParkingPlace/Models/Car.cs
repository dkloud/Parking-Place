using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ParkingPlace.Models
{
    enum CarType {
        Motorcycle = 1,
        Bus = 2,
        Passenger = 3,
        Truck = 5
    }
    class Car
    {
        static int count;
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public CarType CarType { get; set; }
        public Timer CarTimer { get; set; }

        static Car()
        {
            count = 0;
        }
        public Car(decimal balance, CarType carType)
        {
            Id = count;
            Balance = balance;
            CarType = carType;
            count++;
        }

        public void ReplenishBalance(decimal sum)
        {
            Balance += sum;
        }

        public void WithdrawBalance(decimal sum)
        {
            Balance -= sum;
        }

        public string ShowCar()
        {
            return string.Format($"Id - {Id}, car type - {CarType}");
        }
    }
}
