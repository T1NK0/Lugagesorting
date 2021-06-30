using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Lugagesorting
{
    public class Sorter
    {
        private int _arrayIndex = 0;

        public static int AmountInArray(Lugage[] lugage)
        {
            int AmountInArray = 0;
            for (int i = 0; i < Manager.sorterConveyorbelt.Length; i++)
            {
                if (lugage[i] != null)
                {
                    AmountInArray += 1;
                }
            }
            return AmountInArray;
        }

        /// <summary>
        /// Retrieves the lugage from the queue so it can be chekced in.
        /// </summary>
        /// <returns>Either null or the temp lugage from array index 1, so that it consumes from the array as a queue.</returns>
        public Lugage RetrieveFromSorterQueue()
        {
            if (Manager.sorterConveyorbelt == null)
            {
                return null;
            }
            Lugage tempLugage = Manager.sorterConveyorbelt[0];
            for (int i = 1; i < _arrayIndex; i++)
            {
                Manager.sorterConveyorbelt[i - 1] = Manager.sorterConveyorbelt[i];
            }

            if (_arrayIndex != 0)
            {
                Manager.sorterConveyorbelt[_arrayIndex - 1] = null;
            }

            if (_arrayIndex > 0)
            {
                _arrayIndex--;
            }
            return tempLugage;
        }

        public void SortLugage()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                //Try enter on the sorterbuffer
                if (Monitor.TryEnter(Manager.sorterConveyorbelt))
                {
                    //if the sorterbuffer is empty
                    if (AmountInArray(Manager.sorterConveyorbelt) == 0)
                    {
                        //Wait and say it's empty.
                        Debug.WriteLine("Sorter buffer is empty");
                        Monitor.Wait(Manager.sorterConveyorbelt, 2000);
                    }

                    for (int i = 0; i < Manager.gates.Length; i++)
                    {
                        //Sets our temp lugage
                        Lugage tempLugage = RetrieveFromSorterQueue();
                        //If the retrieveFromSorterQueue got a lugage, continue
                        if (tempLugage != null)
                        {
                            if (tempLugage.PlaneNumber == Manager.gates[i].FlightPlan.PlaneNumber)
                            {
                                for (int k = 0; k < Manager.gates[i].GateBuffer.Length; k++)
                                {
                                    //Loops through 25 times.. gotta break it somehow..
                                    if (Manager.gates[i].GateBuffer[k] == null)
                                    {
                                        Debug.WriteLine($"{tempLugage.LugageNumber} has been added to gate {Manager.gates[i].GateNumber}");
                                        tempLugage.TimeStampSortingOut = DateTime.Now;
                                        tempLugage.TimeStampGate = DateTime.Now;
                                        Manager.gates[i].AddToGate(tempLugage);
                                        k = Manager.gates[i].GateBuffer.Length + 1;
                                    }
                                }
                                //DateTime currentTime = DateTime.Now;
                                //tempLugage.TimeStampSortingOut = currentTime;
                                //Manager.gates[i].GateBuffer[_arrayIndex] = tempLugage;
                                //_arrayIndex++;
                                //Debug.WriteLine($"luggage {tempLugage.LugageNumber} added to gate {i} at {currentTime}");
                                //i = Manager.gates.Length + 1;
                                //Thread.Sleep(500);
                            }
                            //}
                        }
                        //}
                        Thread.Sleep(1);
                    }
                }
            }
            Monitor.PulseAll(Manager.sorterConveyorbelt);
            Monitor.Exit(Manager.sorterConveyorbelt);
        }
    }
}
