using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb2Threads
{
    internal class Car
    {
        public int CarID { get; set; }
        public string CarName { get; set; }
        public int ThreadId { get; set; }
        private double _speed;
        public double Speed
        {
            get { lock (speedLock) return _speed; }
            set { lock (speedLock) _speed = value; }
        }
        private double _distance;
        public double Distance
        {
            get { lock (distanceLock) return _distance; }
            set { lock (distanceLock) _distance = value; }
        }
        private readonly object speedLock = new object();
        private readonly object distanceLock = new object();

        public Car(int carid, string carname, double initialSpeed = 120)
        {
            CarID = carid;
            CarName = carname;
            _speed = initialSpeed;
            _distance = 0;
        }

    }
}
