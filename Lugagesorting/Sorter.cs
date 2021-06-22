using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lugagesorting
{
    class Sorter
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="lugage"></param>
        ///// <returns></returns>
        //public bool AddToGateQueue(Lugage lugage)
        //{
        //    for (int i = 0; i < Manager.sorterConveyorbelt.Length; i++)
        //    {
        //        if (Manager.sorterConveyorbelt[i] == null)
        //        {
        //            Manager.sorterConveyorbelt[i] = lugage;
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public Lugage RetrieveFromSorterQueue()
        //{
        //    for (int i = 0; i < Manager.sorterConveyorbelt.Length; i++)
        //    {
        //        if (Manager.sorterConveyorbelt[i] != null)
        //        {
        //            Lugage d = Manager.sorterConveyorbelt[i];
        //            Manager.sorterConveyorbelt[i] = null;
        //            return d;
        //        }
        //    }
        //    return null;
        //}

        public static int AmountInSorterConveyorbelt()
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

        public static int AmountInGateConveyorbelt()
        {
            int AmountInArray = 0;
            for (int i = 0; i < Manager.gates.Length; i++)
            {
                for (int j = 0; j < Manager.gates[i].GateBuffer.Length; j++)
                {
                    if (Manager.gates[j].GateBuffer != null)
                    {
                        AmountInArray += 1;
                    }
                }
            }
            return AmountInArray;
        }

        public void SortBagage()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                Monitor.Enter(Manager.sorterConveyorbelt);

                while (AmountInSorterConveyorbelt() == 0)
                {
                    Console.WriteLine("Sorter conveyor is empty");
                    Monitor.Wait(Manager.sorterConveyorbelt);
                }

                for (int i = 0; i < Manager.sorterConveyorbelt.Length; i++)
                {
                    if (Manager.sorterConveyorbelt[i] != null)
                    {
                        Manager.sorterConveyorbelt[i].TimeStampSortingIn = DateTime.Now;
                        Console.WriteLine($"{Manager.sorterConveyorbelt[i].LugageNumber} has been checked in to sorting at {Manager.sorterConveyorbelt[i].TimeStampSortingIn}");

                        for (int j = 0; j < Manager.gates.Length; j++)
                        {
                            if (Manager.sorterConveyorbelt[i].PlaneNumber == Manager.gates[j].FlightPlan.PlaneNumber)
                            {
                                if (Monitor.TryEnter(Manager.gates[j]))
                                {
                                    while (Manager.gates[j].GateBuffer.Length <= AmountInGateConveyorbelt())
                                    {
                                        Console.WriteLine("Gate conveyorbelt is full");
                                        Monitor.Wait(Manager.gates[j], 2000);
                                    }

                                    //Else target the gates buffer, which then adds it to the gates container.
                                    

                                    Monitor.PulseAll(Manager.gates[j]);
                                    Monitor.Exit(Manager.gates[j]);
                                }
                            }
                        }
                    }
                }

                Monitor.Exit(Manager.sorterConveyorbelt);
            }
        }
    }
}
