using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingPlace.Models;

namespace ParkingPlace
{
    class Program
    {
        static void Main(string[] args)
        {
            Parking parking_place = Parking.Instance;
            Car car;
            bool working = true;
            do
            {
                int choice;
                choice = ShowMenu();
                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        car = CreateCar();
                        if (car == null)
                        {
                            Console.WriteLine("Creating car error, please check input data and try again.");
                            break;
                        }
                        if (parking_place.AddCar(car))
                            Console.WriteLine("Car was added succesfully");
                        else
                            Console.WriteLine("Error! Car was not added.");
                        break;
                    case 2:
                        Console.Clear();
                        car = DeleteCar(parking_place);
                        if (car == null)
                        {
                            Console.WriteLine("Error. There is no such car, please check input data and try again.");
                            break;
                        }
                        if (car.Balance < 0)
                        {
                            Console.WriteLine("You could not remove this car because of its in debt now. Please, add money to balance and try again.");
                            if (ReplenishCarBalance(car))
                            {
                                Console.WriteLine("The car balance was succesfully replenished. Now you can remove from this car from the parking place.");
                            }
                            else
                                Console.WriteLine("Replenish operation was canceled.");
                            break;
                        }
                        if (parking_place.RemoveCar(car))
                            Console.WriteLine("Car was removed succesfully");
                        else
                            Console.WriteLine("Error! Car was not removed.");
                        break;
                    case 3:
                        Console.Clear();
                        parking_place.TransactionHistoryLastMinute();
                        break;
                    case 4:
                        Console.Clear();
                        parking_place.ShowBalance();
                        break;
                    case 5:
                        Console.Clear();
                        parking_place.ShowFreeSpace();
                        break;
                    case 6:
                        Console.Clear();
                        parking_place.ShowTransactionsLog();
                        break;
                    case 7:
                        Console.Clear();
                        parking_place.ShowCars();
                        break;
                    case 8:
                        Console.Clear();
                        parking_place.ShowLastMinuteEarnedBalance();
                        break;
                    case 9:
                        working = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Input Error!!");
                        break;
                }
            }
            while (working);
        }

        static int ShowMenu()
        {
            int choice;
            Console.WriteLine();
            Console.WriteLine("*** Parking Place Menu ***");
            Console.WriteLine();
            Console.WriteLine("Choose the number to execute action");
            Console.WriteLine("1. Add car to Parking place.");
            Console.WriteLine("2. Remove car from Parking place.");
            Console.WriteLine("3. Show transaction history for last minute.");
            Console.WriteLine("4. Show parking place current balance.");
            Console.WriteLine("5. Show free places at the parking place.");
            Console.WriteLine("6. Show Transaction.log file.");
            Console.WriteLine("7. Show cars.");
            Console.WriteLine("8. Show money that was earned for last minute.");
            Console.WriteLine("9. Quit the programm.");

            if (!Int32.TryParse(Console.ReadLine(), out choice))
            {
                return 0;
            }
            if (choice < 1 || choice > 9)
                return 0;

            return choice;
        }

        static Car CreateCar()
        {
            int typeChoice, balance;
            Car car;
            Console.WriteLine("Please, enter type of the car and it's cash balance.");
            Console.Write("First, choose the type of car: 1 - Motorcycle, 2 - Bus,3 - Passenger, 4 - Truck: ");
            if (!Int32.TryParse(Console.ReadLine(), out typeChoice))
            {
                return null;
            }
            if (typeChoice < 1 || typeChoice > 4)
                return null;
            Console.Write("Second, enter the balance of the car: ");
            if (!Int32.TryParse(Console.ReadLine(), out balance))
            {
                return null;
            }
            switch (typeChoice)
            {
                case 1:
                    car = new Car(balance, CarType.Motorcycle);
                    break;
                case 2:
                    car = new Car(balance, CarType.Bus);
                    break;
                case 3:
                    car = new Car(balance, CarType.Passenger);
                    break;
                case 4:
                    car = new Car(balance, CarType.Truck);
                    break;
                default:
                    car = null;
                    break;
            }
            return car;
        }

        static Car DeleteCar(Parking parking)
        {
            if (parking.Cars.Count == 0)
                return null;
            int carId;
            Console.WriteLine("Choose the id of car you want to remove from the following list.");
            parking.ShowCars();
            if (!Int32.TryParse(Console.ReadLine(), out carId))
            {
                return null;
            }
            //Car car = parking.Cars.FirstOrDefault(c => c.Id == carId);
            return parking.Cars.FirstOrDefault(c => c.Id == carId);
        }

        static bool ReplenishCarBalance(Car car)
        {
            Console.WriteLine("Do you want to replenish a car balance now? Type 'y' for yes or 'n'for no");
            string choice = Console.ReadLine();
            decimal sum;
            if (choice == "y")
            {
                Console.Write("Please, enter the sum you want to replenish: ");
                if (!Decimal.TryParse(Console.ReadLine(), out sum))
                {
                    Console.WriteLine("Input error!");
                    return false;
                }
                car.ReplenishBalance(sum);
                return true;
            }
            else
                return false;
        }
    }
}
