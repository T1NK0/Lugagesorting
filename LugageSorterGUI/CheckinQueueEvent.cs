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
        //public int CounterNumber { get; set; }
        //public bool Status{ get; set; }

        //Creates a int we can keep track of the amount in the queue and print to the label
        private int _lugageInCounterQueue;

        public int LugageInCounterQueue
        {
            get { return _lugageInCounterQueue; }
            set { _lugageInCounterQueue = value; }
        }

        //The constructor
        //public CheckinQueueEvent(int amount, int counterNumber, bool status)
        public CheckinQueueEvent(int amount)
        {
            _lugageInCounterQueue = amount; //Sets it equal to our amount which we can change.
            //CounterNumber = counterNumber;
            //Status = status;
        }
    }
}
