using System;
using System.Collections.Generic;
using System.Text;

namespace LugageSorterGUI
{
    class GateEvent : EventArgs
    {
        public int GateNumber { get; set; }
        public bool Status { get; set; }
        public int Amount { get; set; }

        //The constructor
        public GateEvent(int amount, int gateNumber, bool status)
        {
            Amount = amount; //Sets it equal to our amount which we can change.
            GateNumber = gateNumber;
            Status = status;
        }
    }
}
