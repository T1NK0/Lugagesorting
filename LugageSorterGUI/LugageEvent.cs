using System;
using System.Collections.Generic;
using System.Text;

namespace LugageSorterGUI
{
    /// <summary>
    /// Creates an event class to enable us to use an event.
    /// </summary>
    class LugageEvent : EventArgs
    {
        //Creates a int we can keep track of the amount in the queue and print to the label
        private int _lugageInCounterQueue;

        public int LugageInCounterQueue
        {
            get { return _lugageInCounterQueue; }
            set { _lugageInCounterQueue = value; }
        }

        //The constructor
        public LugageEvent(int amount)
        {
            _lugageInCounterQueue = amount; //Sets it equal to our amount which we can change.
        }
    }
}
