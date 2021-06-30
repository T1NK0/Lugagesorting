using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Lugagesorting
{
    public class LugageProducer
    {
        Random random = new Random();

        public void GenerateLugage()
        {
            while (Thread.CurrentThread.IsAlive)
            {
                //Run through for counter array length
                for (int i = 0; i < Manager.counters.Length; i++)
                {
                    //Create a counter for each run through
                    Counter counter = Manager.counters[i];

                    //For the lenght of the counters queue
                    for (int j = 0; j < Manager.counters[i].CounterLugageQueue.Length; j++)
                    {
                        //lock the thread on the counters queue/buffer.
                        if (Monitor.TryEnter(counter.CounterLugageQueue))
                        {
                            //Selects a random flightplan
                            int randomFlightplanIndex = random.Next(0, Manager.flightPlans.Length);

                            //If the flightplan on the random flightplan isnt null
                            if (Manager.flightPlans[randomFlightplanIndex] != null)
                            {
                                //lock on the flightplan
                                if (Monitor.TryEnter(Manager.flightPlans[randomFlightplanIndex]))
                                {
                                    string lugageNumber = Manager.flightPlans[randomFlightplanIndex].PlaneNumber.ToString() + random.Next(0, 50).ToString();

                                    //if (Manager.counters[i].CounterLugageQueue[j].LugageNumber == lugageNumber)
                                    //{
                                    //    lugageNumber = Manager.flightPlans[randomFlightplanIndex].PlaneNumber.ToString() + random.Next(0, 50).ToString();
                                    //}

                                    Lugage lugage = new Lugage(lugageNumber, random.Next(1, 10000), Manager.flightPlans[randomFlightplanIndex].PlaneNumber);

                                    Debug.WriteLine($"Lugage {lugage.LugageNumber} has been created in counter {counter.CounterNumber}");

                                    while (!counter.AddToCheckinCounterQueue(lugage))
                                    {
                                        Console.WriteLine($"Counter {counter.CounterNumber}'s queue is now full");
                                        Monitor.Wait(counter.CounterLugageQueue, 2000);
                                    }

                                    Monitor.PulseAll(Manager.flightPlans[randomFlightplanIndex]);
                                    Monitor.Exit(Manager.flightPlans[randomFlightplanIndex]);
                                }
                                Thread.Sleep(500);
                            }

                            Monitor.PulseAll(counter.CounterLugageQueue);
                            Monitor.Exit(counter.CounterLugageQueue);
                        }
                    }
                }
            }
        }
    }
}
