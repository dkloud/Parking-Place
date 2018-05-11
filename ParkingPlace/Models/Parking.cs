using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ParkingPlace.Models
{
    sealed class Parking
    {
        private static readonly Lazy<Parking> lazy = new Lazy<Parking>(() => new Parking());
        public static Parking Instance { get { return lazy.Value; } }

        public  List<Car> Cars { get; set; }
        private List<Transaction> Transactions { get; set; }
        private List<Transaction> TransactionsLastMinute { get; set; }
        private decimal Balance { get; set; }
        private decimal LastMinuteBalance { get; set; }
        private bool LogTransactionStart { get; set; }
        private Timer LogTimer;

        string filePath;

        private Parking()
        {
            Cars = new List<Car>();
            Transactions = new List<Transaction>();
            TransactionsLastMinute = new List<Transaction>();
            Balance = 0;
            LastMinuteBalance = 0;
            LogTransactionStart = false;
            filePath = Environment.CurrentDirectory + $"/Transactions {DateTime.Now.ToString("dd-MMMM-yyyy HH.mm.ss")}.log ";
        }

        public bool AddCar(Car car)
        {
            if (car != null && Cars.Count < Settings.ParkingPlace)
            {
                Cars.Add(car);
                car.CarTimer = new Timer();
                car.CarTimer.Elapsed += (sender, e) => {WithdrawParkingPrice(car); };
                car.CarTimer.Interval = Settings.Timeout;
                car.CarTimer.Enabled = true;

                if (LogTransactionStart == false)
                {
                    LogTransactionStart = true;
                    LogTimer = new Timer();
                    LogTimer.Elapsed += (sender, e) => { LogTransactions(); };
                    LogTimer.Interval = Settings.LogTimeout;
                    LogTimer.Enabled = true;
                }

                return true;
            }
            return false;
        }

        public bool RemoveCar(Car car)
        {
            if (Cars.Count != 0 && car != null)
            {
                if (car.Balance >= 0)
                {
                    car.CarTimer.Enabled = false;
                    car.CarTimer.Dispose();
                    if (Cars.Count == 0 && LogTransactionStart == true)
                    {
                        LogTransactionStart = false;
                        LogTimer.Dispose();
                    }
                    return Cars.Remove(car);
                }
                else
                    return false;
            }
            else
                return false;
        }

        private void WithdrawParkingPrice(Car car)
        {
            if (car.Balance >= Settings.ParkingPrice[car.CarType])
            {
                Transaction transaction = new Transaction(DateTime.Now, car.Id, Settings.ParkingPrice[car.CarType]);
                Balance = Balance + Settings.ParkingPrice[car.CarType];
                car.WithdrawBalance(Settings.ParkingPrice[car.CarType]);
                Transactions.Add(transaction);
            }
            else//price with fine
            {
                Transaction transaction = new Transaction(DateTime.Now, car.Id, Settings.ParkingPrice[car.CarType] * Settings.Fine);
                Balance = Balance + Settings.ParkingPrice[car.CarType] * Settings.Fine;
                car.WithdrawBalance(Settings.ParkingPrice[car.CarType] * Settings.Fine);
                Transactions.Add(transaction);
            }
        }

        public void ShowBalance(Action<string> showMethod)//output message in other way if needed
        {
            showMethod("Current Parking Place Balance is : " + Balance);
        }

        public void ShowBalance()
        {
             Console.WriteLine("Current Parking Place Balance is : " + Balance);
        }

        public void ShowCars()
        {
            if (Cars.Count != 0)
            {
                Console.WriteLine($"There are {Cars.Count} cars at the parking place:");
                foreach (Car car in Cars)
                {
                    Console.WriteLine($"Car id: {car.Id}, Car Type: {car.CarType} with such balance: {car.Balance}");
                }
            }
            else
                Console.WriteLine("There are no cars atm");
        }

        public void ShowFreeSpace()
        {
            if (Cars.Count != 0)
            {
                Console.WriteLine($"There is/are {Settings.ParkingPlace - Cars.Count}  free place(s) and {Cars.Count} is/are occupied.");
            }
            else
                Console.WriteLine($"There is/are {Settings.ParkingPlace} free place(s).");
        }

        private void LogTransactions()
        {
            if (Transactions.Count != 0)
            {
                TransactionsLastMinute = new List<Transaction>(Transactions);
                using (StreamWriter sw = new StreamWriter(filePath, true, System.Text.Encoding.Default))
                {
                    foreach (Transaction t in TransactionsLastMinute)
                    {
                        sw.WriteLine(t.ShowTransaction());
                        LastMinuteBalance = LastMinuteBalance + t.Money;
                    }
                    Transactions = new List<Transaction>();
                    TransactionsLastMinute = new List<Transaction>();
                }
            }
        }

        public void ShowTransactionsLog()
        {
            using (StreamReader sr = new StreamReader(filePath, Encoding.Default))
            {
                Console.WriteLine(sr.ReadToEnd());
            }
        }

        public async void ShowTransactionsLogAsync()
        {
            using (StreamReader sr = new StreamReader(filePath, Encoding.Default))
            {
                Console.WriteLine(await sr.ReadToEndAsync());
            }
        }

        public void TransactionHistoryLastMinute()
        {
            if (TransactionsLastMinute.Count != 0)
            {
                foreach (Transaction t in TransactionsLastMinute)
                {
                    Console.WriteLine(t.ShowTransaction());
                }
            }
            else
            {
                List<Transaction> tempTransactionList = new List<Transaction>(Transactions);
                foreach (Transaction t in tempTransactionList)
                {
                    Console.WriteLine(t.ShowTransaction());
                }
            }
        }

        public void ShowLastMinuteEarnedBalance()
        {
            Console.WriteLine("Money was earned for last minute: " + LastMinuteBalance);
        }
    }
}
