using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lugagesorting
{
    public class Gate : IOpenClose
    {
        private int _gateNumber;
        private bool _isOpen = false;
        private Lugage[] _planeLugage = new Lugage[50];
        public static Lugage[] _gateBuffer = new Lugage[15];
        private Thread _t;
        private FlightPlan _flightPlan;

        public int GateNumber
        {
            get { return _gateNumber; }
            set { _gateNumber = value; }
        }
        
        public bool IsOpen
        {
            get { return _isOpen; }
            set { _isOpen = value; }
        }
        
        public Lugage[] GateBuffer
        {
            get { return _gateBuffer; }
            set { _gateBuffer = value; }
        }

        public Thread T
        {
            get { return _t; }
            set { _t = value; }
        }

        public FlightPlan FlightPlan
        {
            get { return _flightPlan; }
            set { _flightPlan = value; }
        }

        public Gate(int gateNumber)
        {
            GateNumber = gateNumber;
            T = new Thread(Worker);
            T.Start();
        }

        public void Worker()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                
            }
        }
    }
}
