using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Lugagesorting
{
    public class Gate : IOpenClose
    {
        private int _gateNumber;
        private bool _isOpen;
        private int _arrayIndex = 0;
        private static Lugage[] _planeLugage = new Lugage[50];
        public static Lugage[] _gateBuffer = new Lugage[25];
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
                //Try and enter a thread using the lugage que as a lock
                if (Monitor.TryEnter(GateBuffer))
                {
                    for (int i = 0; i < Manager.flightPlans.Length; i++)
                    {
                        if (Manager.flightPlans[i] == null)
                        {
                            Monitor.Wait(GateBuffer, 2000);
                        }

                        if (Manager.flightPlans[i] != null && GateBuffer[i] != null)
                        {
                            double s = (Manager.flightPlans[i].DepartureTime - DateTime.Now).TotalSeconds;
                            if (((Manager.flightPlans[i].DepartureTime - DateTime.Now).TotalSeconds <= Manager.GateOpenDeparture) && ((Manager.flightPlans[i].DepartureTime - DateTime.Now).TotalSeconds >= Manager.GateCloseDeparture))
                            {
                                IsOpen = true;
                            }
                            else
                            {
                                IsOpen = false;
                            }

                            i = Manager.flightPlans.Length;
                            Debug.WriteLine(s);

                            if (IsOpen)
                            {
                                RetrieveFromGateBuffer();
                            }
                        }
                    }
                    Monitor.PulseAll(GateBuffer);
                    Monitor.Exit(GateBuffer);
                }
            }
        }

        public Lugage RetrieveFromGateBuffer()
        {
            if (GateBuffer == null)
            {
                return null;
            }
            Lugage tempLugage = GateBuffer[0];
            for (int i = 1; i < _arrayIndex; i++)
            {
                GateBuffer[i - 1] = GateBuffer[i];
            }

            if (_arrayIndex != 0)
            {
                GateBuffer[_arrayIndex - 1] = null;
            }

            if (_arrayIndex > 0)
            {
                _arrayIndex--;
            }
            return tempLugage;
        }

        public int AmountInCounterArray()
        {
            int AmountInArray = 0;
            for (int i = 0; i < GateBuffer.Length; i++)
            {
                if (GateBuffer[i] != null)
                {
                    AmountInArray += 1;
                }
            }
            return AmountInArray;
        }
    }
}
