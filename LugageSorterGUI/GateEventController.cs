using Lugagesorting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LugageSorterGUI
{
    class GateEventController
    {
        public EventHandler GateCheckinEventHandler;
        object _threadLock = new object();
        private int _gateNumber;
        public int GateNumber
        {
            get { return _gateNumber; }
            set { _gateNumber = value; }
        }

        public GateEventController(int gateNumber)
        {
            this.GateNumber = gateNumber;
            Thread _t = new Thread(GetLugageCount);
            _t.Start();
        }

        private void GetLugageCount()
        {
            while (true)
            {
                if (Monitor.TryEnter(_threadLock))
                {
                    if (Manager.gates[GateNumber] == null)
                    {
                        Monitor.Wait(_threadLock, 2000);
                    }
                    else
                    {
                        int tempAmount = AmountInGateBuffer();
                        int gateNumber = Manager.gates[GateNumber].GateNumber;
                        bool gateStatus = Manager.gates[GateNumber].IsOpen;


                        GateCheckinEventHandler?.Invoke(this, new GateEvent(tempAmount, gateNumber, gateStatus));
                        Thread.Sleep(250);
                        Monitor.PulseAll(_threadLock);
                        Monitor.Exit(_threadLock);
                    }
                }

            }
        }

        //Measures the amount of luggage we have in the array
        public int AmountInGateBuffer()
        {
            int AmountInArray = 0;

            for (int i = 0; i < Gate._gateBuffer.Length; i++)
            {
                if (Gate._gateBuffer[i] != null)
                {
                    AmountInArray += 1;
                }
            }
            return AmountInArray;
        }

    }
}
