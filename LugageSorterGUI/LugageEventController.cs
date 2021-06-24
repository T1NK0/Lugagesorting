using Lugagesorting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LugageSorterGUI
{
    class LugageEventController
    {
        public EventHandler lugageEventHandler;

        //Starts the thread to get the lugage count.
        public LugageEventController()
        {
            Thread _t = new Thread(GetLugageCount);
            _t.Start();
        }


        //Pushes the changes to the event
        private void GetLugageCount()
        {
            while (true)
            {
                int tempAmount = AmountInCounterArray();

                lugageEventHandler?.Invoke(this, new LugageEvent(tempAmount));
                Thread.Sleep(5000);
            }
        }

        //Measures the amount of luggage we have in the array
        public int AmountInCounterArray()
        {
            int AmountInArray = 0;

            for (int i = 0; i < Counter._counterLugageQueue.Length; i++)
            {
                if (Counter._counterLugageQueue[i] != null)
                {
                    AmountInArray += 1;
                }
            }
            return AmountInArray;
        }
    }
}
