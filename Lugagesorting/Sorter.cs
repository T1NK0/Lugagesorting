using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Lugagesorting
{
    public class Sorter
    {
        object _threadLock = new object();

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

        public static Lugage GetLugage(Lugage[] lugages, bool delete)
        {
            if (Monitor.TryEnter(lugages))
            {
                for (int i = 0; i < lugages.Length; i++)
                {
                    if (lugages[i] != null)
                    {
                        Lugage l = lugages[i];
                        if (delete == true) //Bool value set to true if we want to delete the item, and set to false if we want to check if there is an item in the array.
                        {   //Since we are consuming the item se set it to true, so we set the i of drinks to null.
                            lugages[i] = null;
                            Monitor.Pulse(lugages);
                        }
                        Monitor.Exit(lugages);
                        return l; //Returns the drink we have "found" in our splitter array.
                    }
                }
                Monitor.Exit(lugages);
            }
            return null; //Returns nothing since we have nothing in the array. Error message in splitter.
        }

        public static bool AddLugage(Lugage[] lugages, Lugage lugage)
        {
            if (Monitor.TryEnter(lugages))
            {
                for (int i = 0; i < lugages.Length; i++)
                {
                    if (lugages[i] == null)
                    {
                        lugages[i] = lugage; //We get our lugage from our "sorterBuffer".

                        Monitor.Pulse(lugages);
                        Monitor.Exit(lugages); //If we dont have exit here, we wont be able to exit if it has an empty spot..
                        return true; //Returns true if we have added a drink (hence the bool)
                    }
                }
                Monitor.Exit(lugages);
            }
            return false; //Returns false, if we have not added a drink.
        }

        public void SortBagage()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                if (Monitor.TryEnter(_threadLock))
                {
                    if (AmountInArray(Manager.sorterConveyorbelt) == 0)
                    {
                        //Console.WriteLine("Sorter buffer is empty");
                        Monitor.Wait(_threadLock, 2000);
                    }

                    Lugage lugage = GetLugage(Manager.sorterConveyorbelt, true); //This is the lugage we have gotten from GetLugage in buffer. Can work with whatever buffer we want to get Lugage from.
                    if (lugage != null) //Check if we have gotten a lugage, else it has no lugage left in the buffer (on the conveyor belt)
                    {
                        for (int i = 0; i < Manager.gates.Length; i++)
                        {
                            //NOT SURE HOW TO TARGET THE RIGHT GATE, BUT THINK I GOT THE LOGIC DOWN OTHERWISE
                            if (AddLugage(Manager.gates[i].GateBuffer, lugage) == false) //If false, no room. If true there is room for luage, then add the lugage to the buffer.
                            {
                                //if false (no room) writes out "No more space".
                                AddLugage(Manager.sorterConveyorbelt, lugage); //Adds the lugage in the buffer if there is no space for lugage, since we otherwise would delete the item.
                            }
                            else
                            {
                                if (Manager.gates[i].FlightPlan != null)
                                {
                                    //If lugage has the same flightnumber as the gate, send it to the current date.
                                    //Could be expanded with some "departure times".
                                    if (lugage.PlaneNumber == Manager.gates[i].FlightPlan.PlaneNumber)
                                    {
                                        Debug.WriteLine($"luggage {lugage.LugageNumber} added to gate {i}");
                                        AddLugage(Manager.gates[i].GateBuffer, lugage);
                                        i = Manager.gates.Length + 1;
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
}
