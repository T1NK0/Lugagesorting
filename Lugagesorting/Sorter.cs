﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Lugagesorting
{
    public class Sorter
    {
        object _threadLock = new object();
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

        //public static Lugage GetLugage(Lugage[] lugages, bool delete)
        //{
        //    if (Monitor.TryEnter(lugages))
        //    {
        //        for (int i = 0; i < lugages.Length; i++)
        //        {
        //            if (lugages[i] != null)
        //            {
        //                Lugage l = lugages[i];
        //                if (delete == true) //Bool value set to true if we want to delete the item, and set to false if we want to check if there is an item in the array.
        //                {   //Since we are consuming the item se set it to true, so we set the i of drinks to null.
        //                    lugages[i] = null;
        //                    Monitor.Pulse(lugages);
        //                }
        //                Monitor.Exit(lugages);
        //                return l; //Returns the drink we have "found" in our splitter array.
        //            }
        //        }
        //        Monitor.Exit(lugages);
        //    }
        //    return null; //Returns nothing since we have nothing in the array. Error message in splitter.
        //}

        //public static bool AddLugage(Lugage[] lugages, Lugage lugage)
        //{
        //    if (Monitor.TryEnter(lugages))
        //    {
        //        for (int i = 0; i < lugages.Length; i++)
        //        {
        //            if (lugages[i] == null)
        //            {
        //                lugages[i] = lugage; //We get our lugage from our "sorterBuffer".

        //                Monitor.Pulse(lugages);
        //                Monitor.Exit(lugages); //If we dont have exit here, we wont be able to exit if it has an empty spot..
        //                return true; //Returns true if we have added a drink (hence the bool)
        //            }
        //        }
        //        Monitor.Exit(lugages);
        //    }
        //    return false; //Returns false, if we have not added a drink.
        //}

        public void SortBagage()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                if (Monitor.TryEnter(_threadLock))
                {
                    if (AmountInArray(Manager.sorterConveyorbelt) == 0)
                    {
                        Debug.WriteLine("Sorter buffer is empty");
                        Monitor.Wait(_threadLock, 2000);
                    }

                    for (int i = 0; i < Manager.gates.Length; i++)
                    {
                        if (Manager.gates[i].GateBuffer == null)
                        {
                            Lugage tempLugage = RetrieveFromSorterQueue();
                            if (Manager.gates[i].FlightPlan != null)
                            {
                                if (tempLugage != null)
                                {
                                    if (tempLugage.PlaneNumber == Manager.gates[i].FlightPlan.PlaneNumber)
                                    {
                                        DateTime currentTime = DateTime.Now;
                                        tempLugage.TimeStampSortingOut = currentTime;
                                        Manager.gates[i].GateBuffer[_arrayIndex] = tempLugage;
                                        _arrayIndex++;
                                        Debug.WriteLine($"luggage {tempLugage.LugageNumber} added to gate {i} at {currentTime}");
                                        i = Manager.gates.Length + 1;
                                    }
                                }
                            }
                        }
                    }
                }
                Monitor.PulseAll(_threadLock);
                Monitor.Exit(_threadLock);
            }
        }
    }
}
