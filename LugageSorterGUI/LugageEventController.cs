using Lugagesorting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LugageSorterGUI
{
    class LugageEventController
    {
        public EventHandler lugageCreationEventHandler;
        private int _counterNumber;
        object _threadlock = new object();

        public int CounterNumber
        {
            get { return _counterNumber; }
            set { CounterNumber = value; }
        }

        //Starts the thread to get the lugage count.
        public LugageEventController(int counterNumber)
        {
            this._counterNumber = counterNumber;
            Thread _t = new Thread(GetLugageCount);
            _t.Start();
        }


        //Pushes the changes to the event
        private void GetLugageCount()
        {
            while (true)
            {
                if (Monitor.TryEnter(_threadlock))
                {
                    if (Manager.counters[CounterNumber] == null)
                    {
                        Monitor.Wait(_threadlock, 2000);
                    }

                    else
                    {
                        int tempAmount = AmountInCounterArray();
                        int checkinCounterNumber = Manager.counters[CounterNumber].CounterNumber;
                        bool counterStatus = Manager.counters[CounterNumber].IsOpen;


                        lugageCreationEventHandler?.Invoke(this, new CheckinQueueEvent(tempAmount, checkinCounterNumber, counterStatus));
                        Thread.Sleep(1);
                        Monitor.PulseAll(_threadlock);
                        Monitor.Exit(_threadlock);
                    }
                }

            }
        }

        //Measures the amount of luggage we have in the array
        public int AmountInCounterArray()
        {
            int AmountInArray = 0;


            for (int i = 0; i < Manager.counters[CounterNumber].CounterLugageQueue.Length; i++)
            {
                if (Manager.counters[CounterNumber].CounterLugageQueue != null)
                {
                    AmountInArray += 1;
                }
            }

            return AmountInArray;
        }
    }
}
