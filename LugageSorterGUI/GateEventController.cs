using Lugagesorting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace LugageSorterGUI
{
    class GateEventController
    {
        public EventHandler GateEventHandler;
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
                if (Manager.gates[GateNumber] != null)
                {
                    

                    int tempAmount = AmountInGateBuffer();

                    //Debug.WriteLine($"{tempAmount} {GateNumber}");

                    bool gateStatus = Manager.gates[GateNumber].IsOpen;

                    GateEventHandler?.Invoke(this, new GateEvent(tempAmount, GateNumber, gateStatus));
                    Thread.Sleep(1);
                }
            }

        }

        //Measures the amount of luggage we have in the array
        public int AmountInGateBuffer()
        {
            int AmountInArray = 0;

            for (int i = 0; i < Manager.gates[GateNumber].GateBuffer.Length; i++)
            {
                if (Manager.gates[GateNumber].GateBuffer != null)
                {
                    AmountInArray += 1;
                }
            }
            return AmountInArray;
        }

    }
}
