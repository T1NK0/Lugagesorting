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
            Thread _t = new Thread(GetCurrentLuggage);
            _t.Start();
        }

        private void GetCurrentLuggage()
        {

        }

        private void Sorter()
        {

        }
    }
}
