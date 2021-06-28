using System;
using System.Collections.Generic;
using System.Text;

namespace LugageSorterGUI
{
    /// <summary>
    /// Creates an event class to enable us to use an event.
    /// </summary>
    class CheckinQueueEvent : EventArgs
    {
        public int CounterNumber { get; set; }
        public bool Status { get; set; }
        public int Amount { get; set; }

        //The constructor
        public CheckinQueueEvent(int amount, int counterNumber, bool status)
        {
            Amount = amount; //Sets it equal to our amount which we can change.
            CounterNumber = counterNumber;
            Status = status;
        }
    }
}
