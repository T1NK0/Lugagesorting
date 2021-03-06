using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Lugagesorting
{
    public class Counter : IOpenClose
    {
        Random random = new Random();
        private int _counterNumber;
        private bool _isOpen;
        private int _arrayIndex = 0;
        public static Lugage[] _counterLugageQueue = new Lugage[50];
        private Thread _t;

        public int CounterNumber
        {
            get { return _counterNumber; }
            set { _counterNumber = value; }
        }

        public bool IsOpen
        {
            get { return _isOpen; }
            set { _isOpen = value; }
        }

        public Lugage[] CounterLugageQueue
        {
            get { return _counterLugageQueue; }
            set { _counterLugageQueue = value; }
        }

        public Thread T
        {
            get { return _t; }
            set { _t = value; }
        }

        public Counter(int counterNumber)
        {
            CounterNumber = counterNumber;
            T = new Thread(Worker);
            T.Start();
        }

        public void Worker()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                //Try and enter a thread using the lugage que as a lock
                if (Monitor.TryEnter(CounterLugageQueue))
                {
                    for (int i = 0; i < Manager.flightPlans.Length; i++)
                    {
                        if (Manager.flightPlans[i] == null)
                        {
                            Monitor.Wait(CounterLugageQueue, 2000);
                        }
                        if (Manager.flightPlans[i] != null && CounterLugageQueue[i] != null)
                        {
                            double s = (Manager.flightPlans[i].DepartureTime - DateTime.Now).TotalSeconds;
                            if (((Manager.flightPlans[i].DepartureTime - DateTime.Now).TotalSeconds <= Manager.CounterOpenBeforeDeparture) && ((Manager.flightPlans[i].DepartureTime - DateTime.Now).TotalSeconds >= Manager.CounterCloseBeforeDeparture))
                            {
                                IsOpen = true;
                            }
                            else
                            {
                                IsOpen = false;
                            }
                            i = Manager.flightPlans.Length;
                            Debug.WriteLine(s);
                        }
                    }

                    //While the counter is open, do the following
                    if (IsOpen)
                    {
                        //And the lugageu queue is empty
                        while (AmountInCounterArray() == 0)
                        {
                            //tell the thread to wait for 2 seconds.
                            Debug.WriteLine($"Counter {CounterNumber} is waiting for luggage");
                            Monitor.Wait(CounterLugageQueue, 2000);
                        }

                        //Start checking in lugage.
                        CheckLugageIn();
                    }
                    Monitor.PulseAll(CounterLugageQueue);
                    Monitor.Exit(CounterLugageQueue);
                }
            }
        }

        /// <summary>
        /// Adds lugages to our counter's lugage queue, if the counter is open.
        /// </summary>
        /// <param name="lugage">used to make an arrayindex into a piece of lugage</param>
        /// <returns>true or false wether theres room for lugage</returns>
        public bool AddToCheckinCounterQueue(Lugage lugage)
        {
            if (_arrayIndex >= CounterLugageQueue.Length)
            {
                return false;
            }
            CounterLugageQueue[_arrayIndex] = lugage;
            _arrayIndex++;
            return true;
        }

        /// <summary>
        /// Retrieves the lugage from the queue so it can be chekced in.
        /// </summary>
        /// <returns>Either null or the temp lugage from array index 1, so that it consumes from the array as a queue.</returns>
        public Lugage RetrieveFromCounterQueue()
        {
            if (CounterLugageQueue == null)
            {
                return null;
            }
            Lugage tempLugage = CounterLugageQueue[0];
            for (int i = 1; i < _arrayIndex; i++)
            {
                CounterLugageQueue[i - 1] = CounterLugageQueue[i];
            }

            if (_arrayIndex != 0)
            {
                CounterLugageQueue[_arrayIndex - 1] = null;
            }

            if (_arrayIndex > 0)
            {
                _arrayIndex--;
            }
            return tempLugage;
        }

        ///// <summary>
        ///// Adds to an array like a queue
        ///// </summary>
        ///// <param name="lugage"></param>
        ///// <returns></returns>
        //public bool AddToCheckinCounterQueue(Lugage lugage)
        //{
        //    for (int i = 0; i < CounterLugageQueue.Length; i++)
        //    {
        //        if (CounterLugageQueue[i] == null)
        //        {
        //            CounterLugageQueue[i] = lugage;
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public Lugage RetrieveFromCounterQueue()
        //{
        //    for (int i = 0; i < CounterLugageQueue.Length; i++)
        //    {
        //        if (CounterLugageQueue[i] != null)
        //        {
        //            Lugage d = CounterLugageQueue[i];
        //            CounterLugageQueue[i] = null;
        //            return d;
        //        }
        //    }
        //    return null;
        //}

        /// <summary>
        /// Gets the amount in the counter array
        /// </summary>
        /// <returns>the current amount in the array</returns>
        public int AmountInCounterArray()
        {
            int AmountInArray = 0;
            for (int i = 0; i < CounterLugageQueue.Length; i++)
            {
                if (CounterLugageQueue[i] != null)
                {
                    AmountInArray += 1;
                }
            }
            return AmountInArray;
        }

        /// <summary>
        /// Checks our lugage in and adds it to our sorter conveyor.
        /// </summary>
        public void CheckLugageIn()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                if (Monitor.TryEnter(CounterLugageQueue))
                {
                    //if the amount in the array is 0 wait.
                    if (AmountInCounterArray() == 0)
                    {
                        Debug.WriteLine($"Counter {CounterNumber} queue is empty");
                        //Manager.PrintEvent?.Invoke(new DataPrinter($"Counter {CounterNumber} queue is empty", DataPrinter.DataTypePrint.ManagerData));
                        Monitor.Wait(CounterLugageQueue, 2000);
                    }

                    for (int i = 0; i < Manager.sorterConveyorbelt.Length; i++)
                    {
                        if (Manager.sorterConveyorbelt[i] == null)
                        {
                            Lugage tempLugage = RetrieveFromCounterQueue();
                            if (tempLugage != null)
                            {
                                //Set the datetime variable currentTime to now, so we can give lugage a timestamp.
                                DateTime currentTime = DateTime.Now;
                                tempLugage.TimeStampCheckin = currentTime;

                                Manager.sorterConveyorbelt[i] = tempLugage;
                                Debug.WriteLine($"{tempLugage.LugageNumber} has now been added to spot {i} on the conveyorbelt, with timestamp {tempLugage.TimeStampCheckin}");
                                Monitor.Wait(CounterLugageQueue, random.Next(0, 2500));
                            }
                        }
                    }
                    Monitor.PulseAll(CounterLugageQueue);
                    Monitor.Exit(CounterLugageQueue);
                }
            }
        }
    }
}
