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
        public static Lugage[] _gateBuffer = new Lugage[25];
        private Thread _t;
        private FlightPlan _flightPlan;
        object _threadLock = new object();

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
                if (Monitor.TryEnter(_threadLock))
                {
                    //If gatebuffers index 0 isnt null
                    if (Manager.gates[GateNumber].GateBuffer[0] != null)
                    {
                        //get amount of flightplads
                        for (int i = 0; i < Manager.flightPlans.Length; i++)
                        {
                            //if the flightplan is empty, wait till generated
                            if (Manager.flightPlans[i] == null)
                            {
                                Monitor.Wait(_threadLock, 2000);
                            }

                            //If flightplans isnt null and gatebuffers index isnt null
                            if (Manager.flightPlans[i] != null && GateBuffer[i] != null)
                            {
                                //If flightplans planenumber on targeted index, is the same as the gates planenumber.
                                if (Manager.flightPlans[i].PlaneNumber == GateBuffer[0].PlaneNumber)
                                {
                                    //Used for debugging currently.
                                    double s = (Manager.flightPlans[i].DepartureTime - DateTime.Now).TotalSeconds;

                                    //Get departure time, and open or close gate based on the calculation in the if statement.
                                    if (((Manager.flightPlans[i].DepartureTime - DateTime.Now).TotalSeconds <= Manager.GateOpenDeparture) && ((Manager.flightPlans[i].DepartureTime - DateTime.Now).TotalSeconds >= Manager.GateCloseDeparture))
                                    {
                                        IsOpen = true;
                                    }
                                    else
                                    {
                                        IsOpen = false;
                                    }

                                    i = Manager.flightPlans.Length;

                                    //Used for debugging.
                                    Debug.WriteLine(s);

                                    //If the gate isOpen
                                    if (IsOpen)
                                    {
                                        //This here needs to clear the buffer on the gate, so it can clear the gate aka, plane can take off, and make space for a new plane.
                                        RetrieveFromGateBuffer();
                                    }
                                }
                            }
                        }
                    }
                    Monitor.PulseAll(_threadLock);
                    Monitor.Exit(_threadLock);
                }
            }
        }

        /// <summary>
        /// Add to the corretly targeted gate.
        /// </summary>
        /// <param name="lugage"></param>
        /// <returns>true false value, so it returns lugage if true for instance.</returns>
        public bool AddToGate(Lugage lugage)
        {
            if (_arrayIndex >= GateBuffer.Length)
            {
                return false;
            }
            GateBuffer[_arrayIndex] = lugage;
            _arrayIndex++;
            return true;
        }

        /// <summary>
        /// Retrieves lugage from the gate buffers.
        /// </summary>
        /// <returns>returns the lugage from the first index, so it works like a queue.</returns>
        public Lugage RetrieveFromGateBuffer()
        {
            if (GateBuffer == null)
            {
                return null;
            }
            //Sets our tempLugage to be equal to the lugage in spot 0 so we can save it for later, and move the rest of the lugages down in the array.
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

        /// <summary>
        /// Counts the amount of lugages in the array.
        /// </summary>
        /// <returns>The amount of lugage in the array (as int)</returns>
        public int AmountInGateArray()
        {
            int amountInArray = 0;
            for (int i = 0; i < GateBuffer.Length; i++)
            {
                if (GateBuffer[i] != null)
                {
                    amountInArray += 1;
                }
            }
            return amountInArray;
        }
    }
}
