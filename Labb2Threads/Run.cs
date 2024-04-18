using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2Threads
{
    internal class Run
    {
        public static void RunApp()
        {
           
            CarCompetition competition = new CarCompetition();

            Car car1 = new Car(101, "Volvo");
            Car car2 = new Car(120, "Golf");
            Car car3 = new Car(144, "Porsche");
            Car car4 = new Car(77, "Lambourghini");
            competition.AddCar(car1);
            competition.AddCar(car2);
            competition.AddCar(car3);
            competition.AddCar(car4);
           

            competition.StartRace();

                
          
            

            Console.ReadKey();
        }
    }
}
