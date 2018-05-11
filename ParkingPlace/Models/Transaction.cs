using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingPlace.Models
{
    class Transaction
    {
        public DateTime TransactionTime { get; }
        public int CarId { get; }
        public decimal Money { get; }

        public Transaction(DateTime transactionTime, int carId, decimal withdrawnMoney)
        {
            TransactionTime = transactionTime;
            CarId = carId;
            Money = withdrawnMoney;
        }

        public string ShowTransaction()
        {
            return string.Format($"At {TransactionTime} was withdrawn {Money} from {CarId}");
        }
    }
}
