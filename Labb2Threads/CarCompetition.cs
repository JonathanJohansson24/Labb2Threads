using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Labb2Threads
{
    internal class CarCompetition
    {
        private static Random random = new Random();
        public List<Car> cars = new List<Car>();
        private volatile bool isCompetition = true;
        private object raceStartLock = new object();
        private object lockObject = new object();
        private int finishedCars = 0;
        public void AddCar(Car car)
        {
            cars.Add(car);
        }

        public void StartRace()
        {
            lock (raceStartLock)
            {
                Console.WriteLine("For information during the race, press enter");
                Console.ReadKey();
                foreach (Car item in cars)
                {
                    Console.WriteLine($"{item.CarName} is ready and in position.");
                }
                Console.WriteLine("All cars are at the starting line!");
                Console.ReadKey();
                Console.WriteLine("The race is starting...");
                Thread eventThread = new Thread(HandleRandomEvents);
                eventThread.Start();
                Thread inputThread = new Thread(HandleUserInput);
                inputThread.Start();
                foreach (Car car in cars)
                {
                    Thread carThread = new Thread(() =>
                    {
                        Console.WriteLine($"{car.CarName} starting on thread {Thread.CurrentThread.ManagedThreadId}");
                        car.ThreadId = Thread.CurrentThread.ManagedThreadId;
                        RunRace(car);
                    });
                    carThread.Start();
                }
            }
        }
        private void RunRace(Car car)
        {

            while (car.Distance < 10000) // REPRESENTERAR HUR LÅNG TÄVLINGEN SKALL VARA I METER 
            {
                car.Distance += car.Speed * (1.0 / 3600) * 1000;  // Simulerar körning per sekund
                Thread.Sleep(1000);  // Uppdaterar varje sekund

            }
            lock (lockObject)  // Låser för att hantera tillgång till delade resurser på ett trådsäkert sätt
            {
                finishedCars++;
                Console.WriteLine($"{car.CarName} reached the finish line and is #{finishedCars}!");

                if (finishedCars == cars.Count)
                {
                    isCompetition = false; // Avslutar tävlingen när alla bilar är i mål
                    Console.WriteLine("The Race has ended.");
                }
            }


        }
        private void HandleUserInput()
        {
            while (isCompetition)
            {
                if (Console.KeyAvailable && Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    Console.WriteLine($"\nInputThread: {Thread.CurrentThread.ManagedThreadId}\n");
                    Console.WriteLine("\nCurrent race status:\n");
                    foreach (var car in cars)
                    {
                        Console.WriteLine($"Car: {car.CarName} Thread: {car.ThreadId} Distance: {Math.Round(car.Distance)} Speed: {car.Speed} km/h\n");
                    }
                }
                Thread.Sleep(100); 
            }
        }
        private void HandleRandomEvents()
        {
            while (isCompetition)
            {
                Console.WriteLine($"EventThread: {Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(30000);  // ANGE HUR OFTA DET SKA HÄNDA RANDOM EVENTS VARJE 1000 REPRESENTERAR 1 SEKUND
                if (!isCompetition) break;
                foreach (Car car in cars)
                {
                    if (Monitor.TryEnter(car, TimeSpan.FromMilliseconds(500)))
                    {
                        try
                        {
                            TriggerRandomEvent(car);
                        }
                        finally
                        {
                            Monitor.Exit(car);
                        }
                    }
                }
                
            }
        }
        private void TriggerRandomEvent(Car car)
        {
            
            int eventNum = random.Next(1, 5);
            switch (eventNum)
            {
                case 1:
                    OutOfGas(car);
                    break;
                case 2:
                    FlatTire(car);
                    break;
                case 3:
                    BirdCrash(car);
                    break;
                case 4:
                    EngineTrouble(car);
                    break;
            }
        }
        public static void OutOfGas(Car car)
        {
            Random random = new Random();

            int possibility = random.Next(1, 51);

            if (possibility == 5)
            {
                car.Speed = 0;
                Console.WriteLine($"{car.CarName} has ran out of gas and has to refill the tank for 30sec");
                Thread.Sleep(30000);
                Console.WriteLine($"{car.CarName} completed filling up the tank");
                car.Speed = 120;
            }
            else
            {
                Console.WriteLine($"{car.CarName} didnt get into trouble");
            }

        }

        public static void FlatTire(Car car)
        {

            int possibility = random.Next(1, 26);

            if (possibility == 5)
            {
                car.Speed = 0;
                Console.WriteLine($"{car.CarName} has a flattire and has to fix it this will take 20sec");
                Thread.Sleep(20000);
                Console.WriteLine($"{car.CarName} has a new tire");
                car.Speed = 120;
            }
            else
            {
                Console.WriteLine($"{car.CarName} didnt get into trouble");
            }
        }

        public static void BirdCrash(Car car)
        {
            int possibility = random.Next(1, 11);

            if (possibility == 5)
            {
                car.Speed = 0;
                Console.WriteLine($"{car.CarName} has crashed with a bird and has to clean the windshield for 10sec");
                Thread.Sleep(10000);
                Console.WriteLine($"{car.CarName} has a clean windshield");
                car.Speed = 120;
            }
            else
            {
                Console.WriteLine($"{car.CarName} didnt get into trouble");
            }
        }

        public static void EngineTrouble(Car car)
        {
            int possibility = random.Next(1, 6);

            if (possibility == 5)
            {
                Console.WriteLine($"{car.CarName} has a faulty engine and the speed has decreased with 1km/h");
                car.Speed -= 1;
            }
            else
            {
                Console.WriteLine($"{car.CarName} didnt get into trouble");
            }
        }
    }
}
