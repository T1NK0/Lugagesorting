using System;
using System.Collections.Generic;
using System.Text;

namespace LugageSorterGUI
{
    class LugageSorterEvent : EventArgs
    {
        public int Amount { get; set; }
        public string Lugage { get; set; }

        public LugageSorterEvent(int amount, string lugage)
        {
            Amount = amount;
            Lugage = lugage;
        }
    }
}
