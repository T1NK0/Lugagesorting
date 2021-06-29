using Lugagesorting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace LugageSorterGUI
{
    class LugageSorterEventController
    {
        public EventHandler lugageSorterEventHandler;
        object _threadlock = new object();

        public LugageSorterEventController()
        {
            Thread _t = new Thread(GetLugageCount);
            _t.Start();
        }

        private void GetLugageCount()
        {
            while (true)
            {
                if (Monitor.TryEnter(_threadlock))
                {
                    if (Manager.sorterConveyorbelt[0] == null)
                    {
                        Monitor.Wait(_threadlock, 2000);
                    }
                    else
                    {
                        int tempAmount = AmountInSorterArray();
                        string lugageNumber = Manager.sorterConveyorbelt[0].LugageNumber;
                        lugageSorterEventHandler?.Invoke(this, new LugageSorterEvent(tempAmount, lugageNumber));
                        Thread.Sleep(250);
                        Monitor.PulseAll(_threadlock);
                        Monitor.Exit(_threadlock);
                    }
                }

            }
        }

        //Measures the amount of luggage we have in the array
        public int AmountInSorterArray()
        {
            int AmountInArray = 0;

            for (int i = 0; i < Manager.sorterConveyorbelt.Length; i++)
            {
                if (Manager.sorterConveyorbelt[i] != null)
                {
                    AmountInArray += 1;
                }
            }
            return AmountInArray;
        }
    }
}
